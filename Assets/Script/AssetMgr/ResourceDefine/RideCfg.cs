using System.Text;

public class RideCfg
{
	ERideType rideType;
	
	static public string GetRideAnimPath(ERideType type, string animName)
	{
		StringBuilder sb = new StringBuilder("file://D:/WorkSpace/sgqy8thunk/AssetBundle/Characters/animation/");
		switch(type)
		{
		case ERideType.Horse: sb.Append("horse"); break;
		case ERideType.Tiger: sb.Append("tiger"); break;
		case ERideType.Alpaca: sb.Append("alpaca"); break;
		case ERideType.Wolf: sb.Append("wolf"); break;
		default: sb.Append("horse"); break;
		}
		sb.Append("/");
		sb.Append(animName);
		sb.Append(".unity3d");
		return sb.ToString();
	}
}

