using System.Collections.Generic;

public class SkillCfg
{
	public int SkillId;
	public byte SkillType;
	public byte AffNum;
	public byte AffRange;
	public byte AffDirection;
	public string EffectFile;
	public float EffectSpeed;
	public float AnimTime;
	public List<EAnimType> AnimList;
	
	
	
	
	public int CalDamage()
	{
		return 1000;
	}
	
	
}