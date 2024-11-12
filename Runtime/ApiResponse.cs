namespace Suk {
	public abstract class ApiResponse<T> {
		public bool Success { get; protected set; }
		public string ErrorMessage { get; protected set; }
		public T Data { get; protected set; }

		public override string ToString() {
			return $"Success: {Success}, ErrorMessage: {ErrorMessage}, Data: {Data}";
		}
	}

	// 성공 응답 클래스
	public class SuccessResponse<T> : ApiResponse<T> {
		public SuccessResponse(T data) {
			Success = true;
			Data = data;
			ErrorMessage = null;
		}
	}

	// 실패 응답 클래스
	public class FailureResponse<T> : ApiResponse<T> {
		public FailureResponse(string errorMessage) {
			Success = false;
			ErrorMessage = errorMessage;
			Data = default;
		}
	}
}