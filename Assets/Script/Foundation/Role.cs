using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	Model mainBody;
	List<AttachObject> listAttachObject = new List<AttachObject>();
	
	public Model MainBody
	{
		get { return mainBody; } 
	}
	
	public GameObject MainBodyObj
	{
		get { return null == mainBody ? null : mainBody.gameObject; }
	}
	
	public void PlayAnim(string animPath)
	{
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