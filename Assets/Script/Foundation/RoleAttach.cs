using UnityEngine;
using System.Collections.Generic;

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
	int modelId;
	
	List<AttachObject> listAttachObject = new List<AttachObject>();
	List<AttachObject> listReadyAttachObject = new List<AttachObject>();
	
	public Role(int roleModelId)
	{
		modelId = roleModelId;
		LoadMainBody(roleModelId);
	}
	
	bool IsMainBodyReady
	{
		get { return null != MainBody; }
	}
	
	public void LoadMainBody(int modelId)
	{
		if(IsMainBodyReady) return;
		
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
	
	/************************************************/
	/*    callbacks    */
	/************************************************/
	virtual public void OnMainBodyReady()
	{
		if(0 != rideModelId)
		{
			MountRide(rideModelId);
		}
	}
	
	void OnMainBodyCallBack(string path, Model model, object userParam)
	{
		if(null == model)
		{
			Debug.LogError("Role Load Failed, Model[" + modelId.ToString() + "] Not Found.");
		}
		
		mainBody = model;
//		if(null != model)
//		{
//			model.InitModel();
//		}
		
		if(IsMainBodyReady)
		{
			MainBodyObj.transform.localPosition = Vector3.zero;
			MainBodyObj.transform.localRotation = Quaternion.identity;
			MainBodyObj.transform.localScale	= Vector3.one;
				
			//process ready
			ProcessReadyAttach();
			ProcessReadyAnim();
			
			OnMainBodyReady();
		}
	}
	
	void ProcessReadyAttach()
	{
		for(int i = 0; i < listReadyAttachObject.Count; ++i)
		{
			AttachObject att = listReadyAttachObject[i];
			AttachObjectToHP(att.Obj, att.HardPoint);
		}
		listReadyAttachObject.Clear();
	}
	
	void OnAttachCallBack(string path, Model model, object userParam)
	{
		AttachObject att = (AttachObject)userParam;
		if(null == model || null == att) 
		{
			ModelMgr.Instance.UnLoadModel(path);
			return;
		}
		
		if(!IsMainBodyReady)
		{
			listReadyAttachObject.Add(att);
			return;
		}
		
		att.Obj = model.gameObject;
		AttachObjectToHP(att.Obj, att.HardPoint);
	}
	
	void AttachObjectToHP(GameObject obj, string szHP)
	{
		if(null != obj)
		{
			Vector3 vPos = Vector3.zero;
			Quaternion qRot = Quaternion.identity;
			Vector3 vScl = Vector3.one;
			
			Transform trans = GetHardPoint(MainBodyObj, szHP);
			if(null != trans)
			{
				obj.transform.parent = trans;
			}
			
			Transform hpInit = UnityTools.FindChildRecursion(obj.transform, szHP);
			if(null != hpInit)
			{
				vPos = hpInit.localPosition;
				vPos.y = - vPos.y;
				qRot = hpInit.localRotation;
				vScl = hpInit.localScale;
			}
			
			obj.transform.localPosition = vPos;
			obj.transform.localRotation = qRot;
			obj.transform.localScale = vScl;
		}
	}
	
//	void AttachObjectToHP(GameObject obj, string szHP, bool useTargetHPInfo)
//	{
//		if(null != obj)
//		{
//			Vector3 vPos = Vector3.zero;
//			Quaternion qRot = Quaternion.identity;
//			Vector3 vScl = Vector3.one;
//			
//			Transform trans = GetHardPoint(MainBodyObj, szHP);
//			if(null != trans)
//			{
//				obj.transform.parent = trans;
//			}
//			
//			Transform hpInit = UnityTools.FindChildRecursion(obj.transform, szHP);
//			if(null != hpInit)
//			{
//				vPos = hpInit.localPosition;
//				vPos.y = - vPos.y;
//				qRot = hpInit.localRotation;
//				vScl = hpInit.localScale;
//			}
//			
//			obj.transform.localPosition = vPos;
//			obj.transform.localRotation = qRot;
//			obj.transform.localScale = vScl;
//		}
//	}
	
	
	static string[] szHPArray = null;
	static public string GetHardPointName(EHardPoint eHP)
	{
		if(null == szHPArray)
		{
			szHPArray =new string[(int)EHardPoint.Max];
			szHPArray[(int)EHardPoint.Horse] = "HP_horse";
			szHPArray[(int)EHardPoint.Horse01] = "HP_horse01";
			szHPArray[(int)EHardPoint.Head] = "HP_head";
			szHPArray[(int)EHardPoint.Back] = "HP_back";
			szHPArray[(int)EHardPoint.BackLeft] = "HP_back_left";
			szHPArray[(int)EHardPoint.LeftHand] = "HP_left_hand";
			szHPArray[(int)EHardPoint.RightHand] = "HP_right_hand";
			szHPArray[(int)EHardPoint.RightHandA] = "HP_right_hand_a";
			szHPArray[(int)EHardPoint.Waist] = "HP_waist";
			szHPArray[(int)EHardPoint.Effect] = "HP_effect";
			szHPArray[(int)EHardPoint.BLUp] = "HP_BLUp";
			szHPArray[(int)EHardPoint.BLDown] = "HP_BLDown";
		}
		if(eHP == EHardPoint.Max) return szHPArray[(int)EHardPoint.LeftHand];
		return szHPArray[(int)eHP];
	}
	
	static public GameObject GetHardPointObj(GameObject obj, string szHP)
	{
		return UnityTools.FindChildRecursion(obj, szHP);
	}
	
	static public Transform GetHardPoint(GameObject obj, string szHP)
	{
		GameObject objHP = GetHardPointObj(obj, szHP);
		return null == objHP ? null : objHP.transform;
	}
}

