using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{
	public static class RestApiGateway
	{
		#region Get
		public static async UniTask<T> GetJsonWithAuth<T>(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetJson<T>(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<T> GetJson<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetJson<T>(url, onProgress, headers, cancellationToken);

		public static async UniTask<Texture2D> GetTextureWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetTexture(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<Texture2D> GetTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetTexture(url, onProgress, headers, cancellationToken);

		public static async UniTask<AudioClip> GetAudioWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetAudio(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<AudioClip> GetAudio(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetAudio(url, onProgress, headers, cancellationToken);

		public static async UniTask<string> GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetVideo(url, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<string> GetVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetVideo(url, savePath, onProgress, headers, cancellationToken);

		public static async UniTask<AssetBundle> GetAssetWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetAsset(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<AssetBundle> GetAsset(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetAsset(url, onProgress, headers, cancellationToken);

		public static async UniTask<string> GetTextWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetText(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<string> GetText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetText(url, onProgress, headers, cancellationToken);

		public static async UniTask<byte[]> GetBinaryWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetBinary(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<byte[]> GetBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await RestApiGet.GetBinary(url, onProgress, headers, cancellationToken);




		#endregion

		#region PostNone
		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForTextWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForText(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForText(url, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostNoneForJsonWithAuth<Res>(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForJson<Res>(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostNoneForJson<Res>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForJson<Res>(url, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostNoneForBinaryWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForBinary(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostNoneForBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForBinary(url, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostNoneForAudioWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForAudio(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostNoneForAudio(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForAudio(url, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForVideoWithAuth(string url, string authToken, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForVideo(url, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForVideo(url, savePath, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostNoneForImageWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForImage(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostNoneForImage(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForImage(url, onProgress, headers, cancelToken);
		#endregion

		#region PostText
		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForTextWithAuth(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForText(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForText(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForText(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostTextForJsonWithAuth<Res>(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForJson<Res>(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostTextForJson<Res>(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForJson<Res>(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostTextForBinaryWithAuth(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForBinary(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostTextForBinary(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForBinary(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostTextForAudioWithAuth(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForAudio(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostTextForAudio(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForAudio(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForVideoWithAuth(string url, string body, string savePath, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForVideo(url, body, savePath, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForVideo(string url, string body, string savePath, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForVideo(url, body, savePath, contentType, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 텍스트 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostTextForImageWithAuth(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForImage(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostTextForImage(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForImage(url, body, contentType, onProgress, headers, cancelToken);
		#endregion

		#region PostJson

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<string> PostJsonForTextWithAuth<Req>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForText(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostJsonForText<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForText(url, body, onProgress, headers, cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<Res> PostJsonForJsonWithAuth<Req, Res>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForJson<Req, Res>(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostJsonForJson<Req, Res>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForJson<Req, Res>(url, body, onProgress, headers, cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<byte[]> PostJsonForBinaryWithAuth<Req>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForBinary(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostJsonForBinary<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForBinary(url, body, onProgress, headers, cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<AudioClip> PostJsonForAudioWithAuth<Req>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForAudio(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostJsonForAudio<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForAudio(url, body, onProgress, headers, cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<string> PostJsonForVideoWithAuth<Req>(string url, Req body, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForVideo(url, body, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostJsonForVideo<Req>(string url, Req body, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForVideo(url, body, savePath, onProgress, headers, cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하며, 인증 헤더를 추가합니다.</summary>
		public static async UniTask<Texture2D> PostJsonForImageWithAuth<Req>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForImage(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostJsonForImage<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForImage(url, body, onProgress, headers, cancelToken);
		#endregion

		#region PostBinary
		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForTextWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForText(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForText(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForText(url, body, contentType, onProgress, headers, cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostBinaryForJsonWithAuth<Res>(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForJson<Res>(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostBinaryForJson<Res>(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForJson<Res>(url, body, contentType, onProgress, headers, cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostBinaryForBinaryWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForBinary(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostBinaryForBinary(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForBinary(url, body, contentType, onProgress, headers, cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostBinaryForAudioWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForAudio(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostBinaryForAudio(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForAudio(url, body, contentType, onProgress, headers, cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForVideoWithAuth(string url, byte[] body, string contentType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForVideo(url, body, contentType, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForVideo(string url, byte[] body, string contentType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForVideo(url, body, contentType, savePath, onProgress, headers, cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostBinaryForImageWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForImage(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostBinaryForImage(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostBinary.PostBinaryForImage(url, body, contentType, onProgress, headers, cancellationToken);

		#endregion

		#region PostAudio
		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForTextWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForText(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForText(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForText(url, body, audioType, onProgress, headers, cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostAudioForJsonWithAuth<Res>(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForJson<Res>(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostAudioForJson<Res>(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForJson<Res>(url, body, audioType, onProgress, headers, cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostAudioForBinaryWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForBinary(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostAudioForBinary(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForBinary(url, body, audioType, onProgress, headers, cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostAudioForAudioWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForAudio(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostAudioForAudio(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForAudio(url, body, audioType, onProgress, headers, cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForVideoWithAuth(string url, byte[] body, AudioContentType audioType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForVideo(url, body, audioType, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForVideo(string url, byte[] body, AudioContentType audioType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForVideo(url, body, audioType, savePath, onProgress, headers, cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostAudioForImageWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForImage(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostAudioForImage(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostAudio.PostAudioForImage(url, body, audioType, onProgress, headers, cancellationToken);

		#endregion

		#region PostVideo
		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForTextWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForText(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForText(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForText(url, body, videoType, onProgress, headers, cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostVideoForJsonWithAuth<Res>(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForJson<Res>(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostVideoForJson<Res>(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForJson<Res>(url, body, videoType, onProgress, headers, cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostVideoForBinaryWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForBinary(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostVideoForBinary(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForBinary(url, body, videoType, onProgress, headers, cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostVideoForAudioWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForAudio(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostVideoForAudio(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForAudio(url, body, videoType, onProgress, headers, cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForVideoWithAuth(string url, byte[] body, VideoContentType videoType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForVideo(url, body, videoType, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForVideo(string url, byte[] body, VideoContentType videoType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForVideo(url, body, videoType, savePath, onProgress, headers, cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostVideoForImageWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForImage(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostVideoForImage(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostVideo.PostVideoForImage(url, body, videoType, onProgress, headers, cancellationToken);

		#endregion

		#region PostImage

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForTextWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForText(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForText(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForText(url, body, imageType, onProgress, headers, cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostImageForJsonWithAuth<Res>(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForJson<Res>(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostImageForJson<Res>(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForJson<Res>(url, body, imageType, onProgress, headers, cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostImageForBinaryWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForBinary(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostImageForBinary(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForBinary(url, body, imageType, onProgress, headers, cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostImageForAudioWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForAudio(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostImageForAudio(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForAudio(url, body, imageType, onProgress, headers, cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForVideoWithAuth(string url, byte[] body, ImageContentType imageType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForVideo(url, body, imageType, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForVideo(string url, byte[] body, ImageContentType imageType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForVideo(url, body, imageType, savePath, onProgress, headers, cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostImageForImageWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForImage(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostImageForImage(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await PostImage.PostImageForImage(url, body, imageType, onProgress, headers, cancellationToken);

		#endregion

		#region Utility
		/// <summary> N회 재시도 가능한 고차 함수 (외부 접근용) </summary>
		public static async UniTask<T> RetryAsync<T>(Func<UniTask<T>> taskFactory, int retryCount = -1, float retryDelay = 1.0f)
			=> await RestApiUtility.RetryAsync(taskFactory, retryCount, retryDelay);
		#endregion
	}
}
