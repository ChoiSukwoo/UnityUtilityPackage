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

		// ���°� ����ɶ� ����� �̺�Ʈ �߰�
		public void AddListener(UnityAction<T> listener)
		{
			_onValueChanged.AddListener(listener);
		}

		//�����ʸ� �ϳ��� ����
		public void SetListener(UnityAction<T> listener)
		{
			_onValueChanged.RemoveAllListeners();
			_onValueChanged.AddListener(listener);
		}

		//�����ʸ� �ϳ��� ����
		public void ClearListener()
		{
			_onValueChanged.RemoveAllListeners();
		}

		// ���� ���� ���� ������Ƽ
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
			_onValueChanged?.Invoke(value); // �̺�Ʈ ȣ��
		}

		int ListenerCount => _onValueChanged.GetPersistentEventCount();

		public override string ToString() => $"State<{typeof(T).Name}>: Current Value = {_value}, Listeners = {ListenerCount}";
	}
}
