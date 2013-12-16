using UnityEngine;
using System.Collections.Generic;

public class EffectMgr : MonoBehaviour {
	
	static EffectMgr mInstance;
	static public EffectMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				mInstance = LogicRoot.Instance.GetSingletonObjectScript<EffectMgr>("AssetMgr", "EffectMgr");
			}
			return mInstance;
		}
	}
	
	LinkedList<ResCounter<GameObject>> m_listLoadingList = new LinkedList<ResCounter<GameObject>>();
	Dictionary<string, ResCounter<GameObject>> m_dictGameObject = new Dictionary<string, ResCounter<GameObject>>();
	public void LoadEffect(string fxPath, ResParamLoadCallBack<GameObject> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
		if(string.IsNullOrEmpty(fxPath)) return;
		
		ResCounter<GameObject> res = null;
		if(m_dictGameObject.ContainsKey(fxPath))
		{
			res = m_dictGameObject[fxPath];
			if(null != res.Res)
			{
				res.AddRef();
				if(null != progressCB) progressCB(fxPath, 1f, userParam);
				if(null != loadCB) loadCB(fxPath, res.Res, userParam);
				return;
			}
		}
		else
		{
			//get path
			res = new ResCounter<GameObject>();
			res.ResPath = fxPath;
			
			m_dictGameObject.Add(fxPath, res);
			m_listLoadingList.AddLast(res);
		}
		
		res.AddRef();
		if(null != loadCB || null != progressCB) 
		{
			res.AddLoadParam(userParam, loadCB, progressCB);
		}
	}
	
	public void UnLoadEffect(string fxPath)
	{
		if(!m_dictGameObject.ContainsKey(fxPath)) return;
		ResCounter<GameObject> res = m_dictGameObject[fxPath];
		res.Release();
		if(res.Ref <= 0)
		{
			UnLoadRes(res);
			m_dictGameObject.Remove(fxPath);
		}
	}
	
	void UnLoadRes(ResCounter<GameObject> res)
	{
		if(null != res.Res) Object.DestroyImmediate(res.Res, true);
		Resources.UnloadUnusedAssets();
	}
	
	void OnAssetLoadCallBack(bool success, ResCounter<GameObject> res)
	{
		if(!m_dictGameObject.ContainsKey(res.ResPath))
		{
			UnLoadRes(res);
			return;
		}
		
		if(success && null != res.AssetWWW && null != res.AssetWWW.assetBundle)
		{
			GameObject gameObj = res.AssetWWW.assetBundle.mainAsset as GameObject;
			res.Res = gameObj;

			/*  调用Unload(false)的话会减少asset数量，但是很少，测试只有1，而且只能调用一次，下次调用Unload(true)时，不会卸载资源  */
			/*  assetBundle.Unload 只能调用一次，为了防止泄露，只在删除模型的时候调用  */
			//res.AssetWWW.assetBundle.Unload(false);
		}
		else
		{
			m_dictGameObject.Remove(res.ResPath);
			UnLoadRes(res);
		}
		
		if(null != res.AssetWWW)
		{
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
		}
		
		res.DispatchLoaCallBack();
		res.ClearLoadParam();
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
				bool success = (null == res.AssetWWW.error && null != res.AssetWWW.assetBundle);
				if(!success)
				{
					Debug.LogError("Res Download Failed, respath = " + res.ResPath);
				}
				
				res.DispatchProgress(res.ResPath, res.AssetWWW.progress);
				
				OnAssetLoadCallBack(success, res);
				m_listLoadingList.RemoveFirst();
			}
			else
			{
				res.DispatchProgress(res.ResPath, res.AssetWWW.progress);
			}
		}
	}
}
