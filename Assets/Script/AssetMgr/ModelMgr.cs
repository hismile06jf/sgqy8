using System;
using UnityEngine;
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
	
	class ModelCfgLoadParam
	{
		public ModelCfgLoadParam(ResParamLoadCallBack<Model> cb, ModelCfg cfg, string filePath, object param)
		{
			this.cb = cb;
			this.cfg = cfg;
			this.filePath = filePath;
			this.userParam = param;
		}
		public ResParamLoadCallBack<Model> cb;
		public ModelCfg cfg;
		public string filePath;
		public object userParam;
	}
	
	LinkedList<ResCounter<GameObject>> m_listLoadingList = new LinkedList<ResCounter<GameObject>>();
	Dictionary<string, ResCounter<GameObject>> m_dictModel = new Dictionary<string, ResCounter<GameObject>>();
	List<ModelCfgLoadParam> m_listModelCfg = new List<ModelCfgLoadParam>();
	
	void LoadModel(ModelCfg cfg, ResParamLoadCallBack<Model> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
		if(null == cfg) return;
		// add to list first
		m_listModelCfg.Add(new ModelCfgLoadParam(loadCB, cfg, cfg.FilePath, userParam));
		
		ResCounter<GameObject> res = null;
		if(m_dictModel.ContainsKey(cfg.FilePath))
		{
			res = m_dictModel[cfg.FilePath];
			if(null != res.Res)
			{
				res.AddRef();
				
				//if(null != progressCB) res.ProgressCallBack += progressCB;
				//if(null != loadCB) res.LoadCallBack += loadCB;
				if(null != progressCB) progressCB(res.ResPath, 1f);
				OnAssetOK(res);
				return;
			}
			else
			{
				if(null != res.Bundle)
				{
					//if(null != loadCB) res.LoadCallBack += loadCB;
					//if(null != progressCB) res.ProgressCallBack += progressCB;
					if(null != progressCB) progressCB(res.ResPath, 1f);
					OnAssetLoadCallBack(true, res);
					return;
				}
			}
		}
		else
		{
			//get path
			res = new ResCounter<GameObject>();
			res.ResPath = cfg.FilePath;

			m_dictModel.Add(cfg.FilePath, res);
		}

		res.AddRef();
		m_listLoadingList.AddLast(res);
		//if(null != loadCB) res.LoadCallBack += loadCB;
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
	
	/*   只给一个文件名，就只能模拟一个数据，然后加载   */
	public void LoadModel(string modelPath, ResParamLoadCallBack<Model> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
		ModelCfg cfg = new ModelCfg();
		cfg.FilePath = modelPath;
		LoadModel(cfg, loadCB, progressCB, userParam);
	}
	
	/*   只给一个Id，就只能获取数据，然后加载   */
	public void LoadModel(int modelId, ResParamLoadCallBack<Model> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
		ModelCfg cfg = null;
		if(null == cfg) return;
		
		LoadModel(cfg, loadCB, progressCB, userParam);
	}
	
	public void UnLoadModel(int modelId)
	{
		ModelCfg cfg = null;
		if(null == cfg) return;
		
		UnLoadModel(cfg.FilePath);
	}
	
	void UnLoadRes(ResCounter<GameObject> res)
	{
		//Do Not Destroy Here
		if(null != res.Res) GameObject.DestroyImmediate(res.Res, true);
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
			Debug.LogException(new Exception("Model Load Failed, filePath = " + res.ResPath));
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
			
			OnAssetOK(res);
			
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
			//StartCoroutine(LoadFromBundle(res));
		}
	}
	
	void OnAssetOK(ResCounter<GameObject> res)
	{
		if(null == res) return;
		for(int i = m_listModelCfg.Count - 1; i >= 0; --i)
		{
//			try
//			{
				ModelCfgLoadParam param = m_listModelCfg[i];
				if(null != param && param.filePath == res.ResPath)
				{
					m_listModelCfg.RemoveAt(i);
				
					Model m = null;
					GameObject objModel = GameObject.Instantiate(res.Res) as GameObject;
					if(null != objModel)
					{
						m = objModel.AddComponent<Model>();
						m.SetModelCfg(param.cfg);
					}
					
					if(param.cb != null)
					{
						param.cb(res.ResPath, m, param.userParam);
					}
				}
//			}
//			catch(Exception e)
//			{
//				Debug.LogError("Model OnAssetOK, exception = " + e.Message);
//			}
//			
			//
		}
		//res.LoadCallBack(res.ResPath, res.Res);
		res.LoadCallBack = null;
		res.ProgressCallBack = null;
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

