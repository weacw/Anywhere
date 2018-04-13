using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleLoad : MonoBehaviour {

	private static AssetBundleManifest manifest = null;

	private static string outpath=Application.streamingAssetsPath+ "/AssetBundle/";
	private static string filename="AssetBundle";
	public static AssetBundle LoadAB(string abPath)
    {
     
        if (manifest == null)
        {
			AssetBundle manifestBundle = AssetBundle.LoadFromFile (outpath + filename);
            manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
        }
        if (manifest != null)
        { 
            string[] cubedepends = manifest.GetAllDependencies(abPath);

            for (int index = 0; index < cubedepends.Length; index++)
            {
                LoadAB(cubedepends[index]);
            }
			AssetBundle temp= AssetBundle.LoadFromFile(outpath + abPath);

			return temp;
        }
        return null;
    }


}