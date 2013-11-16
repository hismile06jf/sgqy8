using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	string currAnimation;
	List<AnimationClip> listReadyAnimClip = new List<AnimationClip>();
	
	public void PlayAnim(string animPath)
	{
		currAnimation = animPath;
		AnimationMgr.Instance.LoadAnimation(animPath, OnAnimationLoadCallBack, null);
	}
	
	void OnAnimationLoadCallBack(string resPath, AnimationClip clip)
	{
		if(IsMainBodyReady && null != clip)
		{
			Animation anim = MainBodyObj.GetComponent<Animation>();
			if(null != anim)
			{
				anim.AddClip(clip, clip.name);
				if(null != currAnimation) 
				{
					anim.wrapMode = WrapMode.Loop;
					anim.Play(clip.name);
				}
			}
		}
		else
		{
			listReadyAnimClip.Add(clip);
		}
	}
}

