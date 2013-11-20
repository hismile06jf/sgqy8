using UnityEngine;
using System.Collections.Generic;

public class AnimQueueInfo
{
	public AnimQueueInfo(string name, float time)
	{
		animName = name;
		animTime = time;
	}
	
	public string animName;
	public float animTime;
}

public partial class Role
{
	string playAnimName;
	List<AnimQueueInfo> playAnimList = new List<AnimQueueInfo>();
	
	List<string> listAnimation = new List<string>();
	List<AnimationClip> listReadyAnimClip = new List<AnimationClip>();
	
	public Animation RoleAnimation
	{
		get { return null == MainBodyObj ? null : MainBodyObj.GetComponent<Animation>(); }
	}
	
	public void PlayAnim(EAnimType anim)
	{
		string animName = GetAnimName(anim);
		PlayAnim(animName);
	}
	
	public void PlayAnim(string animName)
	{
		if(string.IsNullOrEmpty(animName)) return;
		
		Animation anim = RoleAnimation;
		if(null != anim)
		{
			AnimationClip clip = anim.GetClip(animName);
			if(null != clip && !anim.IsPlaying(animName))
			{
//				if(playAnimList.Contains(animName))
//				{
//					AnimationEvent animEvent = new AnimationEvent();
//					animEvent.functionName = "OnAnimEvent";
//					animEvent.messageOptions = SendMessageOptions.DontRequireReceiver;
//					clip.AddEvent(animEvent);
//				}
				
				anim.Play(animName);
				return;
			}
		}
		
		//try load
		playAnimName = animName;
		string animPath = GetAnimPath(animName);
		LoadAnim(animPath);
	}
	
	public void PlayAnimQueue(List<AnimQueueInfo> animList)
	{
		if(null == animList || 0 == animList.Count) return;
		
		//laod first
		for(int i = 0; i < animList.Count; ++i)
		{
			LoadAnim(animList[i].animName);
		}
		
		//
	}
	
	public void LoadAnim(string animPath)
	{
		if(listAnimation.Contains(animPath)) return;		
		AnimationMgr.Instance.LoadAnimation(animPath, OnAnimationLoadCallBack, null);
	}
	
	void OnAnimationLoadCallBack(string resPath, AnimationClip clip)
	{
		if(null == clip) return;
		
		listAnimation.Add(resPath);
		if(IsMainBodyReady)
		{
			Animation anim = RoleAnimation;
			if(null != anim) 
			{
				anim.AddClip(clip, clip.name);
			}
			
			PlayAnim(playAnimName);
		}
		else
		{
			listReadyAnimClip.Add(clip);
		}
	}
	
	void ProcessReadyAnim()
	{
		Animation anim = RoleAnimation;
		if(null != anim)
		{
			for(int i = 0; i < listReadyAnimClip.Count; ++i)
			{
				AnimationClip clip = listReadyAnimClip[i];
				if(null != clip) anim.AddClip(clip, clip.name);
			}
			listReadyAnimClip.Clear();
		}
		
		if(!string.IsNullOrEmpty(playAnimName))
		{
			PlayAnim(playAnimName);
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	//generally human role
	virtual public string GetAnimName(EAnimType type)
	{
		switch(type)
		{
		case EAnimType.Idle: return "w_w01";
			
		case EAnimType.Rest_Down: return "h_r01_1";
		case EAnimType.Rest_Idle: return "h_r01_2";
		case EAnimType.Rest_Idle_a: return "h_r01_3";
		case EAnimType.Rest_Up: return "h_r02";
			
		case EAnimType.Walk_Fore: return "h_m01";
		case EAnimType.Walk_Back: return "h_m02";
		case EAnimType.Walk_TurnLeft: return "h_m05";
		case EAnimType.Walk_TurnRight: return "h_m06";
		case EAnimType.Walk_Die: return "h_f02_1";
		case EAnimType.Walk_DieHold: return "h_f02_2";
			
		case EAnimType.Run_Fore: return "h_m03";
		case EAnimType.Run_Back: return "h_m02";
		case EAnimType.Run_RurnLeft: return "h_m05";
		case EAnimType.Run_RurnRight: return "h_m06";
		case EAnimType.Run_Die: return "h_f02_1";
		case EAnimType.Run_DieHold: return "h_f02_2";
		}
		
		return "w_w01";
	}
	
	virtual public string GetAnimPath(string animName)
	{
		return RoleCfg.GetRoleAnimPath(ERoleType.UserL, animName);
	}
}

