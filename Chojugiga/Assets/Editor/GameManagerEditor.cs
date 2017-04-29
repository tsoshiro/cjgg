using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]//拡張するクラスを指定
public class GameManagerEditor : Editor {

	/// <summary>
	/// InspectorのGUIを更新
	/// </summary>
	public override void OnInspectorGUI(){
		base.OnInspectorGUI ();

		GameManager gameManager = target as GameManager;

		//ボタンを表示
		if (GUILayout.Button("Reset PlayerPrefs")){
			gameManager.resetPlayerPrefs ();
		}  
	}

}  