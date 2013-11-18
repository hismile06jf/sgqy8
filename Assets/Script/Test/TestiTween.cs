using UnityEngine;
using System.Collections;

public class TestiTween : MonoBehaviour
{
	void Start()
	{
		iTweenPath path = gameObject.GetComponent<iTweenPath>();
		if(path != null)
		{
			iTween.MoveTo(gameObject, iTween.Hash("path", path.nodes.ToArray(), "time", 10f, "easetype", "linear")); 
		}
	}
	
}

