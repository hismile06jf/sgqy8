using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureMgr : MonoBehaviour {

	static TextureMgr mInstance;
	static public TextureMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				mInstance = LogicRoot.Instance.GetSingletonObjectScript<TextureMgr>("AssetMgr", "TextureMgr");
			}
			return mInstance;
		}
	}
	
	class TexLoadParam
	{
		public TexLoadParam(ResParamLoadCallBack<Texture2D> cb, string filePath, object param)
		{
			this.cb = cb;
			this.filePath = filePath;
			this.userParam = param;
		}
		public ResParamLoadCallBack<Texture2D> cb;
		public string filePath;
		public object userParam;
	}
	
	LinkedList<ResCounter<Texture2D>> m_listLoadingList = new LinkedList<ResCounter<Texture2D>>();
	Dictionary<string, ResCounter<Texture2D>> m_dictTexture = new Dictionary<string, ResCounter<Texture2D>>();
	List<TexLoadParam> m_listTexLoadParam = new List<TexLoadParam>();
	
	public void LoadTexture(string filePath, ResParamLoadCallBack<Texture2D> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
		if(string.IsNullOrEmpty(filePath)) return;
		// add to list first
		m_listTexLoadParam.Add(new TexLoadParam(loadCB, filePath, userParam));
		
		ResCounter<Texture2D> res = null;
		if(m_dictTexture.ContainsKey(filePath))
		{
			res = m_dictTexture[filePath];
			if(null != res.Res)
			{
				res.AddRef();
				
				//if(null != progressCB) res.ProgressCallBack += progressCB;
				//if(null != loadCB) res.LoadCallBack += loadCB;
				if(null != progressCB) progressCB(res.ResPath, 1f);
				OnAssetOK(res);
				return;
			}
			
			/*  如果丢失的话，重新加载，并且清理一下无用资源，防止泄露  */
			m_dictTexture.Remove(filePath);
			Resources.UnloadUnusedAssets();
		}
		else
		{
			//get path
			res = new ResCounter<Texture2D>();
			res.ResPath = filePath;

			m_dictTexture.Add(filePath, res);
		}

		res.AddRef();
		m_listLoadingList.AddLast(res);
		//if(null != loadCB) res.LoadCallBack += loadCB;
		if(null != progressCB) res.ProgressCallBack += progressCB;
	}
	
	public void UnLoadTexture(string filePath)
	{
		if(!m_dictTexture.ContainsKey(filePath)) return;
		ResCounter<Texture2D> res = m_dictTexture[filePath];
		res.Release();
		if(res.Ref <= 0)
		{
			UnLoadRes(res);
			m_dictTexture.Remove(filePath);
		}
	}
	
	void UnLoadRes(ResCounter<Texture2D> res)
	{
		//Do Not Destroy Here
		if(null != res.Res) UnityEngine.Object.DestroyImmediate(res.Res, true);
		Resources.UnloadUnusedAssets();
	}
	
	void OnAssetLoadCallBack(bool success, ResCounter<Texture2D> res)
	{
		if(!m_dictTexture.ContainsKey(res.ResPath))
		{
			UnLoadRes(res);
			return;
		}
		
		if(!success)
		{
			OnAssetFailed(res);
			return;
		}
		
		if(success)
		{
			if(null == res.Res && null != res.AssetWWW)
			{
				res.Res = res.AssetWWW.texture as Texture2D;
				if(null == res.Res)
				{
					OnAssetFailed(res);
					return;
				}
			}
			
			OnAssetOK(res);
			
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
			//StartCoroutine(LoadFromBundle(res));
		}
	}
	
	void OnAssetOK(ResCounter<Texture2D> res)
	{
		if(null == res) return;
		for(int i = m_listTexLoadParam.Count - 1; i <= 0; --i)
		{
			TexLoadParam param = m_listTexLoadParam[i];
			if(null != param && param.filePath == res.ResPath)
			{				
				if(param.cb != null)
				{
					param.cb(res.ResPath, res.Res, param.userParam);
				}
			}
			m_listTexLoadParam.RemoveAt(i);
		}
		//res.LoadCallBack(res.ResPath, res.Res);
		res.LoadCallBack = null;
		res.ProgressCallBack = null;
	}
	
	void OnAssetFailed(ResCounter<Texture2D> res)
	{
		if(null == res) return;
		
		/*  即使失败也要调用一下回调函数  */
		OnAssetOK(res);
		
		Debug.LogException(new Exception("Model Load Failed, filePath = " + res.ResPath));
		if(m_dictTexture.ContainsKey(res.ResPath))
		{
			m_dictTexture.Remove(res.ResPath);
		}
		UnLoadRes(res);
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(m_listLoadingList.Count > 0)
		{
			ResCounter<Texture2D> res = m_listLoadingList.First.Value;
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

