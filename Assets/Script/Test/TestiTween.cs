using UnityEngine;
using System.Collections;

public class TestiTween : MonoBehaviour
{
	public float speed = 100f;
	void Start()
	{
		iTweenPath path = gameObject.GetComponent<iTweenPath>();
		if(path != null)
		{
			iTween.MoveToPathSpeed(gameObject, path.nodes.ToArray(), speed, "OnPathUpdate", null, null, null);
		}
	}
	
	Vector3 lastPos = Vector3.zero;
	void OnPathUpdate()
	{
		Vector3 dir = gameObject.transform.localPosition - lastPos;
		gameObject.transform.localRotation = Quaternion.LookRotation(dir);
		
		lastPos = gameObject.transform.localPosition;
	}
}

