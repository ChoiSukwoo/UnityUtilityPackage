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
		/// <summary> byte[] 데이터를 지정된 경로에 저장합니다 </summary>
		public static async UniTask SaveBytesToFileAsync(byte[] data, string filePath, CancellationToken cancellationToken = default)
		{
			try
			{
				string directory = Path.GetDirectoryName(filePath);

				//경로 없을시 생성
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				//파일 존재시 삭제
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					Debug.Log($"Existing file deleted: {filePath}");
				}

				int bufferSize = GetOptimalBufferSize(data.Length);

				using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true))
				{
					cancellationToken.ThrowIfCancellationRequested(); // 취소 요청 확인
					await fileStream.WriteAsync(data, 0, data.Length, cancellationToken);
				}

				cancellationToken.ThrowIfCancellationRequested(); // 마지막으로 취소 요청 확인
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[BinaryUtility] Operation canceled 발생 [SaveBytesToFileAsync] 작업이 취소 되었습니다.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new IOException($"[BinaryUtility] 파일 저장 중 오류 발생: {ex.Message}", ex);
			}
		}

		/// <summary> 지정된 경로에서 byte[] 데이터를 비동기로 읽어옵니다 </summary>
		public static async UniTask<byte[]> LoadFromFileAsync(string filePath, CancellationToken cancellationToken = default)
		{
			int bufferSize = GetOptimalBufferSize(filePath); // 버퍼 크기 결정

			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
				using (MemoryStream memoryStream = new MemoryStream())
				{
					byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
					int bytesRead;

					try
					{
						// 버퍼 단위로 읽어서 메모리 스트림에 기록
						while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
						{
							cancellationToken.ThrowIfCancellationRequested(); // 취소 요청 확인
							await memoryStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
						}

						cancellationToken.ThrowIfCancellationRequested(); // 마지막으로 취소 요청 확인

						// 메모리 스트림에서 byte[]로 변환
						return memoryStream.ToArray();
					}
					finally
					{
						// 버퍼 반환
						ArrayPool<byte>.Shared.Return(buffer);
					}
				}
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[BinaryUtility] Operation canceled 발생 [LoadFromFileAsync] 작업이 취소 되었습니다.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new IOException($"[BinaryUtility] LoadFromFileAsync\n파일 읽기 중 오류 발생: {ex.Message}", ex);
			}
		}

		/// <summary> byte[] 데이터를 임시 파일로 저장하고 경로를 반환합니다 </summary>
		public static async UniTask<string> SaveBytesToTempFileAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			string uniqueFileName = $"temp_audio_{Guid.NewGuid()}.tmp";
			string tempPath = Path.Combine(Application.temporaryCachePath, uniqueFileName);
			await SaveBytesToFileAsync(data, tempPath, cancellationToken);
			return tempPath;
		}


		//파일 크기에 따라 버퍼 크기 변경
		public static int GetOptimalBufferSize(string filePath)
		{
			// 경로 없을시 기본 버퍼 크기 512KB
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

