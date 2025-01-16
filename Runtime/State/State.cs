using System.Collections.Generic;
using UnityEngine.Events;

namespace Suk
{
	public class State<T>
	{
		private T _value;
		UnityEvent<T> _onValueChanged = new UnityEvent<T>();

		public State(T value)
		{
			_value = value;
		}

		// 상태가 변경될때 실행될 이벤트 추가
		public void AddListener(UnityAction<T> listener)
		{
			_onValueChanged.AddListener(listener);
		}

		//리스너를 하나로 설정
		public void SetListener(UnityAction<T> listener)
		{
			_onValueChanged.RemoveAllListeners();
			_onValueChanged.AddListener(listener);
		}

		//리스너를 하나로 설정
		public void ClearListener()
		{
			_onValueChanged.RemoveAllListeners();
		}

		// 상태 값을 갖는 프로퍼티
		public T Value
		{
			get => _value;
			set => SetValue(value);
		}

		public void SetValue(T value, bool force = false)
		{
			if (!force && EqualityComparer<T>.Default.Equals(_value, value))
				return;

			_value = value;
			_onValueChanged?.Invoke(value); // 이벤트 호출
		}

		int ListenerCount => _onValueChanged.GetPersistentEventCount();

		public override string ToString() => $"State<{typeof(T).Name}>: Current Value = {_value}, Listeners = {ListenerCount}";
	}
}
