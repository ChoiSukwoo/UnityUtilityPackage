using UnityEngine;

namespace Suk.Utility
{
	public static class Math
	{
		public static float CalcInitStepValue(float targetValue, float step, float currentValue)
		{
			if (Mathf.Approximately(targetValue, currentValue)) return currentValue;
			float addValue = currentValue > targetValue ? step : -step;
			float startValue = targetValue + addValue;
			return startValue;
		}

		public static float SafeDivide(float num, float den) { return Mathf.Clamp01(den > 0f ? num / den : 0f); }
		public static float SafeDivide(int num, int den) { return Mathf.Clamp01(den > 0 ? (float)num / den : 0f); }
		public static float SafeDivide(int num, float den) { return Mathf.Clamp01(den > 0f ? num / den : 0f); }
		public static float SafeDivide(float num, int den) { return Mathf.Clamp01(den > 0 ? num / den : 0f); }



		//Origin 에서 Target 사이의 벡터를 계산
		public static Vector2 CalculateDirection(Vector2 originPos, Vector2 targetPos) { return (targetPos - originPos).normalized; }

		//Origin 에서 Target 사이의 각도를 계산 (우측 - `0°` 위 - `90°`)
		public static float CalculateDirectionAngle(Vector2 originPos, Vector2 targetPos)
		{
			Vector2 direction = CalculateDirection(originPos, targetPos);
			float angle = CalculateDirectionAngle(direction);
			return NormalizeAngle(angle);
		}

		//주어진 방향기준 각도를 계산 ((1,0) - `0°` (0,1) - `90°`)
		public static float CalculateDirectionAngle(Vector2 direction)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도 계산
			return NormalizeAngle(angle);
		}

		// 각도를 0~360° 범위로 변환하는 함수
		public static float NormalizeAngle(float angle) { return Mathf.Repeat(angle, 360f); }

		// 각도(float) 기준 Lerp (360도 회전 보간 적용)
		public static float LerpAngle(float origin, float target, float t)
		{
			float delta = Mathf.Repeat(target - origin + 180f, 360f) - 180f; // -180 ~ 180° 범위 유지
			float angle = origin + delta * t;
			return NormalizeAngle(angle);
		}

		// 2D 벡터(Vector2) 기준 Lerp
		public static Vector2 LerpVector2(Vector2 origin, Vector2 target, float t)
		{
			return new Vector2(
				Mathf.Lerp(origin.x, target.x, t),
				Mathf.Lerp(origin.y, target.y, t)
			);
		}

		// 벡터(Vector3) 기준 Lerp
		public static Vector3 LerpVector3(Vector3 origin, Vector3 target, float t)
		{
			return new Vector3(
				Mathf.Lerp(origin.x, target.x, t),
				Mathf.Lerp(origin.y, target.y, t),
				Mathf.Lerp(origin.z, target.z, t)
			);
		}
	}

}
