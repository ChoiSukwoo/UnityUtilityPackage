using UnityEngine;

namespace Suk.RestApi
{
	public class RestApiManager : MonoBehaviour
	{
		// RestApiState의 값을 설정하기 위한 변수들
		[Header("디버그 로그 출력 여부를 설정합니다.")]
		[SerializeField] private bool enableDebugLog = true;

		[Header("진행 디버그 출력을 위한 최소 주기를 설정합니다.")]
		[SerializeField] private float minUpdateInterval = 1f;

		[Header("진행 디버그 출력을 위한 최소 진행률 변화를 설정합니다.")]
		[SerializeField] private float minProgressChange = 0.01f;

		[Header("자동 재시도 횟수를 설정합니다.")]
		[SerializeField] private int retryCount = 3;

		// Awake 메서드에서 RestApiState의 값을 설정합니다.
		void Awake()
		{
			RestApiState.enableDebugLog = enableDebugLog;
			RestApiState.minUpdateInterval = minUpdateInterval;
			RestApiState.minProgressChange = minProgressChange;
			RestApiState.retryCount = retryCount;
		}
	}
}