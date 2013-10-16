@MenuItem("Assets/Auto Build Resource Files")
static function ExportResource () {

	System.IO.Directory.CreateDirectory("AssetBundles");

	var options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
	BuildPipeline.PushAssetDependencies();
	
	

	// All subsequent resources share assets in this resource file
	// It is up to you to ensure that the shared resource file is loaded prior to loading other resources
	BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/artwork/lerpzuv.tif"), null, "AssetBundles/Shared.unity3d", options);	

	// By pushing and popping around the resource file, this file will share resources but later resource files will not share assets in this resource
	BuildPipeline.PushAssetDependencies();
	
	BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/Artwork/Lerpz.fbx"), null, "AssetBundles/Lerpz.unity3d", options);	

	BuildPipeline.PopAssetDependencies();

	// By pushing and popping around the resource file, this file will share resources but later resource files will not share assets in this resource
	BuildPipeline.PushAssetDependencies();

	BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/Artwork/explosive guitex.prefab"), null, "AssetBundles/explosive.unity3d", options);	

	BuildPipeline.PopAssetDependencies();

	// By pushing and popping around the resource file, this file will share resources but later resource files will not share assets in this resource
	BuildPipeline.PushAssetDependencies();

	// Build streamed scene file into a seperate unity3d file
	BuildPipeline.BuildPlayer(["Assets/AdditiveScene.unity"], "AssetBundles/AdditiveScene.unity3d", BuildTarget.WebPlayer, BuildOptions.BuildAdditionalStreamedScenes);	

	BuildPipeline.PopAssetDependencies();
	
	BuildPipeline.PopAssetDependencies();
	
	BuildPipeline.BuildPlayer(["Assets/Loader.unity"], "loader.unity3d", BuildTarget.WebPlayer, BuildOptions.Development);
}