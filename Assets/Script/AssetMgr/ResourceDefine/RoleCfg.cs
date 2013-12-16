using System.Text;

public class RoleCfg
{	
	static public string GetRoleAnimPath(ERoleType type, string animName)
	{
		StringBuilder sb = new StringBuilder(AssetPath.GetAssetStorePathWithSlash() + "Characters/animation/");
		switch(type)
		{
		case ERoleType.UserL: sb.Append("user_l"); break;
		case ERoleType.UserM: sb.Append("user_m"); break;
		case ERoleType.UserX: sb.Append("user_x"); break;
		default: sb.Append("user_x"); break;
		}
		sb.Append("/");
		sb.Append(animName);
		sb.Append(".unity3d");
		return sb.ToString();
	}
}
