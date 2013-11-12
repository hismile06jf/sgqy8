using System.Collections.Generic;


public class MtrlTex
{
	public string Name;
	public string FilePath;
	
	public MtrlTex(string name, string path)
	{
		Name = name;
		FilePath = path;
	}
}

public class ModelMtrl
{
	public string Name;
	public List<MtrlTex> TexList;
	
	public int TexCount
	{
		get { return null == TexList ? 0 : TexList.Count; }
	}
	
	
	//
	public ModelMtrl(string name)
	{
		Name = name;
		TexList = new List<MtrlTex>();
		TexList.Add(new MtrlTex("_MainTex", "file://D:/WorkSpace/sgqy8thunk/AssetBundle/Textures/so0009.unity3d"));
		TexList.Add(new MtrlTex("_ClothTex", "file://D:/WorkSpace/sgqy8thunk/AssetBundle/Textures/so0009_cloth.unity3d"));
	}
}

public class ModelCfg
{
	public int Id;
	public string FilePath;
	public List<ModelMtrl> MtrlList;
	
	public int MtrlCount
	{
		get { return null == MtrlList ? 0 : MtrlList.Count; }
	}
	
	public ModelCfg()
	{
		MtrlList = new List<ModelMtrl>();
		MtrlList.Add(new ModelMtrl("so0009"));
	}
}

