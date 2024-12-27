using UnityEngine;

namespace Suk.RestApi
{
	public class RestApiManager : MonoBehaviour
	{
		// RestApiState�� ���� �����ϱ� ���� ������
		[Header("����� �α� ��� ���θ� �����մϴ�.")]
		[SerializeField] private bool enableDebugLog = true;

		[Header("���� ����� ����� ���� �ּ� �ֱ⸦ �����մϴ�.")]
		[SerializeField] private float minUpdateInterval = 1f;

		[Header("���� ����� ����� ���� �ּ� ����� ��ȭ�� �����մϴ�.")]
		[SerializeField] private float minProgressChange = 0.01f;

		[Header("�ڵ� ��õ� Ƚ���� �����մϴ�.")]
		[SerializeField] private int retryCount = 3;

		// Awake �޼��忡�� RestApiState�� ���� �����մϴ�.
		void Awake()
		{
			RestApiState.enableDebugLog = enableDebugLog;
			RestApiState.minUpdateInterval = minUpdateInterval;
			RestApiState.minProgressChange = minProgressChange;
			RestApiState.retryCount = retryCount;
		}
	}
}