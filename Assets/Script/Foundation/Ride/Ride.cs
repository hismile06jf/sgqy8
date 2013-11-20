
public delegate void RideLoadCallBack();

public class Ride : Role
{
	Model master = null;
	ERideType rideType = ERideType.Horse;
	
	public Ride(int modelId, Model master) : base(modelId)
	{
		this.master = master;
	}
	
//	~Ride()
//	{
//		UnMount();
//	}
	
	public ERideType RideType
	{
		get { return rideType; }
	}
	
	public void Mount(Model m)
	{
		if(null == m) return;
		master = m;
		AttachToHP(m.gameObject, Role.GetHardPointName(EHardPoint.Horse));
	}
	
	public void UnMount(Model m)
	{
		if(null == m) return;
		
		DetachFromHP(m.gameObject, Role.GetHardPointName(EHardPoint.Horse));
	}
	
	public void UnMount()
	{
		UnMount(master);
		master = null;
	}
	
	//
	override public void OnMainBodyReady()
	{
		Mount(master);
	}
	
	//
	override public string GetAnimName(EAnimType type)
	{
		switch(type)
		{
		case EAnimType.Idle: return "w_w01";
			
		case EAnimType.Rest_Down: return "h_r01_1";
		case EAnimType.Rest_Idle: return "h_r01_2";
		case EAnimType.Rest_Idle_a: return "h_r01_3";
		case EAnimType.Rest_Up: return "h_r02";
			
		case EAnimType.Walk_Fore: return "h_m01";
		case EAnimType.Walk_Back: return "h_m02";
		case EAnimType.Walk_TurnLeft: return "h_m05";
		case EAnimType.Walk_TurnRight: return "h_m06";
		case EAnimType.Walk_Die: return "h_f02_1";
		case EAnimType.Walk_DieHold: return "h_f02_2";
			
		case EAnimType.Run_Fore: return "h_m03";
		case EAnimType.Run_Back: return "h_m02";
		case EAnimType.Run_RurnLeft: return "h_m05";
		case EAnimType.Run_RurnRight: return "h_m06";
		case EAnimType.Run_Die: return "h_f02_1";
		case EAnimType.Run_DieHold: return "h_f02_2";
		}
		
		return "w_w01";
	}
	
	override public string GetAnimPath(string animName)
	{
		return RideCfg.GetRideAnimPath(RideType, animName);
	}
}
