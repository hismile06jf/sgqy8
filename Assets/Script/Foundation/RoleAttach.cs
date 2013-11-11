using UnityEngine;

public class AttachObject
{
	public AttachObject(int id, string hp, bool mustUnload)
	{
		modelId = id;
		hardPoint = hp;
		this.mustUnload = mustUnload;
	}
	
	bool mustUnload = false;
	int modelId = 0;
	string hardPoint = null;
	GameObject obj;
	
	public bool MustUnload
	{
		get { return mustUnload; }
	}
	
	public int ModelId
	{
		get { return modelId; }
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
	public void LoadMainBody(int modelId)
	{
		if(null == objMainBody) return;
		
		ModelMgr.Instance.LoadModel(modelId, OnMainBodyCallBack, null, null);
	}
	
	public void LoadDependenceTexture(string modelTexturePath)
	{
	}
	
	public void AddAttach(int modelId, string szHP)
	{
		if(IsAttach(modelId, szHP)) return;
		AttachObject att = new AttachObject(modelId, szHP, true);
		ModelMgr.Instance.LoadModel(modelId, OnAttachCallBack, null, att);
	}
	
	public void DelAttach(int modelId, string szHP)
	{
		DelAttach(GetAttachObject(modelId, szHP));
	}
	
	void DelAttach(AttachObject att)
	{
		if(null == att) return;
		if(null != att.Obj) 
		{
			att.Obj.transform.parent = null;
			if(att.MustUnload)
			{
				GameObject.DestroyImmediate(att.Obj);
				ModelMgr.Instance.UnLoadModel(att.ModelId);
			}
		}
		listAttachObject.Remove(att);
	}
	
	public void DelAllAttach()
	{
		while(listAttachObject.Count > 0)
		{
			DelAttach(listAttachObject[0]);
		}
	}
	
	public void AttachToHP(GameObject obj, string szHP)
	{
		if(null == obj || IsAttach(obj, szHP)) return;
		
		AttachObject att = new AttachObject(0, szHP, false);
		att.Obj = obj;
		AttachObjectToHP(obj, szHP);
	}
	
	public void DetachFromHP(GameObject obj, string szHP)
	{
		if(null == obj) return;
		DelAttach(GetAttachObject(obj, szHP));
	}
	
	/*********************************************/
	/************     Attacth     ****************/
	/*********************************************/
	AttachObject GetAttachObject(int modelId, string hp)
	{
		if(0 == modelId || string.IsNullOrEmpty(hp)) return null;
		for(int i = 0; i < listAttachObject.Count; ++i)
		{
			AttachObject att = listAttachObject[i];
			if(att.ModelId == modelId && att.HardPoint == hp) return listAttachObject[i];
		}
		
		return null;
	}
	
	AttachObject GetAttachObject(GameObject obj, string hp)
	{
		if(null == obj || string.IsNullOrEmpty(hp)) return null;
		for(int i = 0; i < listAttachObject.Count; ++i)
		{
			AttachObject att = listAttachObject[i];
			if(att.Obj == obj && att.HardPoint == hp) return listAttachObject[i];
		}
		
		return null;
	}
	
	bool IsAttach(int modelId, string hp)
	{
		AttachObject att = GetAttachObject(modelId, hp);
		return null != att && null != att.Obj;
	}
	
	bool IsAttach(GameObject obj, string hp)
	{
		AttachObject att = GetAttachObject(obj, hp);
		return null != att;
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
	
	/************************************************/
	/*    callbacks    */
	/************************************************/
	void OnMainBodyCallBack(string path, Model model, object userParam)
	{
		objMainBody = null;
	}
	
	void OnAttachCallBack(string path, Model model, object userParam)
	{
		AttachObject att = (AttachObject)userParam;
		if(null == model || null == att) 
		{
			ModelMgr.Instance.UnLoadModel(path);
			return;
		}
		
		att.Obj = model.gameObject;
		AttachObjectToHP(att.Obj, att.HardPoint);
	}
	
	void AttachObjectToHP(GameObject obj, string szHP)
	{
		if(null != obj)
		{
			obj.transform.parent = GetHardPoint(szHP);
		}
	}
}

