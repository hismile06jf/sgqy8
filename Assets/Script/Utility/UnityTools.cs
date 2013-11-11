using System;
using UnityEngine;

public class UnityTools
{
	/// <summary>
	/// Finds the child.
	/// </summary>
	static public GameObject FindChild(GameObject objParent, string path)
	{
		if (objParent == null || string.IsNullOrEmpty(path)) return null;
		return FindChild(objParent.transform, path);
	}
	
	
	/// <summary>
	/// Finds the child.
	/// </summary>
	static public GameObject FindChild(Transform parent, string path)
	{
		if (parent == null || string.IsNullOrEmpty(path)) return null;
		Transform trans = parent.FindChild(path);
		if(null != trans) return trans.gameObject;
		return null;
	}
	
	
	/// <summary>
	/// Finds the child component.
	/// </summary>
	static public T FindChildComponent<T> (GameObject parent, string childName) where T : Component
	{
		GameObject child = FindChild(parent, childName);
		return null == child ? null : (T)child.GetComponent<T>();
	}
}

