using System.Collections.Generic;


public class MtrlTex
{
	public string Name;
	public string FilePath;
}

public class ModelMtrl
{
	public string Name;
	public List<MtrlTex> TexList;
}

public class ModelCfg
{
	public int Id;
	public string FilePath;
	public List<ModelMtrl> MtrlList;
}

