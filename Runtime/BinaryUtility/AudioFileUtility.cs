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
		/// <summary> byte[] �����͸� ����Ͽ� AudioClip�� �񵿱�� �����մϴ� </summary>
		public static async UniTask<AudioClip> CreateAudioClipAsync(byte[] audioData, AudioType audioType, UnityAction<float> onProgress = null, CancellationToken cancellationToken = default)
		{
			//1. ����� Ÿ�� üũ
			if (audioType == AudioType.UNKNOWN)
				throw new ArgumentException($"[AudioFileUtility] CreateAudioClipAsync\n�������� �ʴ� AudioType�Դϴ�.", nameof(audioType));

			string tempFilePath = null;
			try
			{
				tempFilePath = await BinaryFileUtility.SaveBytesToTempFileAsync(audioData, cancellationToken); //2. ���̳ʸ� �����͸� �ӽ� ���Ϸ� ����
				AudioClip audioClip = await LoadAudioClipFromFileAsync(tempFilePath, audioType, onProgress, cancellationToken); //3. �ӽ� ������ AudioClip�� �ε�
				return audioClip;
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[AudioFileUtility] CreateAudioClipAsync\n�۾��� ��ҵǾ����ϴ�: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"[AudioFileUtility] CreateAudioClipAsync\n{ex.Message}", ex);
			}
			finally
			{
				// 4. �ӽ� ���� ����
				if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
				{
					try
					{ File.Delete(tempFilePath); }
					catch (Exception ex) { Debug.LogWarning($"[AudioFileUtility] CreateAudioClipAsync\n�ӽ� ���� ���� �� ���� �߻�: {ex.Message}"); }
				}
			}
		}

		/// <summary> �ӽ� ���Ͽ��� AudioClip�� �񵿱�� �ε��մϴ�. </summary>
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
				throw new OperationCanceledException($"[AudioFileUtility] LoadAudioClipFromFileAsync\n�۾��� ��ҵǾ����ϴ�: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"[AudioFileUtility] LoadAudioClipFromFileAsync\n {ex}");
			}
		}
	}
}
