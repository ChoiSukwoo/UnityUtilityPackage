using System;
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
				using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
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
			const int bufferSize = 4096; // 4KB ���� ũ��

			try
			{
				using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
				using (var memoryStream = new MemoryStream())
				{
					byte[] buffer = new byte[bufferSize];
					int bytesRead;

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
	}
}

