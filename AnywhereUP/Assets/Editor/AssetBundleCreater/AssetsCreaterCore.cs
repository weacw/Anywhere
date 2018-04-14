	/************************************************************************************
 * Copyright (c) WEACW All Rights Reserved.
 *Author:Well Tsai
 *Email:paris3@163.com 
 *http://weacw.com
/************************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetsCreaterCore :Editor
{
    public static void BuildAbs(List<string> targetList,string savePath,BuildTarget platform)
    {
        AssetBundleBuild[] build = new AssetBundleBuild[2];
        build[0].assetBundleName = System.IO.Path.GetFileName(savePath);
        build[0].assetNames = targetList.ToArray();

        string[] str = savePath.Split('/');
        string subPath = null;
        for (int i = 1; i < str.Length-1; i++)
        {
            subPath += str[i]+"/";
        }        
        BuildPipeline.BuildAssetBundles(Application.dataPath+ "/"+subPath, build,BuildAssetBundleOptions.ChunkBasedCompression, platform);       
        AssetDatabase.Refresh(ImportAssetOptions.Default);
    }

  
}