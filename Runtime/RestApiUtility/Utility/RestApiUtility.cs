using Cysharp.Threading.Tasks;
using Suk.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiState;

namespace Suk.RestApi {
	internal static class RestApiUtility {

		//인증 토큰 헤더
		public static void SetAuthHeader(ref Dictionary<string, string> headers, string authToken) {
			if(headers == null)
				headers = new Dictionary<string, string>();
			headers["Authorization"] = $"Bearer {authToken}";
		}

		public static void SetContentHeader(ref Dictionary<string, string> headers, string contentType) {
			if(headers == null)
				headers = new Dictionary<string, string>();
			headers["Content-Type"] = contentType;
		}

		//헤더 설정
		public static void SetRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers) {
			if(headers != null) {
				foreach((string key, string value) in headers) {
					request.SetRequestHeader(key, value);
				}
			}
		}

		public static DownloadHandler CreateDownloadHandler(string url, ContentTypeState expectedType, AudioType audioType = AudioType.UNKNOWN, uint crc = 0) {
			switch(expectedType) {
				case ContentTypeState.Audio:
				return new DownloadHandlerAudioClip(url, audioType);
				case ContentTypeState.Image:
				return new DownloadHandlerTexture();
				case ContentTypeState.Asset:
				return new DownloadHandlerAssetBundle(url, crc);
				default:
				return new DownloadHandlerBuffer(); // 기본 핸들러
			}
		}


		// Content-Type 유효성 검사 함수
		public static bool ValidateContentType(UnityWebRequest request, ref ContentTypeState expectedType) {
			if(expectedType == ContentTypeState.Unknown) {
				string contentType = request.GetResponseHeader("Content-Type");
				if(!TryParseContentType(contentType, out expectedType)) {
					RestApiDebug.Result(request, expectedType);
					return false;
				}
			}
			return true;
		}

		public static T ParseResponse<T>(UnityWebRequest request, ContentTypeState expectedType) {
			switch(expectedType) {
				case ContentTypeState.Image:
				return (T)(object)DownloadHandlerTexture.GetContent(request);
				case ContentTypeState.Audio:
				return (T)(object)DownloadHandlerAudioClip.GetContent(request);
				case ContentTypeState.Asset:
				return (T)(object)DownloadHandlerAssetBundle.GetContent(request);
				case ContentTypeState.Text:
				return (T)(object)request.downloadHandler.text;
				case ContentTypeState.Binary:
				case ContentTypeState.Video:
				return (T)(object)request.downloadHandler.data;
				default:
				throw new Exception("Unsupported content type.");
			}
		}

		// ContentType 파싱 메서드
		public static bool TryParseContentType(string contentType, out ContentTypeState contentTypeId) {
			contentTypeId = ContentTypeState.Unknown;

			//contentType을 알수 없음
			if(string.IsNullOrEmpty(contentType))
				return false;

			// Content-Type의 메인 타입 파싱
			string mainType = contentType.Split(';')[0].Trim().ToLower();

			if(mainType.StartsWith("text/") || mainType.Contains("json") || mainType.Contains("xml")) {
				contentTypeId = ContentTypeState.Text;
				return true;
			} else if(mainType.StartsWith("image/")) {
				contentTypeId = ContentTypeState.Image;
				return true;
			} else if(mainType.StartsWith("video/")) {
				contentTypeId = ContentTypeState.Video;
				return true;
			} else if(mainType.StartsWith("audio/")) {
				contentTypeId = ContentTypeState.Audio;
				return true;
			} else if(mainType.StartsWith("application/octet-stream") || mainType.Contains("assetbundle")) {
				contentTypeId = ContentTypeState.Asset;
				return true;
			} else {
				contentTypeId = ContentTypeState.Binary;
				return true;
			}
		}

