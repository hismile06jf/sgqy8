using UnityEngine;
using System.Collections;

public class FxTracker : MonoBehaviour {

	float time;
	public float threshold = 1f;
	public float speed;
	Transform cache;
	public Transform target;

	public object msgParam;
	public string msgFuncName;
	public Transform msgReciver;

	MathUtl.CRSpline crSpline;

	public bool isComplete
	{
		get 
		{
			if(null == target) return false;

			Vector3 dis = cache.position - target.position;
			return dis.sqrMagnitude <= (threshold * threshold);
		}
	}

	public Transform Target
	{
		get { return target; }
		set { target = value; ResetTarget(); }
	}

	void ResetTarget()
	{
		crSpline = null;
		if(null != target)
		{
			Vector3 dir = target.position - cache.position;
			float len = dir.magnitude;
			Vector3 pos1 = cache.position + dir.normalized * len * 0.3f;
			Vector3 pos2 = cache.position + dir.normalized * len * 0.6f;
			crSpline = new MathUtl.CRSpline(target.position, pos2, pos1, cache.position);

			time = len / speed;
		}
	}

	// Use this for initialization
	void Start () {	
		cache = gameObject.transform;
		ResetTarget();
	}
	
	// Update is called once per frame
	void Update () {
		if(null != crSpline && null != target)
		{
			Vector3 dir = target.position - cache.position;
			float len = dir.magnitude;
			Vector3 normal = dir.normalized;

			Vector3 vNewPos;
			Vector3 move = normal * speed * Time.deltaTime;
			float moveLen = move.magnitude;
			if(moveLen >= len)
			{
				vNewPos = target.position;

				if(null != msgReciver && string.IsNullOrEmpty(msgFuncName))
				{
					msgReciver.SendMessage(msgFuncName, msgParam, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				//crSpline.pts[3] = cache.position;
				//crSpline.pts[2] = cache.position + normal * len * 0.3f;
				//crSpline.pts[1] = cache.position + normal * len * 0.6f;
				//crSpline.pts[0] = target.position;
				//vNewPos = crSpline.Interp(moveLen / len);
				vNewPos = cache.position + move;
			}

			cache.position = vNewPos;
		}
	}
}
