using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleLoad : MonoBehaviour {

	private static AssetBundleManifest m_MAINFEST = null;
	private static string m_OUTPATH=Application.streamingAssetsPath+ "/AssetBundle/";
	private static string m_FILENAME="AssetBundle";
	public static AssetBundle LoadAB(string _ABname)
    {
     
		if (m_MAINFEST == null)
        {
			AssetBundle manifestBundle = AssetBundle.LoadFromFile (m_OUTPATH + m_FILENAME);
			m_MAINFEST = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
        }
		if (m_MAINFEST != null)
        { 
			string[] cubedepends = m_MAINFEST.GetAllDependencies(_ABname);

            for (int index = 0; index < cubedepends.Length; index++)
            {
                LoadAB(cubedepends[index]);
            }
			AssetBundle temp= AssetBundle.LoadFromFile(m_OUTPATH + _ABname);
			return temp;
        }
        return null;
    }


}