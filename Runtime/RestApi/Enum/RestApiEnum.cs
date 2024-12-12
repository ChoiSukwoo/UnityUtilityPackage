namespace Suk.RestApi
{

	/// <summary> 자체 Content Type </summary>
	internal enum ContentTypeState
	{
		Unknown = -1,    // 알 수 없는 데이터
		Text = 1,       // 텍스트 데이터 (JSON, XML, HTML 등)
		Binary = 2,     // 기타 바이너리 데이터
		Video = 3,      // 비디오 데이터
		Audio = 4,      // 오디오 데이터
		Image = 5,      // 이미지 데이터
		Asset = 6       // 에셋 번들
	}

	/// <summary> 자체 Audio Type </summary>
	public enum AudioContentType
	{
		Unknown = -1, //알수 없음
		MP3 = 1,        // audio/mpeg
		Wav = 2,        // audio/wav
		Ogg = 3         // audio/ogg
	}

	/// <summary> 자체 Video Type </summary>
	public enum VideoContentType
	{
		Unknown = -1,
		Mp4 = 1,    // video/mp4
		Webm = 2    // video/webm
	}

	/// <summary> 자체 Image Type </summary>
	public enum ImageContentType
	{
		Unknown = -1,
		Png = 1,    // image/png
		Jpeg = 2   // image/jpeg
	}
}