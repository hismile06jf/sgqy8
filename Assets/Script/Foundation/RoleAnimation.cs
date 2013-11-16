using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	string playAnimName;
	List<string> listAnimation = new List<string>();
	List<AnimationClip> listReadyAnimClip = new List<AnimationClip>();
	
	public Animation RoleAnimation
	{
		get { return null == MainBodyObj ? null : MainBodyObj.GetComponent<Animation>(); }
	}
	
	public void PlayAnim(string animName)
	{
		Animation anim = RoleAnimation;
		if(null != anim)
		{
			if(null != anim.GetClip(animName) && !anim.IsPlaying(animName))
			{
				anim.Play(animName);
				return;
			}
		}
		
		//try load
		playAnimName = animName;
		string animPath = GetAnimPath(animName);
		LoadAnim(animPath);
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

