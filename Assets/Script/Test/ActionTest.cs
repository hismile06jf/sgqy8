using UnityEngine;
using System.Collections;

public class ActionTest : MonoBehaviour {

	public GameObject obj;
	
	void Start()
	{
		if(null != obj && obj.animation)
		{
			obj.animation.wrapMode = WrapMode.Loop;
		}
	}
}

