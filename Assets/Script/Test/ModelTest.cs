using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelTest : MonoBehaviour {
	
	//file://D:/WorkSpace/sgqy8thunk/AssetBundle/so0009.unity3d
	public string[] modelPath = new string[0];
	public Texture[] mainTex = new Texture[0];
	public Texture[] clothTex = new Texture[0];
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		Debug.Log("=====>>  persistentDataPath" + Application.persistentDataPath);
		Debug.Log("=====>>  temporaryCachePath" + Application.temporaryCachePath);
		Debug.Log("=====>>  absoluteURL" + Application.absoluteURL);
		Debug.Log("=====>>  streamingAssetsPath" + Application.streamingAssetsPath);
		Debug.Log("=====>>  dataPath" + Application.dataPath);
		
		ModelMgr.Instance.LoadModel(modelPath[0], OnAnimLoad, OnAninLoading, null);
	}
	
	float posX = -5f;
	int texIndex = 0;
	List<GameObject> objList = new List<GameObject>();
	void OnAnimLoad(string modelPath, Model m, object param)
	{
		if(null != m) 
		{
			GameObject objNew = m.gameObject;
			objNew.transform.localPosition = new Vector3(posX, 0, 0);
			objNew.transform.parent = null;
			
//			Color cor = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
//			SkinnedMeshRenderer[] rs = objNew.GetComponentsInChildren<SkinnedMeshRenderer>();
//			foreach(SkinnedMeshRenderer r in rs)
//			{
//				r.material.SetTexture("_MainTex", mainTex[texIndex]);
//				r.material.SetTexture("_ClothTex", clothTex[texIndex]);
//				r.material.SetColor("_ClothColor", cor);
//			}
			
			//objNew.AddComponent<CombineSkinnedMeshes>();
			
			posX += 1f;
			texIndex += 1;
			if(texIndex > 1)
			{
				texIndex = 0;
			}
			
			objList.Add(objNew);
		}
		
		//TimeMgr.Instance.Exec(DelModel, modelPath, 5000);
	}
	
	void DelModel(object param)
	{
		string parh = (string)param;
		for(int i = 0; i < objList.Count; ++i)
		{
			GameObject.DestroyImmediate(objList[i], true);
		}
		ModelMgr.Instance.UnLoadModel(parh);
	}
	
	void OnAninLoading(string modelPath, float progress)
	{
		
	}
}
