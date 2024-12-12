using System;
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
				using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
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
			const int bufferSize = 4096; // 4KB 버퍼 크기

			try
			{
				using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, useAsync: true))
				using (var memoryStream = new MemoryStream())
				{
					byte[] buffer = new byte[bufferSize];
					int bytesRead;

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
	}
}

