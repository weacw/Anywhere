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
    private string m_SavePath = "Select the path to save under the Assets folder";
    private static List<Object> sourcesObjects = new List<Object>();
    private BuildTarget m_BuidTarget = BuildTarget.iOS;

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

        Rect hRect = EditorGUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Path:{0}", m_SavePath), "Box");
        if (GUILayout.Button("Path"))
        {
            m_SavePath = EditorUtility.SaveFilePanelInProject("Save path", "Assetbundle", "assetbundle", "");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Abs", "minibutton"))
        {
            if (String.IsNullOrEmpty(m_SavePath) || m_SavePath == "Select the path to save under the Assets folder")
            {
                window.ShowNotification(new GUIContent("Select the folder"));
                return;
            }
            if (System.IO.File.Exists(m_SavePath))
            {
                window.ShowNotification(new GUIContent("Folder already exists"));
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

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clean"))
        {
            sourcesObjects.Clear();
        }
        if (GUILayout.Button("Open folder"))
        {
            string tmp_folderPath = System.IO.Path.GetDirectoryName(m_SavePath).Replace("Assets/","");
            string tmp_fullPath = System.IO.Path.Combine(Application.dataPath, tmp_folderPath);
            System.Diagnostics.Process.Start(tmp_fullPath);
        }
        EditorGUILayout.EndHorizontal();


        GUILayout.Space(5);
        //Draw drag&drap rect
        Rect curRect = EditorGUILayout.BeginHorizontal("WindowBackground", GUILayout.Width(hRect.width), GUILayout.Height(window.position.height));
        if (curRect.Contains(Event.current.mousePosition)) CheckDragNDrop();
        if (sourcesObjects.Count <= 0)
        {
            GUI.Label(new Rect(curRect.width / 2 - 90, window.position.height / 2 - 15f, 256, 25), "Drap the object to here.", EditorStyles.boldLabel);
        }
        EditorGUILayout.Space();
        //Creating the item
        CreateItemGrid(curRect);
        EditorGUILayout.EndHorizontal();
        window.Repaint();
    }
    //Create the item in a grid style
    private void CreateItemGrid(Rect curRect)
    {
        for (int i = 0; i < sourcesObjects.Count; i++)
        {
            Rect boxRect = new Rect(curRect.x + 5, curRect.y + 5 + i * 35, curRect.width * 0.98f, 35);
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