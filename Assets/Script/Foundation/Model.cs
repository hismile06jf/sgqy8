using System;
using UnityEngine;

public class Model : MonoBehaviour {
	
	ModelCfg modelCfg;
		
	public int ModelId
	{
		get { return null == modelCfg ? 0 : modelCfg.Id; }
	}
	
	public void SetModelCfg(ModelCfg cfg)
	{
		modelCfg = cfg;
		InitModel();
	}
	
	public void InitModel()
	{
		if(null == modelCfg) return;
		for(int i = 0; i < modelCfg.MtrlCount; ++i)
		{
			ModelMtrl mtrl = modelCfg.MtrlList[i];
			if(null == mtrl)
			{
				Debug.LogException(new Exception("Model Id = " + modelCfg.Id.ToString() + "Mtrl[" + i.ToString() + "] is null."));
				continue;
			}
			
			for(int j = 0; j < mtrl.TexCount; ++j)
			{
				MtrlTex tex = mtrl.TexList[j];
				if(null == tex)
				{
					Debug.LogException(new Exception("Model Id = " + modelCfg.Id.ToString() + 
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
		
		Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs)
		{
			Material mtrl = r.material;
			if(null != mtrl)
			{
				//mtrl.mainTexture = tex;
				//continue;
				/*  后面如果确认材质固定，可以不检测，直接设置贴图  */
				string mtrlName = mtrl.name.Replace(" (Instance)", "");
				
				if( mtrlName== param.MtrlName )
				{
					mtrl.SetTexture(param.TexName, tex);
				}
			}
				 
		}		
	}
	/**************************************************************************************/
}

