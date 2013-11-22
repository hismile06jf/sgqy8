using UnityEngine;
using System.Collections.Generic;

public partial class Role
{
	Model mainBody;
	
	int rideModelId;
	Ride ride;
	
	ERoleState currState;
	
//	~Role()
//	{
//		UnMountRide();
//	}
	
	public Model MainBody
	{
		get { return mainBody; }
	}
	
	public GameObject MainBodyObj
	{
		get { return null == mainBody ? null : mainBody.gameObject; }
	}
	
	public bool isRide
	{
		get { return null != ride; }
	}
	
	public void MountRide(int modelId)
	{
		ride = null;
		
		rideModelId = modelId;
		if(IsMainBodyReady)
		{
			ride = new Ride(modelId, MainBody);
			//ride.PlayAnim(EAnimType.Walk_Fore);
			rideModelId = 0;
		}
	}
	
	public void UnMountRide()
	{
		if(null != ride)
		{
			ride.UnMount();
		}
		ride = null;
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
	
//	void SwitchState(ERoleState state)
//	{
//		switch(currState)
//		{
//		case ERoleState.Attack:
//			break;
//		}
//	}
}