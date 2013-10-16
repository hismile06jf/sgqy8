using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
	LinkedList<WWW> m_listResLoadQueue = new LinkedList<WWW>();
	
	public void LoadAsset(string resPath, AssetLoadCallBack cb)
	{
	}
	
	void Update()
	{
	}
}
