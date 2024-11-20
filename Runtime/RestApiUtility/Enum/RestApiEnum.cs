namespace Suk.RestApi {

	//����� ������ Ÿ��
	internal enum ContentTypeState {
		Unknown = 0,    // �� �� ���� ������
		Text = 1,       // �ؽ�Ʈ ������ (JSON, XML, HTML ��)
		Binary = 2,     // ��Ÿ ���̳ʸ� ������
		Video = 3,      // ���� ������
		Audio = 4,      // ����� ������
		Image = 5,      // �̹��� ������
		Asset = 6       // ���� ����
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