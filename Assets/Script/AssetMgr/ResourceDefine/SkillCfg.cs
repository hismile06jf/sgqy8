using System.Collections.Generic;

public class SkillEffect
{
	public EAnimType AnimType;
	public bool EffectBegin;
	public byte EffectType;
	public string EffectFile;
	public float EffectTime;
	public float EffectSpeed;
	public byte EffectHardPoint;	
	public byte EffectFireSrc;
	public byte EffectFireSrcHardPoint;


	public EHardPoint TargetHardPoint
	{
		get { return (EHardPoint)EffectHardPoint; }
	}
	
	public ESkillEffectType SkillEffectType
	{
		get { return (ESkillEffectType)EffectType; }
	}

	public ESkillFireSrc SkillFireSrc
	{
		get { return (ESkillFireSrc)EffectFireSrc; }
	}

	public EHardPoint SkillFireSrcHardPoint
	{
		get { return (EHardPoint)EffectFireSrcHardPoint; }
	}
}

public class SkillCfg
{
	public int SkillId;
	public byte AffNum;
	public byte AffRange;
	public byte AffDirection;
	public float AnimTime;
	public List<EAnimType> AnimList;
	public List<SkillEffect> EffectList;
	
	
	
	public int CalDamage()
	{
		return 1000;
	}
	
	public bool IsAffTarget
	{
		get { return 0 == AffRange; }
	}
	
	public bool IsAffRange
	{
		get { return 0 != AffRange && 0 != AffNum; }
	}
	
	public bool IsAnimValid
	{
		get { return AnimTime != 0 && null != AnimList && 0 < AnimList.Count; }
	}
	
	public SkillEffect GetAnimEffect(EAnimType type, bool begin)
	{
		if(null == EffectList) return null;
		SkillEffect effect = null;
		for(int i = 0; i < EffectList.Count; ++i)
		{
			effect = EffectList[i];
			if(null != effect && 
			   effect.AnimType == type &&
			   effect.EffectBegin == begin) 
			{
				return effect;
			}
		}

		return null;
	}
}