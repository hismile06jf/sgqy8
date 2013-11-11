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
	
	public void LoadTexture(string filePath, ResParamLoadCallBack<Texture2D> loadCB, ResLoadProgressCallBack progressCB, object userParam)
	{
	}
	
	public void UnLoadTexture(string filePath)
	{
	}
}

