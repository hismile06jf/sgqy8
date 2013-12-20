using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

[ExecuteInEditMode]
public class SkillEditor : EditorWindow {
	
	static AnimationClip currClip;
	static Animation currSelectAnimation;

	[MenuItem("SkillEditor/PlayAnimation")]
	static void PlayAnim()
	{
		AnimationMode.StartAnimationMode();
		SkillEditor window = (SkillEditor)EditorWindow.GetWindow(typeof(SkillEditor));

		//AnimationMode.StartAnimationMode();
		GameObject obj = Selection.activeGameObject;
		if(null == obj) return;

		if(obj.animation != null && obj.animation.clip != null)
		{
			currSelectAnimation = obj.animation;
			currClip = currSelectAnimation.clip;

			currSelectAnimation[currClip.name].enabled = true;
		}
	}

	void Update ()
	{
		if(null != currClip && null != currSelectAnimation)
		{
			//currSelectAnimation[currClip.name].time = Time.time;
			//currSelectAnimation.Sample();

			AnimationMode.BeginSampling();
			AnimationMode.SampleAnimationClip(currSelectAnimation.gameObject, currClip, Time.realtimeSinceStartup);
			AnimationMode.EndSampling();
			//AnimationMode.StopAnimationMode();
		}
	}
}
