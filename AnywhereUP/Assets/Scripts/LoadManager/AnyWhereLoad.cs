using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using Aliyun.OSS;
using System.IO;
/// <summary>
///加载视频,图片，AB资源类; 
/// </summary>
public class AnyWhereLoad : MonoBehaviour {
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
	public static Texture LoadTexture(string _texturename,int _t2dwith,int _t2dheight)
	{
		
		byte[] m_T2dbyts= File .ReadAllBytes(Config.DirToDownload+"/Download/Texture/"+_texturename+".png");
		Texture2D m_T2d=new Texture2D(_t2dwith,_t2dheight);
		m_T2d.LoadImage (m_T2dbyts);
		return m_T2d;

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
	/// <summary>
	///播放路径下的视频； 
	/// </summary>
	/// <param name="_videoimage">播放视频到指定rawimage.</param>
	/// <param name="_videopath">视频名称.</param>
	/// <param name="_isloop">视频是否循环.</param>
	public static void PlayVideo(GameObject _videorender,string _videoname,bool _isloop=false)
	{
		VideoPlayer m_Vp = _videorender.GetComponent<VideoPlayer> ();
		if (m_Vp != null) {
			if (m_Vp.isPlaying)
				m_Vp.Stop ();
		} else
			m_Vp = _videorender.AddComponent<VideoPlayer> ();
		m_Vp.source = VideoSource.Url;
		m_Vp.url ="file:///"+ Config.DirToDownload + "/Download/Video/" + _videoname + ".mp4";
		m_Vp.renderMode = VideoRenderMode.MaterialOverride;
		m_Vp.targetMaterialRenderer = _videorender.GetComponent<Renderer>();
		_videorender.GetComponent<Renderer> ().material.shader = Shader.Find ("Custom/Video360");
		m_Vp.Play ();
		m_Vp.isLooping = _isloop;
	}
}