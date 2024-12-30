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
		public static async UniTask<string> GetTextWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetText(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<string> GetText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetText(url, onProgress, headers, cancelToken);

		public static async UniTask<T> GetJsonWithAuth<T>(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetJson<T>(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<T> GetJson<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetJson<T>(url, onProgress, headers, cancelToken);

		public static async UniTask<Texture2D> GetTextureWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetTexture(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<Texture2D> GetTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetTexture(url, onProgress, headers, cancelToken);

		public static async UniTask<AudioClip> GetAudioWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetAudio(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<AudioClip> GetAudio(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetAudio(url, onProgress, headers, cancelToken);

		public static async UniTask<string> GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await GetVideo(url, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<string> GetVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetVideo(url, savePath, onProgress, headers, cancelToken);

		public static async UniTask<AssetBundle> GetAssetWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetAsset(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<AssetBundle> GetAsset(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetAsset(url, onProgress, headers, cancelToken);

		public static async UniTask<byte[]> GetBinaryWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetBinary(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		public static async UniTask<byte[]> GetBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await RestApiGet.GetBinary(url, onProgress, headers, cancelToken);
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
		public static async UniTask<string> PostNoneForVideoWithAuth(string url, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForVideo(url, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForVideo(url, savePath, onProgress, headers, cancelToken);

		/// <summary>로그인 토큰과 함께 빈 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostNoneForTextureWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForTexture(url, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>빈 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostNoneForTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostNone.PostNoneForTexture(url, onProgress, headers, cancelToken);
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
		public static async UniTask<Texture2D> PostTextForTextureWithAuth(string url, string body, string authToken, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForTexture(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostTextForTexture(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostText.PostTextForTexture(url, body, contentType, onProgress, headers, cancelToken);
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
		public static async UniTask<Texture2D> PostJsonForTextureWithAuth<Req>(string url, Req body, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForTexture(url, body, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostJsonForTexture<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostJson.PostJsonForTexture(url, body, onProgress, headers, cancelToken);
		#endregion

		#region PostBinary
		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForTextWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForText(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForText(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForText(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostBinaryForJsonWithAuth<Res>(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForJson<Res>(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostBinaryForJson<Res>(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForJson<Res>(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostBinaryForBinaryWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForBinary(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostBinaryForBinary(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForBinary(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostBinaryForAudioWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForAudio(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostBinaryForAudio(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForAudio(url, body, contentType, onProgress, headers, cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForVideoWithAuth(string url, byte[] body, string contentType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForVideo(url, body, contentType, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostBinaryForVideo(string url, byte[] body, string contentType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForVideo(url, body, contentType, savePath, onProgress, headers, cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostBinaryForTextureWithAuth(string url, byte[] body, string contentType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForTexture(url, body, contentType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostBinaryForTexture(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostBinary.PostBinaryForTexture(url, body, contentType, onProgress, headers, cancelToken);

		#endregion

		#region PostAudio
		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForTextWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForText(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForText(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForText(url, body, audioType, onProgress, headers, cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostAudioForJsonWithAuth<Res>(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForJson<Res>(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostAudioForJson<Res>(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForJson<Res>(url, body, audioType, onProgress, headers, cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostAudioForBinaryWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForBinary(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostAudioForBinary(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForBinary(url, body, audioType, onProgress, headers, cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostAudioForAudioWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForAudio(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostAudioForAudio(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForAudio(url, body, audioType, onProgress, headers, cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForVideoWithAuth(string url, byte[] body, AudioContentType audioType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForVideo(url, body, audioType, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForVideo(string url, byte[] body, AudioContentType audioType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForVideo(url, body, audioType, savePath, onProgress, headers, cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostAudioForTextureWithAuth(string url, byte[] body, AudioContentType audioType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForTexture(url, body, audioType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Audio 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostAudioForTexture(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostAudio.PostAudioForTexture(url, body, audioType, onProgress, headers, cancelToken);

		#endregion

		#region PostVideo
		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForTextWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForText(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForText(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForText(url, body, videoType, onProgress, headers, cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostVideoForJsonWithAuth<Res>(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForJson<Res>(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostVideoForJson<Res>(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForJson<Res>(url, body, videoType, onProgress, headers, cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostVideoForBinaryWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForBinary(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostVideoForBinary(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForBinary(url, body, videoType, onProgress, headers, cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostVideoForAudioWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForAudio(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostVideoForAudio(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForAudio(url, body, videoType, onProgress, headers, cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForVideoWithAuth(string url, byte[] body, VideoContentType videoType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForVideo(url, body, videoType, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostVideoForVideo(string url, byte[] body, VideoContentType videoType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForVideo(url, body, videoType, savePath, onProgress, headers, cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostVideoForTextureWithAuth(string url, byte[] body, VideoContentType videoType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForTexture(url, body, videoType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Video 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostVideoForTexture(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostVideo.PostVideoForTexture(url, body, videoType, onProgress, headers, cancelToken);

		#endregion

		#region PostImage

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForTextWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForText(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForText(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForText(url, body, imageType, onProgress, headers, cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostImageForJsonWithAuth<Res>(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForJson<Res>(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostImageForJson<Res>(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForJson<Res>(url, body, imageType, onProgress, headers, cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostImageForBinaryWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForBinary(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostImageForBinary(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForBinary(url, body, imageType, onProgress, headers, cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostImageForAudioWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForAudio(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostImageForAudio(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForAudio(url, body, imageType, onProgress, headers, cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForVideoWithAuth(string url, byte[] body, ImageContentType imageType, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForVideo(url, body, imageType, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForVideo(string url, byte[] body, ImageContentType imageType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForVideo(url, body, imageType, savePath, onProgress, headers, cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostImageForTextureWithAuth(string url, byte[] body, ImageContentType imageType, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForTexture(url, body, imageType, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Image 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostImageForTexture(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
			=> await PostImage.PostImageForTexture(url, body, imageType, onProgress, headers, cancelToken);
		#endregion

		#region PostMultiform
		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultiformForTextWithAuth(string url, MultipartFormData formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForText(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultiformForText(string url, MultipartFormData formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForText(url, formData, onProgress, headers, cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostMultiformForJsonWithAuth<Res>(string url, MultipartFormData formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForJson<Res>(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostMultiformForJson<Res>(string url, MultipartFormData formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForJson<Res>(url, formData, onProgress, headers, cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostMultiformForBinaryWithAuth(string url, MultipartFormData formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForBinary(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostMultiformForBinary(string url, MultipartFormData formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForBinary(url, formData, onProgress, headers, cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostMultiformForAudioWithAuth(string url, MultipartFormData formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForAudio(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostMultiformForAudio(string url, MultipartFormData formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForAudio(url, formData, onProgress, headers, cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultiformForVideoWithAuth(string url, MultipartFormData formData, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForVideo(url, formData, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultiformForVideo(string url, MultipartFormData formData, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForVideo(url, formData, savePath, onProgress, headers, cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostMultiformForTextureWithAuth(string url, MultipartFormData formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForTexture(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>Multipart 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostMultiformForTexture(string url, MultipartFormData formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostMultiform.PostMultiformForTexture(url, formData, onProgress, headers, cancelToken);
		#endregion

		#region PostUrlEncoded
		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostUrlEncodedForTextWithAuth(string url, Dictionary<string, string> formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForText(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostUrlEncodedForText(string url, Dictionary<string, string> formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForText(url, formData, onProgress, headers, cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostUrlEncodedForJsonWithAuth<Res>(string url, Dictionary<string, string> formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForJson<Res>(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostUrlEncodedForJson<Res>(string url, Dictionary<string, string> formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForJson<Res>(url, formData, onProgress, headers, cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostUrlEncodedForBinaryWithAuth(string url, Dictionary<string, string> formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForBinary(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostUrlEncodedForBinary(string url, Dictionary<string, string> formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForBinary(url, formData, onProgress, headers, cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostUrlEncodedForAudioWithAuth(string url, Dictionary<string, string> formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForAudio(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostUrlEncodedForAudio(string url, Dictionary<string, string> formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForAudio(url, formData, onProgress, headers, cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostUrlEncodedForVideoWithAuth(string url, Dictionary<string, string> formData, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForVideo(url, formData, savePath, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostUrlEncodedForVideo(string url, Dictionary<string, string> formData, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForVideo(url, formData, savePath, onProgress, headers, cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 인증 헤더를 추가하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostUrlEncodedForTextureWithAuth(string url, Dictionary<string, string> formData, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForTexture(url, formData, onProgress, SetAuthHeader(headers, authToken), cancelToken);

		/// <summary>UrlEncoded 데이터를 POST 요청으로 전송하며 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostUrlEncodedForTexture(string url, Dictionary<string, string> formData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		=> await PostUrlEncoded.PostUrlEncodedForTexture(url, formData, onProgress, headers, cancelToken);
		#endregion

		#region Utility
		/// <summary> N회 재시도 가능한 고차 함수 (외부 접근용) </summary>
		public static async UniTask<T> RetryAsync<T>(Func<UniTask<T>> taskFactory, int retryCount = -1, float retryDelay = 1.0f)
			=> await RestApiUtility.RetryAsync(taskFactory, retryCount, retryDelay);

		/// <summary> 파일 경로에서 이미지 타입을 반환합니다. </summary>
		public static ImageContentType GetImageContentType(string filePath)
			=> RestApiUtility.GetImageContentType(filePath);

		/// <summary> 파일 경로에서 오디오 타입을 반환합니다. </summary>
		public static AudioContentType GetAudioContentType(string filePath)
			=> RestApiUtility.GetAudioContentType(filePath);

		/// <summary> 파일 경로에서 비디오 타입을 반환합니다. </summary>
		public static VideoContentType GetVideoContentType(string filePath)
			=> RestApiUtility.GetVideoContentType(filePath);
		#endregion
	}
}
