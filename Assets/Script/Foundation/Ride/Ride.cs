
public delegate void RideLoadCallBack();

public class Ride : Role
{
	Model master = null;
	
	public Ride(int modelId, Model master) : base(modelId)
	{
		this.master = master;
	}
	
	~Ride()
	{
		UnMount(master);
	}
	
	public void Mount(Model m)
	{
		if(null == m) return;
		
		AttachToHP(m.gameObject, Role.GetHardPointName(EHardPoint.Horse));
	}
	
	public void UnMount(Model m)
	{
		if(null == m) return;
		
		DetachFromHP(m.gameObject, Role.GetHardPointName(EHardPoint.Horse));
	}
	
	//
	override public void OnMainBodyReady()
	{
		Mount(master);
	}
}
