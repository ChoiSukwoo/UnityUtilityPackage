using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Suk.State
{
	public class State<T>
	{
		T _value;
		readonly UnityEvent<T> _onValueChanged = new UnityEvent<T>();

		public State(T value) { _value = value; }

		#region 외부 이벤트
		public T Value => _value;
		
		public void SetValue(T newValue, bool force = false)
		{
			if (!force && IsEqual(newValue))
				return;
			_value = newValue;
			_onValueChanged?.Invoke(_value); // 값 변경 시 리스너 호출
		}
		
		public void SetValue(Func<T, T> prev, bool force = false) {SetValue(prev(Value), force); }
		// 값 변경 시 호출될 리스너 추가
		public void AddListener(UnityAction<T> listener) { _onValueChanged.AddListener(listener); }
		// 기존 리스너를 제거하고 새로운 리스너 설정
		public void SetListener(UnityAction<T> listener)
		{
			_onValueChanged.RemoveAllListeners();
			_onValueChanged.AddListener(listener);
		}
		// 모든 리스너 제거
		public void RemoveListeners(UnityAction<T> listener) { _onValueChanged.RemoveListener(listener); }
		// 모든 리스너 제거
		public void RemoveAllListeners() { _onValueChanged.RemoveAllListeners(); }
		#endregion

		#region 내부 이벤트
		bool IsEqual(T newValue) { return EqualityComparer<T>.Default.Equals(_value, newValue); }
		int ListenerCount => _onValueChanged.GetPersistentEventCount();
		#endregion

		public override string ToString()
		{
			var stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine($"State<{typeof(T).Name}>: ");
			stringBuilder.AppendLine($"  Current Value : {Value} ");
			stringBuilder.Append($"  Listeners Count : {ListenerCount}");
			return stringBuilder.ToString();
		}
	}
}
