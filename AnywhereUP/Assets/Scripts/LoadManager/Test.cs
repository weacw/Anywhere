using UnityEngine;
using System.Collections;
using Anywhere;
public class Test : MonoBehaviour {
	public static Test instance;
	public GameObject sphere;
	// Use this for initialization
	void Start () {
		instance = this;
//		GameObject.	Instantiate(AssetBundleLoad.LoadAB("test").LoadAsset<GameObject>("Assets/Artwork/Effects/01.prefab"));
		//GetObject.AsyncGetObject ("anywhere-v-1","hs.mp4");
		UploadObject.UploadFile (Application.streamingAssetsPath+"/AssetBundle/assets.assetbundle","anywhere-v-1","assets.assetbundle");
		AnyWhereLoad.PlayVideo (sphere,"file://C:/Users/PXY/Desktop/Anywhere-master/Anywhere/AnywhereUP/Assets/Scripts/LoadManager/青山パチンカー奈美 1992).mp4");

	}
	void Update()
	{
		// if(Input.GetKeyDown(KeyCode.A))
			// GetObject.AsyncGetObject ("anywhere-v-1","assets.assetbundle");
	}
	IEnumerator myw()
	{
		WWW www = new WWW ("https://weacw.com/uploadinfo.php?place=beijing&descript=bj&version=11&assetName=assets&thumbnailname=PortalNormal");
		yield return www;
		Debug.Log (www.text);
	}
}
