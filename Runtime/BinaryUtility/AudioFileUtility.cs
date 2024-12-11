using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiUtility;

namespace Suk.BinaryUtility
{
	public static class AudioFileUtility
	{
		/// <summary> byte[] 데이터를 사용하여 AudioClip을 비동기로 생성합니다 </summary>
		public static async UniTask<AudioClip> CreateAudioClipAsync(byte[] audioData, AudioType audioType, UnityAction<float> onProgress = null, CancellationToken cancellationToken = default)
		{
			//1. 오디오 타입 체크
			if (audioType == AudioType.UNKNOWN)
				throw new ArgumentException($"[AudioFileUtility] CreateAudioClipAsync\n지원되지 않는 AudioType입니다.", nameof(audioType));

			string tempFilePath = null;
			try
			{
				tempFilePath = await BinaryFileUtility.SaveBytesToTempFileAsync(audioData, cancellationToken); //2. 바이너리 데이터를 임시 파일로 저장
				AudioClip audioClip = await LoadAudioClipFromFileAsync(tempFilePath, audioType, onProgress, cancellationToken); //3. 임시 파일을 AudioClip로 로드
				return audioClip;
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[AudioFileUtility] CreateAudioClipAsync\n작업이 취소되었습니다: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"[AudioFileUtility] CreateAudioClipAsync\n{ex.Message}", ex);
			}
			finally
			{
				// 4. 임시 파일 삭제
				if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
				{
					try
					{ File.Delete(tempFilePath); }
					catch (Exception ex) { Debug.LogWarning($"[AudioFileUtility] CreateAudioClipAsync\n임시 파일 삭제 중 오류 발생: {ex.Message}"); }
				}
			}
		}

		/// <summary> 임시 파일에서 AudioClip을 비동기로 로드합니다. </summary>
		private static async UniTask<AudioClip> LoadAudioClipFromFileAsync(string filePath, AudioType audioType, UnityAction<float> onProgress = null, CancellationToken cancellationToken = default)
		{
			try
			{
				string fileUrl = "file://" + filePath;
				using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType))
				{
					UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();

					await UpdateProgress(asyncOperation, onProgress, cancellationToken);

					if (request.result != UnityWebRequest.Result.Success)
						throw new Exception($"[AudioFileUtility] LoadAudioClipFromFileAsync\nUnityWebRequest Error: {request.error}");

					return DownloadHandlerAudioClip.GetContent(request);
				}
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[AudioFileUtility] LoadAudioClipFromFileAsync\n작업이 취소되었습니다: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"[AudioFileUtility] LoadAudioClipFromFileAsync\n {ex}");
			}
		}
	}
}
