using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class ExportAssetBundle : EditorWindow {

	[SerializeField] static bool[] expType = new bool[(int)(EAssetType.Max)];
	[SerializeField] static string[] expPath = new string[(int)(EAssetType.Max)];
	
	[MenuItem("AssetBundle/ExportAssetBundle")]
	static void Init()
	{
		EditorWindow editorWindow = GetWindow(typeof(ExportAssetBundle));
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.ShowUtility();
	}
	
	[MenuItem("AssetBundle/PC")]
	static void ExportBundle()
	{
		Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
		
//		if(IsPath(objs[0]))
//		{
//			DirectoryInfo info = null;
//			info.
//		}
	}
	
	static bool IsPath(Object obj)
	{
		if(null == obj) return false;
		string path = AssetDatabase.GetAssetPath(obj);
		if(Directory.Exists(path)) return true;
		return false;
	}
	
	static bool IsModel(Object obj)
	{
		if(!(obj is GameObject)) return false;
		GameObject go = obj as GameObject;
		
		return go.tag == EAssetType.Model.ToString();
	}
	
	static bool IsAnim(Object obj)
	{
		if(!(obj is GameObject)) return false;
		GameObject go = obj as GameObject;
		
		return go.tag == EAssetType.Anim.ToString();
	}
	
	static bool IsTex(Object obj)
	{
		return obj is Texture;
	}
	
	static bool IsMtrl(Object obj)
	{
		return obj is Material;
	}
	
	static bool IsUI(Object obj)
	{
		if(!(obj is GameObject)) return false;
		GameObject go = obj as GameObject;
		
		return go.tag == EAssetType.UI.ToString();
	}
	
	static bool IsSound(Object obj)
	{
		return obj is AudioClip;
	}
	
	static bool IsStage(Object obj)
	{
		if(!(obj is GameObject)) return false;
		GameObject go = obj as GameObject;
		
		return go.tag == EAssetType.Stage.ToString();
	}
	
	static void ExportBundle(BuildTarget target)
	{
		//BuildPipeline.BuildAssetBundle(0, 0, 0, 0, 
	}
	
	void OnGUI()
	{
		for(int i = 0; i < (int)EAssetType.Max; ++i)
		{
			EditorGUILayout.BeginHorizontal();
			
			bool toggle = EditorGUILayout.Toggle(((EAssetType)i).ToString(), expType[i]);
			if(toggle != expType[i])
			{
				expType[i] = toggle;
			}
			
			string path = EditorGUILayout.TextField(expPath[i]);
			if(path != expPath[i])
			{
				expPath[i] = path;
			}
			
			EditorGUILayout.EndHorizontal();
		}

	}
}
