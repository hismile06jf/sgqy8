﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelMgr : MonoBehaviour {

	static ModelMgr mInstance;
	static public ModelMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				mInstance = LogicRoot.Instance.GetSingletonObjectScript<ModelMgr>("AssetMgr", "ModelMgr");
			}
			return mInstance;
		}
	}
	
	LinkedList<ResCounter<GameObject>> m_listLoadingList = new LinkedList<ResCounter<GameObject>>();
	Dictionary<string, ResCounter<GameObject>> m_dictModel = new Dictionary<string, ResCounter<GameObject>>();
	
	public void LoadModel(string modelPath, ResLoadCallBack<GameObject> loadCB, ResLoadProgressCallBack progressCB)
	{
		ResCounter<GameObject> res = null;
		if(m_dictModel.ContainsKey(modelPath))
		{
			res = m_dictModel[modelPath];
			if(null != res.Res)
			{
				res.AddRef();
				
				if(null != progressCB) progressCB(modelPath, 1f);
				if(null != loadCB) loadCB(modelPath, res.Res);
				//OnAssetLoadCallBack(true, res);
				return;
			}
			else
			{
				if(null != res.Bundle)
				{
					if(null != loadCB) res.LoadCallBack += loadCB;
					if(null != progressCB) res.ProgressCallBack += progressCB;
					OnAssetLoadCallBack(true, res);
					return;
				}
			}
		}
		else
		{
			//get path
			res = new ResCounter<GameObject>();
			res.ResPath = modelPath;

			m_dictModel.Add(modelPath, res);
		}

		res.AddRef();
		m_listLoadingList.AddLast(res);
		if(null != loadCB) res.LoadCallBack += loadCB;
		if(null != progressCB) res.ProgressCallBack += progressCB;
	}
	
	public void UnLoadModel(string modelPath)
	{
		if(!m_dictModel.ContainsKey(modelPath)) return;
		ResCounter<GameObject> res = m_dictModel[modelPath];
		res.Release();
		if(res.Ref <= 0)
		{
			UnLoadRes(res);
			m_dictModel.Remove(modelPath);
		}
	}
	
	void UnLoadRes(ResCounter<GameObject> res)
	{
		//Do Not Destroy Here
		if(null != res.Res) Object.DestroyImmediate(res.Res, true);
		if(null != res.Bundle) 
		{
			res.Bundle.Unload(true);
		}
		Resources.UnloadUnusedAssets();
	}
	
	void OnAssetLoadCallBack(bool success, ResCounter<GameObject> res)
	{
		if(!m_dictModel.ContainsKey(res.ResPath))
		{
			UnLoadRes(res);
			return;
		}
		
		if(!success)
		{
			if(m_dictModel.ContainsKey(res.ResPath))
			{
				m_dictModel.Remove(res.ResPath);
			}
			UnLoadRes(res);
			return;
		}
		
		if(success)
		{
			if(null == res.Res && null != res.Bundle)
			{
				res.Res = res.Bundle.mainAsset as GameObject;
			}
			/*  调用Unload(false)的话会减少asset数量，但是很少，测试只有1，而且只能调用一次，下次调用Unload(true)时，不会卸载资源  */
			/*  assetBundle.Unload 只能调用一次，为了防止泄露，只在删除模型的时候调用  */
			//res.AssetWWW.assetBundle.Unload(false);
			if(null != res.LoadCallBack) 
			{	
				res.LoadCallBack(res.ResPath, res.Res);
				res.LoadCallBack = null;
				res.ProgressCallBack = null;
			}
			
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
			//StartCoroutine(LoadFromBundle(res));
		}
	}
	
	//LoadFromBundle saync
	IEnumerator LoadFromBundle(ResCounter<GameObject> res)
	{
		AssetBundleRequest request = res.AssetWWW.assetBundle.LoadAsync("so0009_1", typeof(GameObject));
		yield return request;
		
		res.Res = request.asset as GameObject;
		if(null != res.LoadCallBack) 
		{	
			res.LoadCallBack(res.ResPath, res.Res);
			res.LoadCallBack = null;
			res.ProgressCallBack = null;
		}
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(m_listLoadingList.Count > 0)
		{
			ResCounter<GameObject> res = m_listLoadingList.First.Value;
			if(res.AssetWWW == null)
			{
				res.AssetWWW = new WWW(res.ResPath);
			}
			
			if(res.AssetWWW.isDone)
			{
				bool success = null == res.AssetWWW.error;
				if(!success)
				{
					Debug.LogError("Res Download Failed, respath = " + res.ResPath);
				}
				
				if(null != res.AssetWWW)
				{
					res.Bundle = res.AssetWWW.assetBundle;
				}
				
				if(null != res.ProgressCallBack) res.ProgressCallBack(res.ResPath, res.AssetWWW.progress);
				
				OnAssetLoadCallBack(success, res);
				m_listLoadingList.RemoveFirst();
			}
			else
			{
				if(null != res.ProgressCallBack) res.ProgressCallBack(res.ResPath, res.AssetWWW.progress);
			}
		}
	}
}

