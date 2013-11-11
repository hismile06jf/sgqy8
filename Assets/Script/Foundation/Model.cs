using System;
using UnityEngine;

public class Model : MonoBehaviour {
	
	ModelCfg modelCfg;
	
	public int ModelId
	{
		get { return null == modelCfg ? 0 : modelCfg.Id; }
	}
	
	public void InitModel(ModelCfg cfg)
	{
		modelCfg = cfg;
		if(null == modelCfg) return;
		
		for(int i = 0; i < cfg.MtrlList.Count; ++i)
		{
			ModelMtrl mtrl = cfg.MtrlList[i];
			if(null == mtrl)
			{
				Debug.LogException(new Exception("Model Id = " + cfg.Id.ToString() + "Mtrl[" + i.ToString() + "] is null."));
				continue;
			}
			
			for(int j = 0; j < mtrl.TexList.Count; ++j)
			{
				MtrlTex tex = mtrl.TexList[j];
				if(null == tex)
				{
					Debug.LogException(new Exception("Model Id = " + cfg.Id.ToString() + 
						"Mtrl[" + i.ToString() + "]Tex[" + j.ToString() + "] is null."));
				}
				
				TextureMgr.Instance.LoadTexture(tex.FilePath, OnTextureLoadCallBack, null, new ModelTexParam(mtrl.Name, tex.Name));
			}
		}
	}
	
	/**************************************************************************************/
	class ModelTexParam
	{
		public ModelTexParam(string mtrlName, string texName)
		{
			this.mtrlName = mtrlName;
			this.texName = texName;
		}
		
		string mtrlName;
		string texName;
		
		public string MtrlName 	{ get { return mtrlName; } }
		public string TexName	{ get { return texName; } }
	}
	
	void OnTextureLoadCallBack(string filePath, Texture2D tex, object userParam)
	{
		if(null == tex)
		{
			Debug.LogException(new Exception(filePath + " Load Failed."));
			return;
		}
		
		ModelTexParam param = (ModelTexParam)userParam;
		if(null == param) return;
		
		
	}
	/**************************************************************************************/
}

