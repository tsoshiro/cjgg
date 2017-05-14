using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class MyFont : ScriptableObject {
	public Font font;
}

public class FontReplacer : EditorWindow {
	static SerializedProperty sp;

	[MenuItem("Tools/Replace All Fonts in Scene")]
	public static void ShowWindow() {
		EditorWindow a = EditorWindow.GetWindow (typeof(FontReplacer), true, "FontReplacer");
		var obj = ScriptableObject.CreateInstance<MyFont> ();
		var serializedObject = new UnityEditor.SerializedObject (obj);

		sp = serializedObject.FindProperty ("font");

		DebugLogger.Log ("font " + sp.propertyType);
	}

	void OnGUI() {
		EditorGUILayout.PropertyField (sp);
		if (GUILayout.Button ("Replace All Fonts")) {
			DebugLogger.Log ("you are trying to replace all fonts to new one in the scene");

			var textComponents = Resources.FindObjectsOfTypeAll (typeof(Text)) as Text[];
			foreach (var component in textComponents) {
				component.font = sp.objectReferenceValue as Font;
			}
			EditorSceneManager.MarkAllScenesDirty ();
			DebugLogger.Log ("Replace complete");
		}
	}
}
