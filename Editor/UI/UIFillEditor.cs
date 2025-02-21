using Cysharp.Threading.Tasks;
using Suk.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIFill))] 
public class UIFillEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		UIFill generator = (UIFill)target;
		
		GUILayout.Space(10);
		GUILayout.Label("부모 기준으로 Content 초기화");
		if (GUILayout.Button("Init Content Size")) 
			generator.InitContentSize();
		
		//MoveTo 버튼
		GUILayout.Space(10);
		GUILayout.Label("SetFill TestCase");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("SetFill(0)"))
			generator.SetFill(0);
		if (GUILayout.Button("SetFill(1)")) 
			generator.SetFill(1);
		GUILayout.EndHorizontal(); 
		
		//SlideTo 버튼
		GUILayout.Space(10);
		GUILayout.Label("FillTo TestCase");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("FillTo(0,3f)")) 
			generator.FillTo(0,3f).Forget();
		if (GUILayout.Button("FillTo(1,3f)")) 
			generator.FillTo(1,3f).Forget();
		GUILayout.EndHorizontal(); 
		
	}
}
