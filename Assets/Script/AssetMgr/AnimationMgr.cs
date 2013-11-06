using UnityEngine;
using System.Collections.Generic;

public class AnimationMgr : MonoBehaviour {
	
	static AnimationMgr mInstance;
	static public AnimationMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				GameObject obj = new GameObject("GameLogic/AssetMgr/AnimationMgr");
				DontDestroyOnLoad(obj);
				mInstance = obj.AddComponent<AnimationMgr>();
			}
			return mInstance;
		}
	}
	
	LinkedList<ResCounter<AnimationClip>> m_listLoadingList = new LinkedList<ResCounter<AnimationClip>>();
	Dictionary<string, ResCounter<AnimationClip>> m_dictAnimationClip = new Dictionary<string, ResCounter<AnimationClip>>();
	public void LoadAnimation(string animPath, ResLoadCallBack<AnimationClip> loadCB, ResLoadProgressCallBack progressCB)
	{
		ResCounter<AnimationClip> res = null;
		if(m_dictAnimationClip.ContainsKey(animPath))
		{
			res = m_dictAnimationClip[animPath];
			if(null != res.Res)
			{
				res.AddRef();
				if(null != progressCB) progressCB(animPath, 1f);
				if(null != loadCB) loadCB(animPath, res.Res);
				return;
			}
		}
		else
		{
			//get path
			res = new ResCounter<AnimationClip>();
			res.ResPath = animPath;

			m_dictAnimationClip.Add(animPath, res);
			m_listLoadingList.AddLast(res);
		}

		res.AddRef();
		if(null != loadCB) res.LoadCallBack += loadCB;
		if(null != progressCB) res.ProgressCallBack += progressCB;
	}
	
	public void UnLoadAnimation(string animPath)
	{
		if(!m_dictAnimationClip.ContainsKey(animPath)) return;
		ResCounter<AnimationClip> res = m_dictAnimationClip[animPath];
		res.Release();
		if(res.Ref <= 0)
		{
			UnLoadRes(res);
			m_dictAnimationClip.Remove(animPath);
		}
	}
	
	void UnLoadRes(ResCounter<AnimationClip> res)
	{
		if(null != res.Res) Object.DestroyImmediate(res.Res, true);
		if(null != res.AssetWWW) 
		{
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
		}
		Resources.UnloadUnusedAssets();
	}
	
	void OnAssetLoadCallBack(bool success, ResCounter<AnimationClip> res)
	{
		if(!m_dictAnimationClip.ContainsKey(res.ResPath))
		{
			UnLoadRes(res);
			return;
		}
		
		if(success)
		{
			GameObject gameObj = res.AssetWWW.assetBundle.mainAsset as GameObject;
			Animation animation = gameObj.animation;
			if(null != animation) res.Res = animation.clip;
			res.AssetWWW.assetBundle.Unload(false);
		}
		
		if(null != res.AssetWWW)
		{
			res.AssetWWW.Dispose();
			res.AssetWWW = null;
		}
		
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
			ResCounter<AnimationClip> res = m_listLoadingList.First.Value;
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
