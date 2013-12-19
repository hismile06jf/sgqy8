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
			Transform target = null;
			Vector3 vPos = Vector3.zero;
			EHardPoint targetHP = EHardPoint.Back;
			if(skillEffect.SkillEffectType == ESkillEffectType.User || 
			   skillEffect.SkillFireSrc == ESkillFireSrc.Role )
			{
				if(!IsMainBodyReady)
				{
					Debug.LogError("SkillEffectType == User, but main body not ready.");
					return;
				}
				target = MainBodyObj.transform;
				targetHP = skillEffect.TargetHardPoint;
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.Target)
			{
				if(!IsMainBodyReady)
				{
					Debug.LogError("SkillEffectType == Target, but Target not select.");
					return;
				}
				target = SelectTarget.transform;
				targetHP = skillEffect.TargetHardPoint;
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.SelectPlace)
			{
				vPos = SelectTargetPostion;
			}
			else if(skillEffect.SkillEffectType == ESkillEffectType.TrackTarget)
			{
				if(skillEffect.SkillFireSrc == ESkillFireSrc.WeaponLeft)
				{
					GameObject weaponObj = GetAttachGameObject(GetHardPointName(EHardPoint.LeftHand));
					if(null == weaponObj)
					{
						Debug.LogError("SkillFireSrc == WeaponLeft, but weaponObj not ready.");
						return;
					}
					target = weaponObj.transform;
					targetHP = skillEffect.SkillFireSrcHardPoint;
				}
				else if(skillEffect.SkillFireSrc == ESkillFireSrc.WeaponRight)
				{
					GameObject weaponObj = GetAttachGameObject(GetHardPointName(EHardPoint.RightHand));
					if(null == weaponObj)
					{
						Debug.LogError("SkillFireSrc == WeaponRight, but weaponObj not ready.");
						return;
					}
					target = weaponObj.transform;
					targetHP = skillEffect.SkillFireSrcHardPoint;
				}
				else if(skillEffect.SkillFireSrc == ESkillFireSrc.Ride)
				{
					if(null == ride || !ride.IsMainBodyReady)
					{
						Debug.LogError("SkillFireSrc == Ride, but ride not ready.");
						return;
					}
					target = ride.MainBodyObj.transform;
					targetHP = skillEffect.SkillFireSrcHardPoint;
				}
			}

			if(null != target)
			{
				Transform hp = GetHardPoint(target, GetHardPointName(targetHP));
				if(null != hp) target = hp;
			}

			EffectUtility.PlayEffect(skillEffect.EffectFile, skillEffect.EffectTime, target, vPos, skillEffect, OnSkillFxPlayCallBack);
		}
	}

	void OnSkillFxPlayCallBack(EffectUtility.FxPlayParam param)
	{
		if(null == param) return;

		SkillEffect skillEffect = (SkillEffect)param.userParam;
		if(skillEffect.SkillEffectType == ESkillEffectType.TrackTarget || null != SelectTarget)
		{
			if(null != param.objFx && param.state == EffectUtility.FxPlayState.Begin)
			{
				param.objFx.transform.parent = null;
				FxTracker fxTracker = UnityTools.EnsureComponent<FxTracker>(param.objFx);
				fxTracker.speed = skillEffect.EffectSpeed;
				fxTracker.threshold = 0.01f;
				fxTracker.msgParam = param;
				fxTracker.onFxCallBack = OnFxTrackerCallBack;
				Transform target = SelectTarget.transform;
				if(null != target)
				{
					Transform hp = GetHardPoint(target, GetHardPointName(skillEffect.TargetHardPoint));
					if(null != hp) target = hp;
				}
				fxTracker.Target = target;
			}
		}
	}

	void OnFxTrackerCallBack(object param)
	{
		EffectUtility.FxPlayParam fx = (EffectUtility.FxPlayParam)param;
		if(null == fx) return;

		//FxTracker fxTracker = UnityTools.EnsureComponent<FxTracker>(param.objFx);
		//if(null != fxTracker) Object.Destroy(fxTracker);

		EffectUtility.DestroyEffect(fx);
	}
}