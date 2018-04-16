﻿using System;
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
	public string myurl;
	public MyThread (string a)
	{
		myurl = a;
	}
	public  void CreateGetHttpResponse()  
	{  
		
		HttpWebRequest request = WebRequest.Create(myurl) as HttpWebRequest;  
		request.Method = "GET";  

	}
}
public class AssetsBundleUpLoader : EditorWindow
{

	private static AssetsBundleUpLoader window;
    private string buckname = "anywhere-v-1";
    private static List<Object> sourcesObjects = new List<Object>();
    private static float OBJECTSLOTSIZE;
	private  string Dirname = "Dirname";
	private  string type = "type";
	private  string Version = "Version";
	private  Object thumbnailname;
	private  string place = "place";
	private  string Introduce = "Introduce";

    [MenuItem("ABTools/ABUpload")]
    private static void Init()
    {
        sourcesObjects.Clear();
		window = GetWindow<AssetsBundleUpLoader>();
        window.titleContent = new GUIContent("ABUpload");
        window.Show();

    }

    private void OnGUI()
    {

		if (!window) {
			
			Init ();
		}
	
        OBJECTSLOTSIZE = (EditorGUIUtility.currentViewWidth / 4) * 0.98f;
        Rect hRect = EditorGUILayout.BeginHorizontal();
		place=GUILayout.TextField(place);
		Introduce=GUILayout.TextField(Introduce);
		Version=GUILayout.TextField(Version);
		thumbnailname=EditorGUILayout.ObjectField (thumbnailname,typeof(Texture));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("UpLoad", "minibuttonleft"))
        {
            List<string> assetsPath = new List<string>();
            for (int i = 0; i < sourcesObjects.Count; i++)
            {
				
				UploadObject.UploadFile(AssetDatabase.GetAssetPath(sourcesObjects[i]),buckname,sourcesObjects[i].name+".assetbundle");
				if (AssetDatabase.GetAssetPath(sourcesObjects[i]).EndsWith (".assetbundle")) {
					type = "ab";
				} 
				else {
					string[] temp={"jpg","png","PNG","JPG"};
					foreach (string t in temp) {
						if (AssetDatabase.GetAssetPath(sourcesObjects[i]).EndsWith (t))
							type = "texture";
						else
						{
							type = "video";
						}
					}

				}
				Dirname = sourcesObjects [i].name;
				string myurl= string.Format("http://weacw.com/anywhere/uploadinfo.php?place={0}&type={1}&descript={2}&" +
					"version={3}&assetName={4}&thumbnailName={5}",place,type,Introduce,Version,Dirname,thumbnailname.name);
				Debug.Log(myurl);
				MyThread mythrea=new MyThread(myurl);
				mythrea.CreateGetHttpResponse ();
            }

        }
        
        EditorGUILayout.EndHorizontal();

        //Draw drag&drap rect
        Rect curRect = EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(hRect.width), GUILayout.Height(window.position.height - 60));
        if (curRect.Contains(Event.current.mousePosition)) CheckDragNDrop();
        if (sourcesObjects.Count <= 0)
        {
            GUI.Label(new Rect(curRect.width / 2 - 90, window.position.height / 2 - 15f, 256, 25), "Empty!! Drap your AssetBundle to here.", EditorStyles.boldLabel);
        }
        EditorGUILayout.Space();
        //Creating the item
        CreateItemGrid(curRect);
        EditorGUILayout.EndHorizontal();
        //Show the copyright
        GUI.Label(new Rect(curRect.width / 2 - 90, window.position.height - 15f, 180, 25), "Powerd by WEACW Well Tsai", EditorStyles.miniBoldLabel);
        window.Repaint();
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
                OBJECTSLOTSIZE - 1, OBJECTSLOTSIZE - 1);
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
		sourcesObjects.Remove((Object)obj);thumbnailname = null;

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
                        window.ShowNotification(new GUIContent(string.Format("Repeat to add {0}.", o.name)));
                        continue;
                    }

                    sourcesObjects.Add(o);
                }
                Event.current.Use();
                break;
        }
    }
}