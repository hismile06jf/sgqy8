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
	
	virtual public string GetAnimPath(string animName)
	{
		return animName;
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
}

