using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	Vector3 vSelectPos;
	GameObject objSelectTarget;
	SkillCfg currSkill;

	public Vector3 SelectTargetPostion
	{
		get { return vSelectPos; }
		set { vSelectPos = value; }
	}
	
	public GameObject SelectTarget
	{
		get { return objSelectTarget; }  
		set { objSelectTarget = value; }
	}

	public bool isSelectTarget
	{
		get { return SelectTarget != null;  }
	}

	public bool isSelectTargetPostion
	{
		get { return vSelectPos != Vector3.zero;  }
	}

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
		currSkill = skill;

		if(skill.IsAffTarget && isSelectTarget)
		{
			if(skill.IsAnimValid)
			{
				PlayAnimQueue(skill.AnimList, time);
			}
		}
		else if(skill.IsAffRange && isSelectTargetPostion)
		{
			//
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

	/*  begin：true开始播放，false结束播放  */
	void OnSkillAnimPlayCallBack(EAnimType animType, bool begin)
	{
		if(null == currSkill) return;

		//play effect

		SkillEffect skillEffect = currSkill.GetAnimEffect(animType, begin);
		if(null != skillEffect)
		{
			if(skillEffect.SkillEffectType == ESkillEffectType.User)
			{
				//
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.Target)
			{
				//
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.SelectPlace)
			{
				//
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.TrackTarget)
			{
				//
			}
		}


	}
}