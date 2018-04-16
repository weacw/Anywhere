using UnityEngine;
using System.Collections;
using Anywhere;
using System.Threading;
using System.Net;
using System.IO;
using System;
using System.Text;

public class GetInfo : MonoBehaviour
{
	public static GetInfo instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
//		GameObject.	Instantiate(AssetBundleLoad.LoadAB("test").LoadAsset<GameObject>("Assets/Artwork/Effects/01.prefab"));
		//GetObject.AsyncGetObject ("anywhere-v-1","hs.mp4");
		//UploadObject.UploadFile (Application.streamingAssetsPath+"/AssetBundle/assets.assetbundle","anywhere-v-1","assets.assetbundle");

		HttpGet ("");
	}

	void Update ()
	{
		// if(Input.GetKeyDown(KeyCode.A))
		// GetObject.AsyncGetObject ("anywhere-v-1","assets.assetbundle");
	}

	public string HttpGet (string search)
	{
		HttpWebRequest request = WebRequest.Create ("https://weacw.com/anywhere/searchinfo.php?search=" + search) as HttpWebRequest;  
		request.Method = "GET";  
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		Stream myResponseStream = response.GetResponseStream ();
		StreamReader myStreamReader = new StreamReader (myResponseStream, Encoding.GetEncoding ("utf-8"));
		string retString = myStreamReader.ReadToEnd ();
		myStreamReader.Close ();
		myResponseStream.Close ();
		return retString;
	}

	public string HttpGet (int page)
	{

		HttpWebRequest request = WebRequest.Create ("https://weacw.com/anywhere/getinfo.php?page=" + page) as HttpWebRequest;
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
