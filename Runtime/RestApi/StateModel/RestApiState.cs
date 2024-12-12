namespace Suk.RestApi
{
	internal static class RestApiState
	{
		//디버그 출력 여부
		public static bool enableDebugLog = true;
		//진행 디버그 출력을위한 최소 주기
		public static float minUpdateInterval = 1f;
		//진행 디버그 출력을위한 최소 진행률
		public static float minProgressChange = 0.01f;
		//자동 재시도 횟수
		public static int retryCount = 3;
	}
}
