using UnityEngine;
using System.Collections.Generic;

public class LogicRoot : MonoBehaviour {
	static LogicRoot mInstance;
	static public LogicRoot Instance
	{
		get {
			if(null == mInstance) 
			{
				GameObject obj = new GameObject("0LogicRoot");
				DontDestroyOnLoad(obj);
				mInstance = obj.AddComponent<LogicRoot>();
			}
			return mInstance;
		}
	}
	
	public GameObject GetParentObject(string name)
	{
		GameObject obj = UnityTools.FindChild(gameObject, name);
		if(null == obj) 
		{
			obj = new GameObject(name);
			DontDestroyOnLoad(obj);
			obj.transform.parent = gameObject.transform;
		}
		return obj;
	}
	
	public T GetSingletonObjectScript<T>(string parentName, string objName) where T : Component
	{
		GameObject objParent = GetParentObject(parentName);
		GameObject obj = new GameObject(objName);
		DontDestroyOnLoad(obj);
		obj.transform.parent = objParent.transform;
		return obj.AddComponent<T>();
	}
}
