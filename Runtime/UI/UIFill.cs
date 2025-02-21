using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Suk.UI
{
	public class UIFill : MonoBehaviour
	{
		enum Direction
		{
			Left = 0, Right = 1, Down = 2,
			Up = 3,
		}

		[Header("부모 기준으로 Content 크기 변경됨")]
		[SerializeField] RectTransform _content;
		[SerializeField] Direction _fillDirection;

		CancellationTokenSource _cts; // UniTask 취소 토큰

		void Awake() { InitContentSize(); }

		/// <summary>Fill을 즉시 특정 값으로 변경합니다.</summary>
		public void SetFill(float targetValue)
		{
			CancelToken();
			float clampedValue = Mathf.Clamp01(targetValue); // 값 범위를 0~1로 한정
			Rect anchor = GetCalculatedAnchor(clampedValue, _fillDirection);
			ApplyAnchor(_content, anchor);
		}

		/// <summary>Fill을 부드럽게 채웁니다.</summary>
		public async UniTask FillTo(float targetValue, float duration)
		{

			// 기존 UniTask 작업 취소
			CancelToken();
			CancellationToken token = CreateToken();

			//목표 Fill 값
			float clampedTargetValue = Mathf.Clamp01(targetValue); // 값 범위 제한 (0~1)
			// 현재 진행도
			float currentValue = GetCurrentValue(_content, _fillDirection);
			//경과 시간 연산
			float elapsedTime = GetElapsedTime(currentValue, clampedTargetValue, duration);
			//동작의 시작 값 => 늘어남 : 0 줄어듬 : 1
			float beginValue = GetBeginValue(currentValue, clampedTargetValue);
			
			while(elapsedTime < duration) {
				if (token.IsCancellationRequested) return; // 취소 요청 시 함수 종료

				// 프레임 시간 누적
				elapsedTime += Time.deltaTime;

				// 이동 진행
				float progress = Mathf.Lerp(beginValue, clampedTargetValue, elapsedTime / duration);
				ApplyAnchor(_content, GetCalculatedAnchor(progress, _fillDirection));

				await UniTask.Yield(PlayerLoopTiming.Update, token); // 다음 프레임 대기
			}

			ApplyAnchor(_content, GetCalculatedAnchor(clampedTargetValue, _fillDirection));
		}

		// 현재 채워짐 정도 획득
		float GetCurrentValue(RectTransform content, Direction direction)
		{
			// 방향에 따라 currentAnchor를 기반으로 값 계산
			switch (direction) {
			case Direction.Left:
				return Mathf.InverseLerp(1f, 0f, content.anchorMin.x);
			case Direction.Right:
				return Mathf.InverseLerp(0f, 1f, content.anchorMax.x);
			case Direction.Down:
				return Mathf.InverseLerp(1f, 0f, content.anchorMin.y);
			case Direction.Up:
				return Mathf.InverseLerp(0f, 1f, content.anchorMax.y);
			default:
				Debug.LogError("알 수 없는 방향(Direction)입니다!");
				return 0f; // 기본값 반환
			}
		}

		//경과 시간 계산
		float GetElapsedTime(float currentValue, float targetValue, float duration)
		{
			return currentValue < targetValue ? currentValue * duration : (1f - currentValue) * duration;
		}

		//방향에 따른 시작값
		float GetBeginValue(float currentValue, float targetValue)
		{
			return currentValue < targetValue ? 0f : 1f;
		}
		

		/// <summary> targetValue와 방향에 따라 앵커를 계산</summary>
		Rect GetCalculatedAnchor(float value, Direction direction)
		{
			value = Mathf.Clamp01(value); // 0~1 제한

			Vector2 anchorMin = Vector2.zero;
			Vector2 anchorMax = Vector2.one;

			// Direction에 따른 anchorMin, anchorMax 계산
			switch (direction) {
			case Direction.Left:
				anchorMin.x = Mathf.Lerp(1f, 0f, value); // 오른쪽에서 왼쪽
				anchorMax.x = 1f;                        // 오른쪽에서 왼쪽 유지
				break;
			case Direction.Right:
				anchorMin.x = 0f; // 오른쪽에서 왼쪽
				anchorMax.x = Mathf.Lerp(0f, 1f, value);
				break;
			case Direction.Down:
				anchorMin.y = Mathf.Lerp(1f, 0f, value);
				anchorMax.y = 1f;
				break;
			case Direction.Up:
				anchorMin.y = 0f;
				anchorMax.y = Mathf.Lerp(0f, 1f, value);
				break;
			default:
				Debug.LogError("알 수 없는 방향(Direction)입니다!");
				break;
			}

			return new Rect(anchorMin, anchorMax - anchorMin); // anchorMin과 max 차이를 포함한 Rect 반환
		}


		/// <summary> RectTransform에 계산된 앵커를 적용 </summary>
		void ApplyAnchor(RectTransform rectTransform, Rect calculatedAnchor)
		{
			if (!rectTransform) {
				Debug.LogError("RectTransform이 null입니다.");
				return;
			}

			// Rect에서 anchorMin과 anchorMax를 분리하여 적용
			rectTransform.anchorMin = calculatedAnchor.min; // anchorMin 설정
			rectTransform.anchorMax = calculatedAnchor.max; // anchorMax 설정
		}

		CancellationToken CreateToken()
		{
			_cts = new CancellationTokenSource();
			return _cts.Token;
		}

		void CancelToken()
		{
			if (_cts == null)
				return;
			_cts.Cancel();
			_cts.Dispose();
			_cts = null;
		}

		public void InitContentSize()
		{
			if (!_content)
				return;

			_content.sizeDelta = Vector2.zero;
			_content.anchorMin = new Vector2(0, 0);
			_content.anchorMax = new Vector2(1, 1);
			_content.anchoredPosition = Vector2.zero;
		}
	}
}
