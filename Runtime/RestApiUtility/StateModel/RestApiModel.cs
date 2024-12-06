using Suk.DictionaryExtension;
using UnityEngine;

namespace Suk.RestApi
{
	public static class RestApiModel
	{
		// AudioContentType <-> Content-Type
		public static readonly BiDictionary<AudioContentType, string> AudioContentTypeToContentType = new BiDictionary<AudioContentType, string>
		{
			{ AudioContentType.MP3, "audio/mpeg" },
			{ AudioContentType.Wav, "audio/wav" },
			{ AudioContentType.Ogg, "audio/ogg" }
		};

		// AudioContentType <-> Unity AudioType
		public static readonly BiDictionary<AudioContentType, AudioType> AudioContentTypeToAudioType = new BiDictionary<AudioContentType, AudioType>
		{
			{ AudioContentType.MP3, AudioType.MPEG },
			{ AudioContentType.Wav, AudioType.WAV },
			{ AudioContentType.Ogg, AudioType.OGGVORBIS }
		};

		// Content-Type <-> Unity AudioType
		public static readonly BiDictionary<string, AudioType> ContentTypeToAudioType = new BiDictionary<string, AudioType>
		{
			{ "audio/mpeg", AudioType.MPEG },
			{ "audio/wav", AudioType.WAV },
			{ "audio/ogg", AudioType.OGGVORBIS }
		};

		// VideoContentType <-> Content-Type
		public static readonly BiDictionary<VideoContentType, string> VideoContentTypeToContentType = new BiDictionary<VideoContentType, string>
		{
			{ VideoContentType.Mp4, "video/mp4" },
			{ VideoContentType.Webm, "video/webm" }
		};

		// ImageContentType <-> Content-Type
		public static readonly BiDictionary<ImageContentType, string> ImageContentTypeToContentType = new BiDictionary<ImageContentType, string>
		{
			{ ImageContentType.Png, "image/png" },
			{ ImageContentType.Jpeg, "image/jpeg" }
		};
	}
}
