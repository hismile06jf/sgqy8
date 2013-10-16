var download : WWW;
var url = "packed_resource.unity3d";
var resourcePath = "LoadScene";
var guiOffset = 20;
var assetBundle : AssetBundle;

function StartDownload () {
	// Use fancy logic to get the actual path
	if (url.IndexOf ("file://") == 0 || url.IndexOf ("http://") == 0)
		download = new WWW (url);
	if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
		download = new WWW ("../AssetBundles/" + url);
	else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		download = new WWW ("file://" + Application.dataPath + "/../AssetBundles/" + url);
	
	yield download;
	
	assetBundle = download.assetBundle;
	
	Application.LoadLevelAdditive("AdditiveScene");
}

function OnGUI()
{
	GUILayout.Space(guiOffset);
	GUILayout.BeginHorizontal();
	if (download == null)
	{
		
		if (GUILayout.Button("Download " + url))
			StartDownload();	
	}
	else
	{
		if (download.error == null)
		{
			var progress = parseInt(download.progress * 100);
			GUILayout.Label(progress + "%");	
			
			if (download.isDone && GUILayout.Button("Unload Resource")	)
			{
				download.Dispose();
				download = null;
				assetBundle.Unload (true);
				assetBundle = null;
			}
		}
		else
		{
			GUILayout.Label(download.error);		
		}	
	}
	
	GUILayout.EndHorizontal();
}