using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleLoad : MonoBehaviour {
	private static List<Object> m_InstaniateObj = new List<Object>();
	private static AssetBundleManifest m_MAINFEST = null;
	private static string m_OUTPATH=Application.streamingAssetsPath+ "/AssetBundle/";
	private static string m_FILENAME="AssetBundle";

	private static AssetBundle LoadAB(string _ABname)
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

			AssetBundle temp= AssetBundle.LoadFromFile(m_OUTPATH + _ABname+".assetbundle");
			return temp;
        }
        return null;
    }
	/// <summary>
	///_AssetName=null或空时，实例化该AB包所有资源
	/// </summary>
	/// <returns>The A.</returns>
	/// <param name="_ABname">A bname.</param>
	/// <param name="_AssetName">Asset name.</param>
	public static void InstaniateAB(string _ABname,string _AssetName=null)
	{
		
		AssetBundle m_tempAB=LoadAB (_ABname);
		if (string.IsNullOrEmpty(_AssetName))
		{
			
			for (int index = 0; index < m_tempAB.LoadAllAssets().Length; index++) 
			{
				m_InstaniateObj.Add (GameObject.Instantiate(m_tempAB.LoadAllAssets()[index]));
			}
		}
		else
			m_InstaniateObj.Add (GameObject.Instantiate (m_tempAB.LoadAsset(_AssetName)));
		
		m_tempAB.Unload(false);
		Debug.Log ("生成AB对象并卸载AB资源完毕");
	}
	/// <summary>
	/// 移除当前所有场景内AB资源
	/// </summary>
	public static void RemoveABSource()
	{
		for (int index = 0; index < m_InstaniateObj.Count; index++) 
		{
			Destroy (m_InstaniateObj[index]);
		}
		Debug.Log ("移除AB对象完毕");
	}
}