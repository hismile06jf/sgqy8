using UnityEngine;
using System.Collections.Generic;

public class AnimQueueInfo
{
	public AnimQueueInfo(string name, bool loaded)
	{
		animName = name;
		isLoad = loaded;
	}
	
	public string animName;
	public bool isLoad;
}

public partial class Role
{
	string playAnimName;
	
	float playAnimListTotalTime = 0f;
	float playAnimListLoopTime = 0f;
	float playAnimListSpeed = 0f;
	bool isAnimListPlaying = false;
	List<AnimQueueInfo> playAnimList = new List<AnimQueueInfo>();
	
	List<string> listAnimation = new List<string>();
	List<AnimationClip> listReadyAnimClip = new List<AnimationClip>();
	
	public Animation RoleAnimation
	{
		get { return null == MainBodyObj ? null : MainBodyObj.GetComponent<Animation>(); }
	}
	
	public bool HaveAnim(string animName)
	{
		return null != RoleAnimation && null != RoleAnimation.GetClip(animName);
	}
	
	public bool IsPlaying(string animName)
	{
		if(string.IsNullOrEmpty(animName) || null == RoleAnimation) return false;
		return RoleAnimation.IsPlaying(animName);
		//AnimationState state = RoleAnimation[animName];
		//return null != state && ;
	}
	
	public void PlayAnim(EAnimType anim)
	{
		string animName = GetAnimName(anim);
		PlayAnim(animName);
	}
	
	public void PlayAnim(string animName)
	{
		if(string.IsNullOrEmpty(animName) || IsPlaying(animName)) return;
		
		Animation anim = RoleAnimation;
		if(null != anim)
		{
			AnimationState state = anim[animName];
			if(null != state && null != state.clip && !anim.IsPlaying(animName))
			{
				state.speed = 1f;
				if(state.clip.wrapMode == WrapMode.Loop) 
				{
					state.wrapMode = WrapMode.Loop;
				}
				anim.Play(animName);
				playAnimName = null;
				return;
			}
		}
		
		//try load
		playAnimName = animName;
		string animPath = GetAnimPath(animName);
		LoadAnim(animPath);
	}
	
	public bool IsAnimQueuePlaying
	{
		get { return playAnimList.Count > 0 && isAnimListPlaying; }
	}
	
	public void PlayAnimQueue(List<EAnimType> animList, float totalTime)
	{
		if(playAnimList.Count > 0 || null == animList || 0 == animList.Count) return;
		
		//laod first
		for(int i = 0; i < animList.Count; ++i)
		{
			string animName = GetAnimName(animList[i]);
			bool isLoad = HaveAnim(animName);
			
			if(!isLoad)
			{
				LoadAnim(GetAnimPath(animName));
			}
			
			playAnimList.Add(new AnimQueueInfo(animName, isLoad));
		}
		
		playAnimListTotalTime= totalTime;
		isAnimListPlaying = false;
		CheckAndPlayAnimQueue();
	}
	
	public void LoadAnim(string animPath)
	{
		if(listAnimation.Contains(animPath)) return;		
		AnimationMgr.Instance.LoadAnimation(animPath, OnAnimationLoadCallBack, null);
	}
	
	bool IsAnimQueueReady
	{
		get 
		{
			if(playAnimList.Count == 0) return false;
			for(int i = 0; i < playAnimList.Count; ++i)
			{
				if(!playAnimList[i].isLoad) return false;
			}
			
			return true;
		}
	}
	
	void CheckAndPlayAnimQueue()
	{
		if(!IsAnimQueueReady || IsAnimQueuePlaying) return;
		
		float totalTime = 0f;
		float loopAnimTime = 0f;
		AnimationState state = null;
		for(int i = 0; i < playAnimList.Count; ++i)
		{
			AnimQueueInfo anim = playAnimList[i];
			state = RoleAnimation[anim.animName];
			totalTime += state.clip.length;
			
			if(state.clip.wrapMode == WrapMode.Loop) loopAnimTime = state.clip.length;
		}
		
		playAnimListSpeed = 1f;
		float exludeLoopTime = totalTime - loopAnimTime;
		playAnimListLoopTime = playAnimListTotalTime - exludeLoopTime;
		if(playAnimListLoopTime <= 0f)
		{
			playAnimListSpeed = playAnimListTotalTime / exludeLoopTime;
		}
		
		state = RoleAnimation[playAnimList[0].animName];
		state.speed = playAnimListSpeed;
		TimeMgr.Instance.Exec(AnimQueuePlayFinished, 0, (int)(state.clip.length * playAnimListSpeed * 1000f));
		RoleAnimation.Play(state.name);
		isAnimListPlaying = true;
	}
	
	//use for queued anim
	void AnimQueuePlayFinished(object param)
	{
		int currIndex = (int)param;
		if(!IsAnimQueueReady || currIndex < 0 || currIndex >= playAnimList.Count) return;
		
		currIndex++;
		
		//play finished
		if(currIndex == playAnimList.Count)
		{
			//
			isAnimListPlaying = false;
			playAnimList.Clear();
			PlayAnim(EAnimType.Idle);
			return;
		}
		
		AnimationState state = RoleAnimation[playAnimList[currIndex].animName];
		AnimationClip clip = state.clip;
		float time = clip.length;
		if(clip.wrapMode == WrapMode.Loop)
		{
			if(playAnimListLoopTime > 0f)
			{
				state.wrapMode = WrapMode.Loop;
				time = playAnimListLoopTime;
			}
			else
			{
				//play next
				AnimQueuePlayFinished(currIndex);
				return;
			}
		}
		
		time *= playAnimListSpeed;
		
		state.speed = playAnimListSpeed;
		RoleAnimation.Play(clip.name);
		TimeMgr.Instance.Exec(AnimQueuePlayFinished, currIndex, (int)(time * 1000f));
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
			
			//check is in list
			for(int i = 0; i < playAnimList.Count; ++i)
			{
				if(playAnimList[i].animName == clip.name)
				{
					playAnimList[i].isLoad = true;
				}
			}
			
			ProcessReadyAnim();
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
			
		CheckAndPlayAnimQueue();
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
			
		case EAnimType.Jump_Begin: return "h_f02_2";
		case EAnimType.Jump_Hold: return "h_f02_2";
		case EAnimType.Jump_End: return "h_f02_2";
		case EAnimType.Jump_EndRun: return "h_f02_2";
			
		case EAnimType.Swim_Fore: return "h_f02_2";
		case EAnimType.Swim_Back: return "h_f02_2";
		case EAnimType.Swim_TurnLeft: return "h_f02_2";
		case EAnimType.Swim_Die: return "h_f02_2";
		case EAnimType.Swim_Die_a: return "h_f02_2";
			
		//attack
		case EAnimType.Attack_Arrow_Begin: return isRide ? "r_a09_1" : "w_a09_1";
		case EAnimType.Attack_Arrow_Hold: return isRide ? "r_a09_2" : "w_a09_2";
		case EAnimType.Attack_Arrow_Fire: return isRide ? "r_a09_3" : "w_a09_3";
		}
		
		return "w_w01";
	}
	
	virtual public string GetAnimPath(string animName)
	{
		return RoleCfg.GetRoleAnimPath(ERoleType.UserL, animName);
	}
}

