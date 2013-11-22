using UnityEngine;
using System.Collections;

public class ActionTest : MonoBehaviour {
	
	public AnimationClip clip1 = null;
	public AnimationClip clip2 = null;
	void Start()
	{
		if(null != animation)
		{
			animation.AddClip(clip1, clip1.name);
			animation.AddClip(clip2, clip1.name);
			
			animation.wrapMode = WrapMode.Loop;
			//animation.clip = clip1;
			
			AnimationEvent evt = new AnimationEvent();
			evt.time = 1f;
			evt.functionName = "OnAnimationEvent";
			evt.messageOptions = SendMessageOptions.DontRequireReceiver;
			animation[clip1.name].clip.AddEvent(evt);
			animation.Play(clip1.name);
		}
	}
	
	bool addevent = false;
	void OnAnimationEvent(AnimationEvent evt)
	{
		Debug.Log("=============>>  OnAnimationEvent, curr Time = " + Time.realtimeSinceStartup.ToString());
		animation.Stop(clip1.name);
		//animation.Play(clip2.name);
		
		if(!addevent)
		{
			AnimationEvent evvt = new AnimationEvent();
			evvt.time = 3f;
			evvt.functionName = "OnAnimationEvent";
			evvt.messageOptions = SendMessageOptions.DontRequireReceiver;
			animation[clip1.name].clip.AddEvent(evt);
			animation.Play(clip1.name);
			addevent = true;
		}
		
	}
}

