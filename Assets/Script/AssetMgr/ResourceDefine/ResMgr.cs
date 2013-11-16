using System.Collections.Generic;

public class ResMgr
{
	static ResMgr mInstance;
	static public ResMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				mInstance = new ResMgr();
			}
			return mInstance;
		}
	}
	
	static ModelCfg model88;
	static ModelCfg model09;
	public ModelCfg GetModelCfg(int modelId)
	{
		if(modelId == 88)
		{
			if(null == model88)
			{
				model88 = new ModelCfg();
				model88.Id = 88;
				model88.FilePath = "file://D:/WorkSpace/sgqy8thunk/AssetBundle/we0088.unity3d";
			}
			
			return model88;
		}
		
		if(modelId == 9)
		{
			if(null == model09)
			{
				model09 = new ModelCfg();				
				model09.Id = 9;
				model09.FilePath = "file://D:/WorkSpace/sgqy8thunk/AssetBundle/so0009.unity3d";
				
				model09.MtrlList = new List<ModelMtrl>();
				ModelMtrl mtrl = new ModelMtrl();
				mtrl.Name = "so0009";
				mtrl.TexList = new List<MtrlTex>();
				mtrl.TexList.Add(new MtrlTex("_MainTex", "file://D:/WorkSpace/sgqy8thunk/AssetBundle/Textures/so0009.unity3d"));
				mtrl.TexList.Add(new MtrlTex("_ClothTex", "file://D:/WorkSpace/sgqy8thunk/AssetBundle/Textures/so0009_cloth.unity3d"));
				
				model09.MtrlList.Add(mtrl);
			}
			
			return model09;
		}
		
		return null;
	}
}

