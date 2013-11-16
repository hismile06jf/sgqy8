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
	
	/// <summary>
	/// Finds the child.
	/// </summary>
	static public GameObject FindChildRecursion(GameObject objParent, string name)
	{
		if (objParent == null || string.IsNullOrEmpty(name)) return null;
		Transform trans = FindChildRecursion(objParent.transform, name);
		return null == trans ? null : trans.gameObject;
	}
	
	/// <summary>
	/// Finds the child.
	/// </summary>
	static public Transform FindChildRecursion(Transform parent, string name)
	{
		if (parent == null || string.IsNullOrEmpty(name)) return null;
		if(parent.name == name) return parent;
		
		for(int i = 0; i < parent.childCount; ++i)
		{			
			Transform tran = FindChildRecursion(parent.GetChild(i), name);
			if(null != tran) return tran;
		}
		
		return null;
	}
}

