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

		ParticleSystem[] pars = gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach(ParticleSystem par in pars)
		{
			par.Play(true);
		}
	}
	
	Vector3 lastPos = Vector3.zero;
	void OnPathUpdate()
	{
		Vector3 dir = gameObject.transform.localPosition - lastPos;
		if(dir == Vector3.zero) return;

		gameObject.transform.localRotation = Quaternion.LookRotation(dir);
		
		lastPos = gameObject.transform.localPosition;
	}
}

