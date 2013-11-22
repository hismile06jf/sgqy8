using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	GameObject objSelectTarget;
	
	public bool CanUseSkill
	{
		get { return currState != ERoleState.Trade; }
	}
	
	public void UseSkill(int skillId, float time)
	{
		if(!CanUseSkill) 
		{
			//show reason
			return;
		}
		
		SkillCfg skill = ResMgr.Instance.GetSkillCfg(skillId);
		if(null == skill) return;
		
		if(skill.IsAffTarget)
		{
			//null != objSelectTarget && 
			if(skill.IsAnimValid)
			{
				PlayAnimQueue(skill.AnimList, time);
			}
		}
		else if(skill.IsAffRange)
		{
		}
	}
	
//	Transform findSkillTarget(ESkillDir dir)
//	{
//		
//	}
	
	void AttackSingleTarget(GameObject objTarget)
	{
		//
	}
	
	void OnSkillAnimPlayCallBack()
	{
		//play effect
		currState = ERoleState.Idle;
	}
}