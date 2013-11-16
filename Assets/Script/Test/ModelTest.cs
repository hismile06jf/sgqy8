using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelTest : MonoBehaviour {
	
	//file://D:/WorkSpace/sgqy8thunk/AssetBundle/so0009.unity3d
	public string[] modelPath = new string[0];
	public Texture[] mainTex = new Texture[0];
	public Texture[] clothTex = new Texture[0];
	
	Role killer = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{		
		//ModelMgr.Instance.LoadModel(modelPath[0], OnAnimLoad, OnAninLoading, null);
		killer = new Role(9);
		killer.AddAttach(88, "HP_right_hand");
		killer.LoadAnim("file://D:/WorkSpace/sgqy8thunk/AssetBundle/w_a01.unity3d");
		killer.PlayAnim("w_a01");
	}
}
