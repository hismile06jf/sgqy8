using System;
using UnityEngine;

public delegate void AssetLoadCallBack(string resPath, WWW www);

public class AssetLoadParam
{
	public AssetLoadParam ()
	{
	}
}

public delegate void ResLoadProgressCallBack(string resPath, float progress);
public delegate void ResLoadCallBack<T>(string resPath, T res);


public class ResCounter<T>
{
	int _ref;
	T _res;
	ResLoadCallBack<T> _cb;
	ResLoadProgressCallBack _pcb;
	WWW _assetWWW;
	AssetBundle _assetBundle;
	string _resPath;
	
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
	
	public ResLoadCallBack<T> LoadCallBack
	{
		get { return _cb; }
		set { _cb = value; }
	}
	
	public ResLoadProgressCallBack ProgressCallBack
	{
		get { return _pcb; }
		set { _pcb = value; }
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
		
}
