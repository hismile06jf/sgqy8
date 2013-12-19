using System;

public enum ESkillEffectType
{
	TrackTarget,
	SelectPlace,
	Target,
	User,
}

public enum ESkillFireSrc
{
	WeaponLeft,
	WeaponRight,
	Ride,
	Role,
}

public enum ESkillDir
{
	Fore,
	Back,
	All,
}

public class Skill
{
	SkillCfg cfg;
	public Skill(int skillId)
	{
		cfg = ResMgr.Instance.GetSkillCfg(skillId);
	}
	
	
}

