using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Anywhere;
using System.Threading;
using System.Net;
public class MyThread
{
	private string m_Myurl;
	public MyThread (string a)
	{
		m_Myurl = a;
	}
	public  void CreateGetHttpResponse()  
	{  
		
		HttpWebRequest request = WebRequest.Create(m_Myurl) as HttpWebRequest;  
		request.Method = "GET";  

	}
}
public class AssetsBundleUpLoader : EditorWindow
{

	private static AssetsBundleUpLoader m_WINDOW;
	private string m_Buckname = "anywhere-v-1";
    private static List<Object> sourcesObjects = new List<Object>();
    private static float OBJECTSLOTSIZE;
	private  string m_Dirname = "Dirname";
	private  string m_Type = "type";
	private  string m_Version = "Version";
	private  Object m_Thumbnailname;
	private  string m_Place = "place";
	private  string m_Introduce = "Introduce";

    [MenuItem("ABTools/ABUpload")]
    private static void Init()
    {
        sourcesObjects.Clear();
		m_WINDOW = GetWindow<AssetsBundleUpLoader>();
		m_WINDOW.titleContent = new GUIContent("Anywhere资源上传器","test");
	
		m_WINDOW.Show();

    }
	GUISkin t;
    private void OnGUI()
    {
		
		if (!m_WINDOW) {

			Init ();
		}
	
        OBJECTSLOTSIZE = (EditorGUIUtility.currentViewWidth / 4) * 0.98f;
        Rect hRect = EditorGUILayout.BeginHorizontal();
		GUILayout.Label ("AB名称",EditorStyles.miniBoldLabel);
		GUILayout.Label ("缩略图名称","label");

		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label ("类型","label");
		m_Version=GUILayout.TextField(m_Version,EditorStyles.textField);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginHorizontal();
		m_Thumbnailname=EditorGUILayout.ObjectField(m_Thumbnailname,typeof(Texture));
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginHorizontal();
		m_Place=GUILayout.TextField(m_Place,EditorStyles.textField);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginHorizontal();
		m_Introduce=GUILayout.TextArea(m_Introduce,EditorStyles.textArea);
        EditorGUILayout.EndHorizontal();
     
        //Draw drag&drap rect
		Rect curRect = EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(hRect.width), GUILayout.Height(m_WINDOW.position.height - 200));
        if (curRect.Contains(Event.current.mousePosition)) CheckDragNDrop();
        if (sourcesObjects.Count <= 0)
        {
			GUI.Label(new Rect(curRect.width / 2 - 90, m_WINDOW.position.height / 2 - 15f, 256, 25), "Empty!! Drap your AssetBundle to here.", EditorStyles.boldLabel);
        }
	
        EditorGUILayout.Space();
		CreateItemGrid(curRect);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("确认上传",EditorStyles.miniButtonMid))
		{
			if (m_Thumbnailname != null) {
				string tmp_Thumbnailpath = AssetDatabase.GetAssetPath (m_Thumbnailname);
				tmp_Thumbnailpath = tmp_Thumbnailpath.Remove (0, tmp_Thumbnailpath.LastIndexOf ("/") + 1);
				UploadObject.UploadFile (AssetDatabase.GetAssetPath (m_Thumbnailname), m_Buckname, tmp_Thumbnailpath);
			}
			for (int i = 0; i < sourcesObjects.Count; i++)
			{
				
				UploadObject.UploadFile(AssetDatabase.GetAssetPath(sourcesObjects[i]),m_Buckname,sourcesObjects[i].name+".assetbundle");
				if (AssetDatabase.GetAssetPath(sourcesObjects[i]).EndsWith (".assetbundle")) {
					m_Type = "ab";
				} 
				else {
					string[] tmp_Filetype={"jpg","png","PNG","JPG"};
					foreach (string t in tmp_Filetype) {
						if (AssetDatabase.GetAssetPath(sourcesObjects[i]).EndsWith (t))
							m_Type = "texture";
						else
						{
							m_Type = "video";
						}
					}

				}
				m_Dirname = sourcesObjects [i].name;
				string tmp_Myurl= string.Format("http://weacw.com/anywhere/uploadinfo.php?place={0}&type={1}&descript={2}&" +
					"version={3}&assetName={4}&thumbnailName={5}",m_Place,m_Type,m_Introduce,m_Version,m_Dirname,m_Thumbnailname.name);
				Debug.Log(tmp_Myurl);
				MyThread tmp_Mythrea=new MyThread(tmp_Myurl);
				tmp_Mythrea.CreateGetHttpResponse ();
			}

		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		GUI.Label(new Rect(curRect.width / 2 - 90, m_WINDOW.position.height - 15f, 180, 25), "Powerd by WEACW Well Tsai", EditorStyles.miniBoldLabel);
		m_WINDOW.Repaint();
    }
    //Create the item in a grid style
    private void CreateItemGrid(Rect curRect)
    {
        int vertical = 0, horizatonal = 0;
        for (int i = 0; i < sourcesObjects.Count; i++)
        {
            if (i%4 == 0 && i != 0)
            {
                vertical++;
                horizatonal = 0;
            }

            Rect boxRect = new Rect(curRect.x + 5 + (OBJECTSLOTSIZE*horizatonal),
                curRect.y + 5 + OBJECTSLOTSIZE*vertical,
                OBJECTSLOTSIZE - 10, OBJECTSLOTSIZE - 10);
            horizatonal++;
            if (GUI.Button(boxRect, sourcesObjects[i].name, "WindowBackground"))
                OnMouseEventCheck(boxRect, i);
        }
    }

    //Check mouse click on the item and show grenice menu
    public void OnMouseEventCheck(Rect rect, int index)
    {
        if (Event.current.type == EventType.Used)
        {
            if (rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
            {
                EditorGUIUtility.PingObject(sourcesObjects[index]);
            }
            else
            {
                if (Event.current.button == 1)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Remove"), false, RemoveCurItem, sourcesObjects[index]);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
            }
        }
    }
    //GenericMenu function
    public void RemoveCurItem(object obj)
    {
		sourcesObjects.Remove((Object)obj);m_Thumbnailname = null;

    }
    //Check drag or drop
    public void CheckDragNDrop()
    {
        if (null == DragAndDrop.objectReferences) return;
        switch (Event.current.type)
        {
            case EventType.DragUpdated:
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                break;
            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();
                foreach (Object o in DragAndDrop.objectReferences)
                {
				
			
                    if (sourcesObjects.Contains(o))
                    {
                        m_WINDOW.ShowNotification(new GUIContent(string.Format("Repeat to add {0}.", o.name)));
                        continue;
                    }

                    sourcesObjects.Add(o);
                }
                Event.current.Use();
                break;
        }
    }
}