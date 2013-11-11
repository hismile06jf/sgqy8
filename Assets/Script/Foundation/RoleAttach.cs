using UnityEngine;

public class AttachObject
{
	public AttachObject(string path, string hp)
	{
		filePath = path;
		hardPoint = hp;
	}
	
	string filePath = null;
	string hardPoint = null;
	GameObject obj;
	
	public string Path
	{
		get { return filePath; }
	}
	
	public string HardPoint
	{
		get { return hardPoint; }
		set { hardPoint = value; }
	}
	
	public GameObject Obj
	{
		get { return obj; }
		set { obj = value; }
	}
}

public partial class Role
{
	AttachObject GetAttachObject(string filePath)
	{
		for(int i = 0; i < listAttachObject.Count; ++i)
		{
			if(listAttachObject[i].Path == filePath) return listAttachObject[i];
		}
		
		return null;
	}
	
	bool IsAttach(string filePath)
	{
		AttachObject att = GetAttachObject(filePath);
		return null != att && null != att.Obj;
	}
	
	public GameObject GetHardPointObj(string szHP)
	{
		return UnityTools.FindChild(objMainBody, szHP);
	}
	
	public Transform GetHardPoint(string szHP)
	{
		GameObject objHP = GetHardPointObj(szHP);
		return null == objHP ? null : objHP.transform;
	}
	
	public void LoadMainBody(string modelPath)
	{
		if(null == objMainBody) return;
		
		ModelMgr.Instance.LoadModel(modelPath, OnMainBodyCallBack, null);
	}
	
	public void LoadDependenceTexture(string modelTexturePath)
	{
	}
	
	public void AddAttach(string modelPath, string szHP)
	{
		if(IsAttach(modelPath)) return;
		AttachObject att = new AttachObject(modelPath, szHP);
		ModelMgr.Instance.LoadModel(modelPath, OnAttachCallBack, null);
	}
	
	public void DelAttach(string modelPath)
	{
		DelAttach(GetAttachObject(modelPath));
	}
	
	public void DelAttach(AttachObject att)
	{
		if(null == att) return;
		if(null != att.Obj) GameObject.DestroyImmediate(att.Obj);
		ModelMgr.Instance.UnLoadModel(att.Path);
		listAttachObject.Remove(att);
	}
	
	public void AttachToHP(GameObject obj, string szHP)
	{
	}
	
	public void DetachFromHP(GameObject obj)
	{
	}
	
	/************************************************/
	/*    callbacks    */
	/************************************************/
	void OnMainBodyCallBack(string path, GameObject obj)
	{
		objMainBody = obj;
	}
	
	void OnAttachCallBack(string path, GameObject obj)
	{
		AttachObject att = GetAttachObject(path);
		if(null == att) ModelMgr.Instance.UnLoadModel(path);
		
		att.Obj = obj;
		if(null != obj)
		{
			obj.transform.parent = GetHardPoint(att.HardPoint);
		}
	}
}

