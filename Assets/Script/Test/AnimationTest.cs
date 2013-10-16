using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {
	
	int lastIndex = 0;
	int loadIndex = 0;
	string lastName = "";
	public string[] animPath = new string[0];
	
	public GameObject objM;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		objM.animation.RemoveClip(lastName);
		AnimationMgr.Instance.UnLoadAnimation(animPath[lastIndex]);
		if(loadIndex >= animPath.Length) loadIndex = 0;
		AnimationMgr.Instance.LoadAnimation(animPath[loadIndex], OnAnimLoad, OnAninLoading);
		lastIndex = loadIndex;
		loadIndex++;
	}
	
	void OnAnimLoad(string animPath, AnimationClip clip)
	{
		objM.animation.AddClip(clip, clip.name);
		objM.animation.wrapMode = WrapMode.Loop;
		objM.animation.Play(clip.name);
		lastName = clip.name;
	}
	
	void OnAninLoading(string animPath, float progress)
	{
		
	}
}
