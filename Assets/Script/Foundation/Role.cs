using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	Model mainBody;
	
	public Model MainBody
	{
		get { return mainBody; } 
	}
	
	public GameObject MainBodyObj
	{
		get { return null == mainBody ? null : mainBody.gameObject; }
	}
	
	public void SetPostion()
	{
	}
	
	public void SetScale()
	{
	}
	
	public void SetRotation()
	{
		//
	}
	
	public void MoveTo(float speed)
	{
	}
	
	public void MovePath(float speed)
	{
	}
}