namespace Suk.RestApi {

	//응답용 컨텐츠 타입
	internal enum ContentTypeState {
		Unknown = 0,    // 알 수 없는 데이터
		Text = 1,       // 텍스트 데이터 (JSON, XML, HTML 등)
		Binary = 2,     // 기타 바이너리 데이터
		Video = 3,      // 비디오 데이터
		Audio = 4,      // 오디오 데이터
		Image = 5,      // 이미지 데이터
		Asset = 6       // 에셋 번들
	}


	public enum AudioContentType {
		MP3,   // audio/mpeg
		Wav,    // audio/wav
		Ogg     // audio/ogg
	}

	public enum VideoContentType {
		Mp4,    // video/mp4
		Webm    // video/webm
	}

	public enum ImageContentType {
		Png,    // image/png
		Jpeg    // image/jpeg
	}
}