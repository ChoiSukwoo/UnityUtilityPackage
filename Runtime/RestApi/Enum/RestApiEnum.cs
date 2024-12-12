namespace Suk.RestApi
{

	/// <summary> ��ü Content Type </summary>
	internal enum ContentTypeState
	{
		Unknown = -1,    // �� �� ���� ������
		Text = 1,       // �ؽ�Ʈ ������ (JSON, XML, HTML ��)
		Binary = 2,     // ��Ÿ ���̳ʸ� ������
		Video = 3,      // ���� ������
		Audio = 4,      // ����� ������
		Image = 5,      // �̹��� ������
		Asset = 6       // ���� ����
	}

	/// <summary> ��ü Audio Type </summary>
	public enum AudioContentType
	{
		Unknown = -1, //�˼� ����
		MP3 = 1,        // audio/mpeg
		Wav = 2,        // audio/wav
		Ogg = 3         // audio/ogg
	}

	/// <summary> ��ü Video Type </summary>
	public enum VideoContentType
	{
		Unknown = -1,
		Mp4 = 1,    // video/mp4
		Webm = 2    // video/webm
	}

	/// <summary> ��ü Image Type </summary>
	public enum ImageContentType
	{
		Unknown = -1,
		Png = 1,    // image/png
		Jpeg = 2   // image/jpeg
	}
}