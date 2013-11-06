using UnityEngine;
using System.Collections;

public class ModelTest : MonoBehaviour {
	
	//file://D:/WorkSpace/sgqy8thunk/AssetBundle/so0009.unity3d
	public string[] modelPath = new string[0];
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		ModelMgr.Instance.LoadModel(modelPath[0], OnAnimLoad, OnAninLoading);
	}
	
	void OnAnimLoad(string modelPath, GameObject model)
	{
		if(null != model) 
		{
			GameObject objNew = GameObject.Instantiate(model) as GameObject;
			objNew.transform.localPosition = Vector3.zero;
			objNew.transform.parent = null;
		}
		
		TimeMgr.Instance.Exec(DelModel, modelPath, 10000);
	}
	
	void DelModel(object param)
	{
		string parh = (string)param;
		ModelMgr.Instance.UnLoadModel(parh);
	}
	
	void OnAninLoading(string modelPath, float progress)
	{
		
	}
}
