using UnityEngine;
using UnityEditor;

public class ModelPostprocessor : AssetPostprocessor
{
	void OnPreprocessModel ()
	{
		ModelImporter importer = assetImporter as ModelImporter;
		importer.globalScale = 1f;
		importer.animationType = ModelImporterAnimationType.Legacy;
		
	}
	
	void OnPostprocessModel(GameObject go)
	{
		MeshRenderer[] meshComponents = go.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinMeshComponents = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		bool isAnim = false;
		if(meshComponents.Length == 0 && skinMeshComponents.Length == 0)
		{
			isAnim = true;
		}
		
		ModelImporter importer = assetImporter as ModelImporter;
		if(isAnim)
		{
			importer.optimizeMesh = false;
			importer.importMaterials = false;
			importer.normalImportMode = ModelImporterTangentSpaceMode.None;
			importer.tangentImportMode = ModelImporterTangentSpaceMode.None;
			importer.splitTangentsAcrossSeams = false;
		}
		
		//
		importer.importAnimation = isAnim;
		
		
		go.tag = isAnim ? EAssetType.Anim.ToString() : EAssetType.Model.ToString();
	}
}
