using System.Collections.Generic;

public class SkillCfg
{
	public int SkillId;
	public byte AffNum;
	public byte AffRange;
	public byte AffDirection;
	public byte EffectType;
	public string EffectFile;
	public float EffectSpeed;
	public byte EffectHardPoint;
	public float AnimTime;
	public List<EAnimType> AnimList;
	
	
	
	
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
	
	public EHardPoint TargetHardPoint
	{
		get { return (EHardPoint)EffectHardPoint; }
	}
}