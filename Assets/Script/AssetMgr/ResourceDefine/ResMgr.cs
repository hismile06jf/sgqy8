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
				model88.FilePath = AssetPath.GetAssetStorePathWithSlash() + "we0088.unity3d";
			}
			
			return model88;
		}
		
		if(modelId == 9)
		{
			if(null == model09)
			{
				model09 = new ModelCfg();				
				model09.Id = 9;
				model09.FilePath = AssetPath.GetAssetStorePathWithSlash() + "so0009.unity3d";
				
				model09.MtrlList = new List<ModelMtrl>();
				ModelMtrl mtrl = new ModelMtrl();
				mtrl.Name = "so0009";
				mtrl.TexList = new List<MtrlTex>();
				mtrl.TexList.Add(new MtrlTex("_MainTex", AssetPath.GetAssetStorePathWithSlash() + "Textures/so0009.unity3d"));
				mtrl.TexList.Add(new MtrlTex("_ClothTex", AssetPath.GetAssetStorePathWithSlash() + "Textures/so0009_cloth.unity3d"));
				
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
				model111.FilePath = AssetPath.GetAssetStorePathWithSlash() + "h1_ho0001.unity3d";
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

			SkillEffect eff = new SkillEffect();
			eff.AnimType = EAnimType.Attack_Arrow_Fire;
			eff.EffectBegin = false;
			eff.EffectType = (byte)ESkillEffectType.Target;
			eff.EffectFile = AssetPath.GetAssetStorePathWithSlash() + "Effect/efftest.unity3d";
			eff.EffectSpeed = 1f;
			eff.EffectHardPoint = (byte)EHardPoint.Back;

			skill001.EffectList = new List<SkillEffect>();
			skill001.EffectList.Add(eff);
			skill001.AnimTime = 1f;
			skill001.AnimList = new List<EAnimType>();
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Begin);
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Hold);
			skill001.AnimList.Add(EAnimType.Attack_Arrow_Fire);
		}
		
		return skill001;
	}
}