		//1%이상 증가하거나 1초가 지날시 
		public static IEnumerator UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> onProgress) {
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			while(!asyncOperation.isDone) {
				if(Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange) {
					onProgress?.Invoke(asyncOperation.progress);
					lastProgress = asyncOperation.progress;
					lastProgressUpdate = Time.time;
				}
				yield return null;
			}
			// 요청 완료 시 진행률을 100%로 설정
			onProgress?.Invoke(1f);
		}

		public static async UniTask UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> progress = null, CancellationToken token = default) {
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			while(!asyncOperation.isDone) {
				if(Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange) {
					progress?.Invoke(asyncOperation.progress); // 진행률 업데이트
					lastProgress = asyncOperation.progress;
					lastProgressUpdate = Time.time;
				}
				await UniTask.Yield(PlayerLoopTiming.Update, token); // 프레임 대기 (취소 토큰 지원)
			}

			progress?.Invoke(1f);
		}

		public static string GetAudioMimeType(AudioContentType contentType) {
			return contentType switch {
				AudioContentType.MP3 => "audio/mpeg",
				AudioContentType.Wav => "audio/wav",
				AudioContentType.Ogg => "audio/ogg",
				_ => throw new NotSupportedException($"Unsupported Audio Content-Type: {contentType}")
			};
		}
		public static AudioType ConvertToUnityAudioType(AudioContentType contentType) {
			return contentType switch {
				AudioContentType.MP3 => AudioType.MPEG,
				AudioContentType.Wav => AudioType.WAV,
				AudioContentType.Ogg => AudioType.OGGVORBIS,
				_ => AudioType.UNKNOWN // 지원되지 않는 타입은 UNKNOWN으로 처리
			};
		}

		public static string GetVideoMimeType(VideoContentType contentType) {
			return contentType switch {
				VideoContentType.Mp4 => "video/mp4",
				VideoContentType.Webm => "video/webm",
				_ => throw new NotSupportedException($"Unsupported Video Content-Type: {contentType}")
			};
		}

		public static string GetImageMimeType(ImageContentType contentType) {
			return contentType switch {
				ImageContentType.Png => "image/png",
				ImageContentType.Jpeg => "image/jpeg",
				_ => throw new NotSupportedException($"Unsupported Image Content-Type: {contentType}")
			};
		}

		public static IEnumerator ExecuteWithAuth(string authToken, Dictionary<string, string> headers, Func<IEnumerator> execute) {
			SetAuthHeader(ref headers, authToken);
			yield return execute();
		}

		public static async UniTask ExecuteWithAuth(string authToken, Dictionary<string, string> headers, Func<UniTask> execute) {
			SetAuthHeader(ref headers, authToken); // 인증 헤더 설정
			await execute(); // 비동기 작업 실행
		}

		public static void HandleJsonResponse<T>(ApiResponse<string> apiResponse, UnityAction<ApiResponse<T>> onComplete = null) {
			if(!apiResponse.Success) {
				onComplete?.Invoke(new FailureResponse<T>(apiResponse.ErrorMessage));
				return;
			}
			try {
				var parsedData = JsonParser.Parse<T>(apiResponse.Data);
				onComplete?.Invoke(new SuccessResponse<T>(parsedData));
			} catch(Exception ex) {
				onComplete?.Invoke(new FailureResponse<T>($"JSON 파싱 오류: {ex.Message}"));
			}
		}

		public static T HandleJsonResponse<T>(string jsonResponse) {
			try {
				var parsedData = JsonParser.Parse<T>(jsonResponse);
				return parsedData;
			} catch(Exception ex) {
				throw new Exception($"JSON 파싱 오류: {ex.Message}");
			}
		}

		public static void HandleVideoResponse(ApiResponse<byte[]> apiResponse, string savePath, UnityAction<ApiResponse<string>> onComplete = null) {
			if(!apiResponse.Success) {
				onComplete?.Invoke(new FailureResponse<string>(apiResponse.ErrorMessage));
				return;
			}

			try {
				File.WriteAllBytes(savePath, apiResponse.Data); // 파일 저장
				Debug.Log($"Video saved at: {savePath}");
				onComplete?.Invoke(new SuccessResponse<string>(savePath)); // 성공 시 저장 경로 반환
			} catch(Exception ex) {
				onComplete?.Invoke(new FailureResponse<string>($"Failed to save video: {ex.Message}"));
			}
		}

		public static string HandleVideoResponse(byte[] videoData, string savePath) {
			try {
				// 비디오 데이터를 파일로 저장
				File.WriteAllBytes(savePath, videoData);
				Debug.Log($"Video saved at: {savePath}");
				return savePath;
			} catch(Exception ex) {
				// 예외 발생 시 상위에서 처리 가능
				throw new Exception($"Failed to save video: {ex.Message}");
			}
		}



		public static IEnumerator HandleNoneSend(Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			byte[] bodyData = new byte[0]; // 빈 데이터
			yield return execute(bodyData);
		}

		public static async UniTask HandleNoneSend(Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			byte[] bodyData = new byte[0]; // 빈 데이터
			await execute(bodyData);
		}


		public static IEnumerator HandleTextSend(string text, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, contentType);
			byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(text);
			yield return execute(bodyData);
		}

		public static async UniTask HandleTextSend(string text, string contentType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, contentType);
			byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(text);
			await execute(bodyData);
		}


		public static IEnumerator HandleBinarySend(byte[] binaryData, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, contentType);
			yield return execute(binaryData);
		}

		public static async UniTask HandleBinarySend(byte[] binaryData, string contentType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, contentType);
			await execute(binaryData);
		}

		public static IEnumerator HandleJsonSend<T>(T body, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, "application/json");
			byte[] bodyData = JsonParser.ToByteArray(body);
			yield return execute(bodyData);
		}

		public static async UniTask HandleJsonSend<T>(T body, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, "application/json");
			byte[] bodyData = JsonParser.ToByteArray(body);
			await execute(bodyData);
		}

		public static IEnumerator HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], AudioType, IEnumerator> execute) {
			string contentType = GetAudioMimeType(audioType); // AudioContentType에 따라 MIME 타입 결정
			SetContentHeader(ref headers, contentType); // 헤더 설정
			AudioType unityAudioType = ConvertToUnityAudioType(audioType); // Unity에서 사용하는 AudioType으로 변환
			yield return execute(audioData, unityAudioType); // 준비된 데이터를 다음 단계로 전달
		}

		public static async UniTask HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], AudioType, UniTask> execute) {
			string contentType = GetAudioMimeType(audioType); // AudioContentType에 따라 MIME 타입 결정
			SetContentHeader(ref headers, contentType); // 헤더 설정
			AudioType unityAudioType = ConvertToUnityAudioType(audioType); // Unity에서 사용하는 AudioType으로 변환
			await execute(audioData, unityAudioType);
		}

		public static IEnumerator HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			string contentType = GetVideoMimeType(videoType);
			SetContentHeader(ref headers, contentType);
			yield return execute(videoData);
		}

		public static async UniTask HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			string contentType = GetVideoMimeType(videoType);
			SetContentHeader(ref headers, contentType);
			await execute(videoData);
		}


		public static IEnumerator HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			string contentType = GetImageMimeType(imageType);
			SetContentHeader(ref headers, contentType);
			yield return execute(imageData);
		}

		public static async UniTask HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			string contentType = GetImageMimeType(imageType);
			SetContentHeader(ref headers, contentType);
			await execute(imageData);
		}
		public static IEnumerator HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			yield return execute(bodyData);
		}

		public static async UniTask HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			await execute(bodyData);
		}

		//GetBase


	}
}