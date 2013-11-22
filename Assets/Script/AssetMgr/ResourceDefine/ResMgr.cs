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
	static ModelCfg model111;
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
		
		if(modelId == 111)
		{
			if(null == model111)
			{
				model111 = new ModelCfg();
				model111.Id = 111;
				model111.FilePath = "file://D:/WorkSpace/sgqy8thunk/AssetBundle/h1_ho0111.unity3d";
			}
			
			return model111;
		}
		
		return null;
	}
	
	SkillCfg skill001 = null;
	public SkillCfg GetSkillCfg(int skillId)
	{
		if(null == skill001)
		{
			skill001 = new SkillCfg();
			skill001.SkillId = 1;
			skill001.AffNum = 1;
			skill001.AffRange = 0;
			skill001.AffDirection = (byte)ESkillDir.Fore;
			skill001.EffectType = (byte)ESkillEffectType.Target;
			skill001.EffectFile = "file://D:/WorkSpace/sgqy8thunk/AssetBundle/Effect/efftest.unity3d";
			skill001.EffectSpeed = 1f;
			skill001.AnimTime = 1f;
			skill001.AnimList = new List<EAnimType>();
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Begin);
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Hold);
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Fire);
		}
		
		return skill001;
	}
}

