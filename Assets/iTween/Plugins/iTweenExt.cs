using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public partial class iTween
{
	/// <summary>
	/// Changes a GameObject's position over time to a supplied destination with MINIMUM customization options.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/> to be the target of the animation.
	/// </param>
	/// <param name="position">
	/// A <see cref="Vector3"/> for the destination Vector3.
	/// </param>
	/// <param name="time">
	/// A <see cref="System.Single"/> for the time in seconds the animation will take to complete.
	/// </param>
	public static void MoveToPath(GameObject target, Vector3[] nodes, float time)
	{		
		MoveTo(target, nodes, time, EaseType.linear, true, null, null, null, null);
	}
	
	public static void MoveToPath(GameObject target, Vector3[] nodes, float time, 
		string updateFuncName, string complateFuncName, GameObject funcTarget, object param)
	{
		MoveTo(target, nodes, time, EaseType.linear, true, updateFuncName, complateFuncName, funcTarget, param);
	}
	
	public static void MoveToPath(GameObject target, Vector3[] nodes, float time, bool firstNode)
	{
		MoveTo(target, nodes, time, EaseType.linear, firstNode, null, null, null, null);
	}
	
	static void MoveTo(GameObject target, Vector3[] nodes, float time, EaseType easyType, bool firstNode, 
		string updateFuncName, string complateFuncName, GameObject funcTarget, object param)
	{
		if( null != target && 
			firstNode && 
			null != nodes && 
			nodes.Length > 0 )
		{
			target.transform.localPosition = nodes[0];
		}
		
		Hashtable args = iTween.Hash("path", nodes, "time", time, "easetype", easyType.ToString());
		if(null != updateFuncName) 
		{
			args["onupdate"] = updateFuncName;
			if(null != param)
			{
				args["onupdateparams"] = param;
			}
			if(null != funcTarget) args["onupdatetarget"] = funcTarget;
		}
		if(null != complateFuncName) 
		{
			args["oncomplete"] = complateFuncName;
			if(null != param)
			{
				args["oncompleteparams"] = param;
			}
			if(null != funcTarget) args["oncompletetarget"] = funcTarget;
		}
		
		iTween.MoveTo(target, args); 
	}
	
	
	/// <summary>
	/// Changes a GameObject's position over time to a supplied destination with MINIMUM customization options.
	/// </summary>
	/// <param name="target">
	/// A <see cref="GameObject"/> to be the target of the animation.
	/// </param>
	/// <param name="position">
	/// A <see cref="Vector3"/> for the destination Vector3.
	/// </param>
	/// <param name="time">
	/// A <see cref="System.Single"/> for the time in seconds the animation will take to complete.
	/// </param>
	public static void MoveToPathSpeed(GameObject target, Vector3[] nodes, float speed)
	{		
		MoveToPathSpeed(target, nodes, speed, EaseType.linear, true, null, null, null, null);
	}
	
	public static void MoveToPathSpeed(GameObject target, Vector3[] nodes, float speed, 
		string updateFuncName, string complateFuncName, GameObject funcTarget, object param)
	{
		MoveToPathSpeed(target, nodes, speed, EaseType.linear, true, updateFuncName, complateFuncName, funcTarget, param);
	}
	
	public static void MoveToPathSpeed(GameObject target, Vector3[] nodes, float speed, bool firstNode)
	{
		MoveToPathSpeed(target, nodes, speed, EaseType.linear, firstNode, null, null, null, null);
	}
	
	static void MoveToPathSpeed(GameObject target, Vector3[] nodes, float speed, EaseType easyType, bool firstNode, 
		string updateFuncName, string complateFuncName, GameObject funcTarget, object param)
	{
		if( null != target && 
			firstNode && 
			null != nodes && 
			nodes.Length > 0 )
		{
			target.transform.localPosition = nodes[0];
		}

		//"looptype", LoopType.loop
		Hashtable args = iTween.Hash("path", nodes, "speed", speed, "easetype", easyType.ToString(), "looptype", LoopType.loop);
		//args["looptype"] = LoopType.loop.ToString();
		if(null != updateFuncName) 
		{
			args["onupdate"] = updateFuncName;
			if(null != param)
			{
				args["onupdateparams"] = param;
			}
			if(null != funcTarget) args["onupdatetarget"] = funcTarget;
		}
		if(null != complateFuncName) 
		{
			args["oncomplete"] = complateFuncName;
			if(null != param)
			{
				args["oncompleteparams"] = param;
			}
			if(null != funcTarget) args["oncompletetarget"] = funcTarget;
		}
		
		iTween.MoveTo(target, args); 
	}
}
	
