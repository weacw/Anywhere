using UnityEngine;
using System.Collections;
using Anywhere;
using System.Threading;
using System.Net;
using System.IO;
using System;
using System.Text;
using Aliyun.OSS;
public class GetInfo : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{

//		GameObject.	Instantiate(AssetBundleLoad.LoadAB("test").LoadAsset<GameObject>("Assets/Artwork/Effects/01.prefab"));
		GetObject.AsyncGetObject ("anywhere-v-1","assets.assetbundle");
		//UploadObject.UploadFile (Application.streamingAssetsPath+"/AssetBundle/assets.assetbundle","anywhere-v-1","assets.assetbundle");

	}

	void Update ()
	{
		// if(Input.GetKeyDown(KeyCode.A))
		// GetObject.AsyncGetObject ("anywhere-v-1","assets.assetbundle");
	}

	public static string HttpGet (string _search)
	{
		HttpWebRequest request = WebRequest.Create ("https://weacw.com/anywhere/searchinfo.php?search=" + _search) as HttpWebRequest;  
		request.Method = "GET";  
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		Stream myResponseStream = response.GetResponseStream ();
		StreamReader myStreamReader = new StreamReader (myResponseStream, Encoding.GetEncoding ("utf-8"));
		string retString = myStreamReader.ReadToEnd ();
		myStreamReader.Close ();
		myResponseStream.Close ();
		return retString;
	}

	public static string HttpGet (int _page)
	{

		HttpWebRequest request = WebRequest.Create ("https://weacw.com/anywhere/getinfo.php?page=" + _page) as HttpWebRequest;
		request.Method = "GET";  
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		Stream myResponseStream = response.GetResponseStream ();
		StreamReader myStreamReader = new StreamReader (myResponseStream, Encoding.GetEncoding ("utf-8"));
		string retString = myStreamReader.ReadToEnd ();
		myStreamReader.Close ();
		myResponseStream.Close ();
		return retString;
	}
}
