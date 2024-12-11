using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{

	public class MultipartFormData
	{
		private readonly List<byte[]> _parts = new List<byte[]>();
		public string Boundary { get; } = "----UnityBoundary" + Guid.NewGuid();

		// �ؽ�Ʈ �ʵ� �߰�
		public void AddField(string name, string value)
		{
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n"));
		}

		// ����� ���� �߰�
		public void AddAudioFile(string name, string filename, byte[] data, AudioContentType audioContentType)
		{
			string contentType = GetContentTypeFromAudioContentType(audioContentType);
			AddFileInternal(name, filename, data, contentType);
		}

		// ���� ���� �߰�
		public void AddVideoFile(string name, string filename, byte[] data, VideoContentType videoContentType)
		{
			string contentType = GetContentTypeFromVideoContentType(videoContentType);
			AddFileInternal(name, filename, data, contentType);
		}

		// �̹��� ���� �߰�
		public void AddImageFile(string name, string filename, byte[] data, ImageContentType imageContentType)
		{
			string contentType = GetContentTypeFromImageContentType(imageContentType);
			AddFileInternal(name, filename, data, contentType);
		}

		// ����� ���� ���� �߰�
		public void AddCustomFile(string name, string filename, byte[] data, string contentType)
		{
			AddFileInternal(name, filename, data, contentType);
		}

		// ���� ���� ���� �߰� �޼���
		private void AddFileInternal(string name, string filename, byte[] data, string contentType)
		{
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}\r\nContent-Disposition: form-data; name=\"{name}\"; filename=\"{filename}\"\r\nContent-Type: {contentType}\r\n\r\n"));
			_parts.Add(data);
			_parts.Add(Encoding.UTF8.GetBytes("\r\n"));
		}

		// ���� �ٿ���� �߰� �� ������ ����ȭ
		public byte[] ToBytes()
		{
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}--\r\n"));
			return _parts.SelectMany(p => p).ToArray();
		}
	}

}