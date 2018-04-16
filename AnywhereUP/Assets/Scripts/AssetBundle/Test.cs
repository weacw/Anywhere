using UnityEngine;
using System.Collections;
using Aliyun.OSS;
public class Test : MonoBehaviour {
	public static Test instance;
	// Use this for initialization
	void Start () {
		instance = this;
//		GameObject.	Instantiate(AssetBundleLoad.LoadAB("test").LoadAsset<GameObject>("Assets/Artwork/Effects/01.prefab"));
		//GetObject.AsyncGetObject ("anywhere-v-1","hs.mp4");
		UploadObject.UploadFile (Application.streamingAssetsPath+"/AssetBundle/assets.assetbundle","anywhere-v-1","assets.assetbundle");
		StartCoroutine (myw());
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
			GetObject.AsyncGetObject ("anywhere-v-1","assets.assetbundle");
	}
	IEnumerator myw()
	{
		WWW www = new WWW ("https://weacw.com/uploadinfo.php?place=beijing&descript=bj&version=11&assetName=assets&thumbnailname=PortalNormal");
		yield return www;
		Debug.Log (www.text);
	}
}
