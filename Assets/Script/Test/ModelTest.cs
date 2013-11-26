using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelTest : MonoBehaviour {
	
	//file://D:/WorkSpace/sgqy8thunk/AssetBundle/so0009.unity3d
	public string[] modelPath = new string[0];
	public Texture[] mainTex = new Texture[0];
	public Texture[] clothTex = new Texture[0];
	
	Role killer = null;
	List<Role> roleList = new List<Role>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{		
		if(killer == null)
		{
			//ModelMgr.Instance.LoadModel(modelPath[0], OnAnimLoad, OnAninLoading, null);
			killer = new Role(9);
			killer.AddAttach(88, Role.GetHardPointName(EHardPoint.LeftHand));
			//killer.LoadAnim("file://D:/WorkSpace/sgqy8thunk/AssetBundle/r_a02.unity3d");
			killer.PlayAnim("r_a02");
			killer.MountRide(111);
		}
		else
		{
			float time = Random.Range(1f, 5f);
			killer.UseSkill(1, time);
			//Debug.Log("time = " + time.ToString());
		}
		//roleList.Add(killer);
	}
	
	void OnDestroy()
	{
		for(int i = 0; i < roleList.Count; ++i)
		{
			roleList[i] = null;
		}
		roleList.Clear();
	}
}
