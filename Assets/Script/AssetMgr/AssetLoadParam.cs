using System;
using UnityEngine;
using System.Collections.Generic;

public delegate void AssetLoadCallBack(string resPath, WWW www);

public class AssetLoadParam
{
	public AssetLoadParam ()
	{
	}
}

public class ResLoadParam<T>
{
	public object userParam;
	public ResParamLoadCallBack<T> _cb;
	public ResLoadProgressCallBack _pcb;

	public ResLoadParam(object param, ResParamLoadCallBack<T> loadCB, ResLoadProgressCallBack progressCB)
	{
		userParam = param;
		_cb = loadCB;
		_pcb = progressCB;
	}

	public void DispatchProgress(string resPath, float progress)
	{
		if(null != _pcb) 
		{
			_pcb(resPath, progress, userParam);
		}
	}

	public void DispatchLoadCallBack(string resPath, T res)
	{
		if(null != _cb) 
		{
			_cb(resPath, res, userParam);
		}
	}
}

public delegate void ResLoadProgressCallBack(string resPath, float progress, object userParam);
public delegate void ResParamLoadCallBack<T>(string resPath, T res, object userParam);


public class ResCounter<T>
{
	int _ref;
	T _res;
	WWW _assetWWW;
	AssetBundle _assetBundle;
	string _resPath;
	LinkedList<ResLoadParam<T>> listResLoadParam = new LinkedList<ResLoadParam<T>>();
	
	public ResCounter()
	{
	}
	
	public void AddRef()
	{
		++_ref;
	}
	
	public void Release()
	{
		--_ref;
	}
	
	public int Ref
	{
		get { return _ref; }
	}
	
	public T Res
	{
		get { return _res; }
		set { _res = value; }
	}
	
	public WWW AssetWWW
	{
		get { return _assetWWW; }
		set { _assetWWW = value; }
	}
	
	public AssetBundle Bundle
	{
		get { return _assetBundle; }
		set { _assetBundle = value; }
	}
	
	public string ResPath
	{
		get { return _resPath; }
		set { _resPath = value; }
	}

	public void AddLoadParam(object userParam, ResParamLoadCallBack<T> loadCB, ResLoadProgressCallBack progressCB)
	{
		listResLoadParam.AddLast(new ResLoadParam<T>(userParam, loadCB, progressCB));
	}

	public void ClearLoadParam()
	{
		listResLoadParam.Clear();
	}

	public void DispatchProgress(string resPath, float progress)
	{
		foreach(ResLoadParam<T> param in listResLoadParam)
		{
			if(null == param) continue;
			param.DispatchProgress(resPath, progress);
		}
	}
		
	public void DispatchLoaCallBack()
	{
		foreach(ResLoadParam<T> param in listResLoadParam)
		{
			if(null == param) continue;
			param.DispatchLoadCallBack(ResPath, Res);
		}
	}
}
