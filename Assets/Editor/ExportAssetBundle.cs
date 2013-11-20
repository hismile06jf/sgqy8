using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class ExportAssetBundle : EditorWindow {
	
	static string buildDestRootPath;
	static BuildTarget target = BuildTarget.StandaloneWindows;
	
	
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
		target = BuildTarget.StandaloneWindows;
		string dataPath = Application.dataPath;
		int pos = dataPath.LastIndexOf("/");
		dataPath = dataPath.Substring(0, pos);
		buildDestRootPath = dataPath + "/AssetBundle";
			
		Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
		ExportBundle(objs);
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
	
	static void ExportBundle(Object[] objs)
	{
		if(null == objs) return;
		
		
		string subPath = "";
		for(int i = 0; i < objs.Length; ++i)
		{
			Object obj = objs[i];
			subPath = AssetDatabase.GetAssetPath(obj);
			
			if(IsModel(obj))
			{
				subPath = subPath.Replace(".fbx", "");
			}
			else if(IsAnim(obj))
			{
				subPath = subPath.Replace(".animation.fbx", "");
			}
			else if(IsSound(obj))
			{
				subPath = subPath.Replace(".wav", "");
			}
			else if(IsStage(obj))
			{
				subPath = subPath.Replace(".prefab", "");
			}
			else if(IsUI(obj))
			{
				subPath = subPath.Replace(".prefab", "");
			}
			else if(IsPath(obj))
			{
				//
				string objPath= "";
				ExportBundleInPath(objPath);
				continue;
			}
			else
			{
				continue;
			}
			
			subPath = subPath.Replace("Assets/ResData/", "");
			string fullPathName = buildDestRootPath + "/" + subPath + ".unity3d";
			string fullPath = fullPathName.Substring(0, fullPathName.LastIndexOf("/"));
			DirectoryInfo dir = new DirectoryInfo(fullPath);
			if(!dir.Exists)
			{
				dir.Create();
			}
			BuildPipeline.BuildAssetBundle(obj, null, fullPathName, BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.CollectDependencies, target);
		}
	}
	
	static void ExportBundleInPath(string path)
	{
		//
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
