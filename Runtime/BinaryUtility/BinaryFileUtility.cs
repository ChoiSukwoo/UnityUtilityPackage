using System;
using System.Buffers;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Suk.BinaryUtility
{
	public static class BinaryFileUtility
	{
		/// <summary> byte[] �����͸� ������ ��ο� �����մϴ� </summary>
		public static async UniTask SaveBytesToFileAsync(byte[] data, string filePath, CancellationToken cancellationToken = default)
		{
			try
			{
				string directory = Path.GetDirectoryName(filePath);

				//��� ������ ����
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				//���� ����� ����
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					Debug.Log($"Existing file deleted: {filePath}");
				}

				int bufferSize = GetOptimalBufferSize(data.Length);

				using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true))
				{
					cancellationToken.ThrowIfCancellationRequested(); // ��� ��û Ȯ��
					await fileStream.WriteAsync(data, 0, data.Length, cancellationToken);
				}

				cancellationToken.ThrowIfCancellationRequested(); // ���������� ��� ��û Ȯ��
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[BinaryUtility] Operation canceled �߻� [SaveBytesToFileAsync] �۾��� ��� �Ǿ����ϴ�.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new IOException($"[BinaryUtility] ���� ���� �� ���� �߻�: {ex.Message}", ex);
			}
		}

		/// <summary> ������ ��ο��� byte[] �����͸� �񵿱�� �о�ɴϴ� </summary>
		public static async UniTask<byte[]> LoadFromFileAsync(string filePath, CancellationToken cancellationToken = default)
		{
			int bufferSize = GetOptimalBufferSize(filePath); // ���� ũ�� ����

			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
				using (MemoryStream memoryStream = new MemoryStream())
				{
					byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
					int bytesRead;

					try
					{
						// ���� ������ �о �޸� ��Ʈ���� ���
						while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
						{
							cancellationToken.ThrowIfCancellationRequested(); // ��� ��û Ȯ��
							await memoryStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
						}

						cancellationToken.ThrowIfCancellationRequested(); // ���������� ��� ��û Ȯ��

						// �޸� ��Ʈ������ byte[]�� ��ȯ
						return memoryStream.ToArray();
					}
					finally
					{
						// ���� ��ȯ
						ArrayPool<byte>.Shared.Return(buffer);
					}
				}
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[BinaryUtility] Operation canceled �߻� [LoadFromFileAsync] �۾��� ��� �Ǿ����ϴ�.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new IOException($"[BinaryUtility] LoadFromFileAsync\n���� �б� �� ���� �߻�: {ex.Message}", ex);
			}
		}

		/// <summary> byte[] �����͸� �ӽ� ���Ϸ� �����ϰ� ��θ� ��ȯ�մϴ� </summary>
		public static async UniTask<string> SaveBytesToTempFileAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			string uniqueFileName = $"temp_audio_{Guid.NewGuid()}.tmp";
			string tempPath = Path.Combine(Application.temporaryCachePath, uniqueFileName);
			await SaveBytesToFileAsync(data, tempPath, cancellationToken);
			return tempPath;
		}


		//���� ũ�⿡ ���� ���� ũ�� ����
		public static int GetOptimalBufferSize(string filePath)
		{
			// ��� ������ �⺻ ���� ũ�� 512KB
			if (!File.Exists(filePath))
				return 524288;

			FileInfo fileInfo = new FileInfo(filePath);
			return GetOptimalBufferSize(fileInfo.Length);
		}


		public static int GetOptimalBufferSize(long dataSize)
		{
			const int smallFileBuffer = 524288; // 512KB
			const int largeFileBuffer = 2097152; // 2MB
			const long fileSizeThreshold = 500 * 1024 * 1024; // 500MB

			return dataSize > fileSizeThreshold ? largeFileBuffer : smallFileBuffer;
		}
	}
}

