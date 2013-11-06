using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeMgr : MonoBehaviour {
	
	static TimeMgr mInstance;
	static public TimeMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				GameObject obj = new GameObject("GameLogic/TimeEvent/TimeMgr");
				DontDestroyOnLoad(obj);
				mInstance = obj.AddComponent<TimeMgr>();
			}
			return mInstance;
		}
	}
	
	class FuncParam
	{
		public int timeMS;
		public object objParam;
		
		public FuncParam(int time, object param)
		{
			timeMS = time;
			objParam = param;
		}
	}
	
	class VoidTimeParam
	{
		public int timeMS;
		public VoidDelegate function;
		
		public VoidTimeParam(int time, VoidDelegate func)
		{
			timeMS = time;
			function = func;
		}
	}
	
	class ParamTimeParam : FuncParam
	{
		public ParamDelegate function;
		
		public ParamTimeParam(int time, object param, ParamDelegate func)
			: base(time, param)
		{
			function = func;
		}
	}
	
	public delegate void VoidDelegate();
	public delegate void ParamDelegate(object param);
	
	List<VoidTimeParam>						listVoidFunc = new List<VoidTimeParam>();
	List<ParamTimeParam>					listParamFunc = new List<ParamTimeParam>();
	
	Dictionary<VoidDelegate, VoidTimeParam>	dictVoidFunc = new Dictionary<VoidDelegate, VoidTimeParam>();
	Dictionary<ParamDelegate, FuncParam>	dictParamFunc = new Dictionary<ParamDelegate, FuncParam>();
	
	public void Exec(VoidDelegate func, int timeMS)
	{
		listVoidFunc.Add(new VoidTimeParam(timeMS, func));
	}
	
	public void Exec(ParamDelegate func, object param, int timeMS)
	{
		listParamFunc.Add(new ParamTimeParam(timeMS, param, func));
	}
	
	public void PushFunc(VoidDelegate func, int timeMS)
	{
		if(dictVoidFunc.ContainsKey(func)) 
		{
			Debug.LogWarning("void func is already exist...");
			return;
		}
		
		dictVoidFunc.Add(func, new VoidTimeParam(timeMS, func));
	}
	
	public void PushFunc(ParamDelegate func, object param, int timeMS)
	{
		if(dictParamFunc.ContainsKey(func)) 
		{
			Debug.LogWarning("param func is already exist...");
			return;
		}
		
		dictParamFunc.Add(func, new FuncParam(timeMS, param));
	}
	
	public void PopFunc(VoidDelegate func)
	{
		if(!dictVoidFunc.ContainsKey(func)) return;
		
		dictVoidFunc.Remove(func);
	}

	public void PopFunc(ParamDelegate func)
	{
		if(!dictParamFunc.ContainsKey(func)) return;
		
		dictParamFunc.Remove(func);
	}
	
	List<VoidDelegate> listVoidClear = new List<VoidDelegate>();
	List<ParamDelegate> listParamClear = new List<ParamDelegate>();
	// Update is called once per frame
	void Update () {
		int delta = (int)(Time.fixedTime * 1000f);
		
		//
		for(int i = listVoidFunc.Count - 1; i >= 0; --i)
		{
			VoidTimeParam p = listVoidFunc[i];
			p.timeMS -= delta;
			if(p.timeMS <= 0)
			{
				if(null != p.function) p.function();
				listVoidFunc.Remove(p);
			}
		}
		
		//
		for(int i = listParamFunc.Count - 1; i >= 0; --i)
		{
			ParamTimeParam p = listParamFunc[i];
			p.timeMS -= delta;
			if(p.timeMS <= 0)
			{
				if(null != p.function) p.function(p.objParam);
				listParamFunc.Remove(p);
			}
		}
		
		//
		foreach(KeyValuePair<VoidDelegate,VoidTimeParam> kv in dictVoidFunc)
		{
			kv.Value.timeMS -= delta;
			if(kv.Value.timeMS <= 0)
			{
				if(null != kv.Key) kv.Key();
				listVoidClear.Add(kv.Key);
			}
		}
		for(int i = 0; i < listVoidClear.Count; ++i)
		{			
			dictVoidFunc.Remove(listVoidClear[i]);
		}
		listVoidClear.Clear();
		
		//
		foreach(KeyValuePair<ParamDelegate,FuncParam> kv in dictParamFunc)
		{
			kv.Value.timeMS -= delta;
			if(kv.Value.timeMS <= 0)
			{
				if(null != kv.Key) kv.Key(kv.Value.objParam);
				listParamClear.Add(kv.Key);
			}
		}
		for(int i = 0; i < listParamClear.Count; ++i)
		{
			dictParamFunc.Remove(listParamClear[i]);
		}
		listParamClear.Clear();
	}
	
	
}
