using UnityEngine;
using UnityEditor;
using System.IO;
public class NewAssetBundleEditor : Editor {



	[MenuItem ("AB Editor/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
		if (Directory.Exists (AssetBundleConfig.outpath) == false)
			Directory.CreateDirectory (AssetBundleConfig.outpath);
		Caching.ClearCache ();
        BuildPipeline.BuildAssetBundles(AssetBundleConfig.outpath.Substring(AssetBundleConfig.projectpath.Length), BuildAssetBundleOptions.UncompressedAssetBundle |
                                          BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.DeterministicAssetBundle|BuildAssetBundleOptions.None, BuildTarget.iOS);
		AssetDatabase.Refresh ();
	}

  

}
