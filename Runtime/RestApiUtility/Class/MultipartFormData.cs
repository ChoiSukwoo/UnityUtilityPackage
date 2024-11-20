using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {

	public class MultipartFormData {
		private readonly List<byte[]> _parts = new List<byte[]>();
		public string Boundary { get; } = "----UnityBoundary" + Guid.NewGuid();

		// 텍스트 필드 추가
		public void AddField(string name, string value) {
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n"));
		}

		// 오디오 파일 추가
		public void AddAudioFile(string name, string filename, byte[] data, AudioContentType contentType) {
			string mimeType = GetAudioMimeType(contentType);
			AddFileInternal(name, filename, data, mimeType);
		}

		// 비디오 파일 추가
		public void AddVideoFile(string name, string filename, byte[] data, VideoContentType contentType) {
			string mimeType = GetVideoMimeType(contentType);
			AddFileInternal(name, filename, data, mimeType);
		}

		// 이미지 파일 추가
		public void AddImageFile(string name, string filename, byte[] data, ImageContentType contentType) {
			string mimeType = GetImageMimeType(contentType);
			AddFileInternal(name, filename, data, mimeType);
		}

		// 사용자 정의 파일 추가
		public void AddCustomFile(string name, string filename, byte[] data, string mimeType) {
			AddFileInternal(name, filename, data, mimeType);
		}

		// 내부 공통 파일 추가 메서드
		private void AddFileInternal(string name, string filename, byte[] data, string contentType) {
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}\r\nContent-Disposition: form-data; name=\"{name}\"; filename=\"{filename}\"\r\nContent-Type: {contentType}\r\n\r\n"));
			_parts.Add(data);
			_parts.Add(Encoding.UTF8.GetBytes("\r\n"));
		}

		// 최종 바운더리 추가 및 데이터 직렬화
		public byte[] ToBytes() {
			_parts.Add(Encoding.UTF8.GetBytes($"--{Boundary}--\r\n"));
			return _parts.SelectMany(p => p).ToArray();
		}
	}

}