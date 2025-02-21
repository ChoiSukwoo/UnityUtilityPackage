using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Suk.UI
{
	public class UISlider : MonoBehaviour
	{
		enum Direction
		{
			Left = 0, Right = 1, Down = 2,
			Up = 3,
		}

		[Header("부모 기준으로 Content 크기 변경됨")]
		[SerializeField] RectTransform _content;
		[SerializeField] Direction _slideDirection;

		CancellationTokenSource _cts; // UniTask 취소 토큰

		void Awake() { InitContentSize(); }

		/// <summary>RectTransform을 즉시 특정 위치로 이동합니다.</summary>
		public void MoveTo(float targetValue)
		{
			CancelToken();
			float clampedValue = Mathf.Clamp01(targetValue); // 값 범위를 0~1로 한정
			Rect anchor = GetCalculatedAnchor(clampedValue, _slideDirection);
			ApplyAnchor(_content, anchor);
		}

		/// <summary> RectTransform을 특정 위치로 부드럽게 슬라이드합니다. </summary>
		public async UniTask SlideTo(float targetValue, float duration)
		{

			// 기존 UniTask 작업 취소
			CancelToken();
			CancellationToken token = CreateToken();

			//목표 Slide 값
			float clampedTargetValue = Mathf.Clamp01(targetValue); // 값 범위 제한 (0~1)
			//현재 진행도
			float currentValue = GetCurrentValue(_content, _slideDirection);
			//경과 시간 연산
			float elapsedTime = GetElapsedTime(currentValue, clampedTargetValue, duration);
			//동작의 시작 값 => 늘어남 : 0 줄어듬 : 1
			float beginValue = GetBeginValue(currentValue, clampedTargetValue);


			while(elapsedTime < duration) {
				if (token.IsCancellationRequested) return; // 취소 요청 시 함수 종료

				// 프레임 시간 누적
				elapsedTime += Time.deltaTime;

				//이동 진행
				float progress = Mathf.Lerp(beginValue, clampedTargetValue, elapsedTime / duration);
				ApplyAnchor(_content, GetCalculatedAnchor(progress, _slideDirection));

				await UniTask.Yield(PlayerLoopTiming.Update, token); // 다음 프레임 대기
			}

			ApplyAnchor(_content, GetCalculatedAnchor(clampedTargetValue, _slideDirection));
		}

		// 앵커를 이용해 현재 진행도 획득
		float GetCurrentValue(RectTransform content, Direction direction)
		{
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
		float GetElapsedTime(float currentValue, float targetValue, float duration) { return currentValue < targetValue ? currentValue * duration : (1f - currentValue) * duration; }

		//방향에 따른 시작값
		float GetBeginValue(float currentValue, float targetValue) { return currentValue < targetValue ? 0f : 1f; }

		/// <summary> targetValue와 방향에 따라 앵커를 계산</summary>
		Rect GetCalculatedAnchor(float value, Direction direction)
		{
			value = Mathf.Clamp01(value); // 0~1 제한

			Vector2 anchorMin = Vector2.zero;
			Vector2 anchorMax = Vector2.one;

			// Direction에 따른 anchorMin, anchorMax 계산
			if (direction == Direction.Left) {
				anchorMin.x = Mathf.Lerp(1f, 0f, value); // 오른쪽에서 왼쪽
				anchorMax.x = Mathf.Lerp(2f, 1f, value);
			} else if (direction == Direction.Right) {
				anchorMin.x = Mathf.Lerp(-1f, 0, value); // 왼쪽에서 오른쪽
				anchorMax.x = Mathf.Lerp(0, 1f, value);
			} else if (direction == Direction.Down) {
				anchorMin.y = Mathf.Lerp(1f, 0f, value); // 위에서 아래로
				anchorMax.y = Mathf.Lerp(2f, 1f, value);
			} else if (direction == Direction.Up) {
				anchorMin.y = Mathf.Lerp(-1f, 0f, value); // 아래에서 위로
				anchorMax.y = Mathf.Lerp(0f, 1f, value);
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
