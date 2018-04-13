using UnityEngine;
using System.Collections;

public class AssetBundleConfig : MonoBehaviour {


	public static string outpath=Application.streamingAssetsPath+ "/AssetBundle/";
	public static string filename="AssetBundle";
    public static string respath = Application.dataPath + "/";
	public static string projectpath = outpath.Substring(0, respath.Length - 7);
	public static string houzui = ".AssetBundle";

}
