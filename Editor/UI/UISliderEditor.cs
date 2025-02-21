using Cysharp.Threading.Tasks;
using Suk.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UISlider))] 
public class UISliderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		UISlider generator = (UISlider)target;
		
		GUILayout.Space(10);
		GUILayout.Label("부모 기준으로 Content 초기화");
		if (GUILayout.Button("Init Content Size")) 
			generator.InitContentSize();
		
		//MoveTo 버튼
		GUILayout.Space(10);
		GUILayout.Label("MoveTo TestCase");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("MoveTo(0)"))
			generator.MoveTo(0);
		if (GUILayout.Button("MoveTo(1)")) 
			generator.MoveTo(1);
		GUILayout.EndHorizontal(); 
		
		//SlideTo 버튼
		GUILayout.Space(10);
		GUILayout.Label("SlideTo TestCase");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("SlideTo(0,3f)")) 
			generator.SlideTo(0,3f).Forget();
		if (GUILayout.Button("SlideTo(1,3f)")) 
			generator.SlideTo(1,3f).Forget();
		GUILayout.EndHorizontal(); 
		
	}
}
