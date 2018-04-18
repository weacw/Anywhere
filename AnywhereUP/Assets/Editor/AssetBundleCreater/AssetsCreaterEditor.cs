/************************************************************************************
 * Copyright (c) WEACW All Rights Reserved.
 *Author:Well Tsai
 *Email:paris3@163.com 
 *http://weacw.com
/************************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetsCreaterEditor : EditorWindow
{

    private static AssetsCreaterEditor window;
    private string m_SavePath = "选择保存在Asset下的路径";
    private static List<Object> sourcesObjects = new List<Object>();    
    private BuildTarget m_BuidTarget=BuildTarget.iOS;
    private static float OBJECTSLOTSIZE;

    [MenuItem("ABTools/ABCreater")]
    private static void Init()
    {
        sourcesObjects.Clear();
        window = GetWindow<AssetsCreaterEditor>();
        window.titleContent = new GUIContent("Assets Creater");
        window.Show();
       
    }

    private void OnGUI()
    {

        if (!window)
            Init();

        OBJECTSLOTSIZE = (EditorGUIUtility.currentViewWidth / 4) * 0.98f;


        Rect hRect = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(window.minSize.x * 0.5f)))
        {
			m_SavePath = EditorUtility.SaveFilePanelInProject("Save path", "Assets", "assetbundle", "");

        }
		GUILayout.TextField(m_SavePath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Abs", "minibuttonleft"))
        {
			if (String.IsNullOrEmpty (m_SavePath) || m_SavePath == "选择保存在Asset下的路径") {
				window.ShowNotification (new GUIContent ("选择你的文件夹 !!"));
				return;
			}
			if (System.IO.File.Exists (m_SavePath)) {
				window.ShowNotification (new GUIContent ("文件已经存在!!"));
				return;
			}
            List<string> assetsPath = new List<string>();
            for (int i = 0; i < sourcesObjects.Count; i++)
            {
                assetsPath.Add(AssetDatabase.GetAssetPath(sourcesObjects[i]));
            }
			AssetsCreaterCore.BuildAbs(assetsPath, m_SavePath, m_BuidTarget);
        }
       
        EditorGUILayout.EndHorizontal();

        //Draw drag&drap rect
        Rect curRect = EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(hRect.width), GUILayout.Height(window.position.height - 60));
        if (curRect.Contains(Event.current.mousePosition)) CheckDragNDrop();
        if (sourcesObjects.Count <= 0)
        {
            GUI.Label(new Rect(curRect.width / 2 - 90, window.position.height / 2 - 15f, 256, 25), "Empty!! Drap the object to here.", EditorStyles.boldLabel);
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
        sourcesObjects.Remove((Object)obj);
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