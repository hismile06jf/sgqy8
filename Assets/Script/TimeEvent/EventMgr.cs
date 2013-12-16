using UnityEngine;
using System.Collections;

public class EventMgr : MonoBehaviour {

	static EventMgr mInstance;
	static public EventMgr Instance
	{
		get {
			if(null == mInstance) 
			{
				mInstance = LogicRoot.Instance.GetSingletonObjectScript<EventMgr>("AssetMgr", "EventMgr");
			}
			return mInstance;
		}
	}


}
