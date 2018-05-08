using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CacheMachine
{
	/// <summary>
	/// 判断文件是否为缓存
	/// </summary>
	/// <param name="_filePath"></param>
	/// <returns></returns>
    public static bool IsCache(string _filePath)
    {
        return File.Exists(_filePath);
    }

	/// <summary>
	/// 清空缓存
	/// </summary>
    public static void CleanCache()
    {
        DirectoryInfo tmp_FilesName = new DirectoryInfo(Configs.GetConfigs.m_CachePath);
        foreach (var tmp_File in tmp_FilesName.GetFiles())
        {
            if (File.Exists(tmp_File.FullName))
                File.Delete(tmp_File.FullName);
        }
    }

	/// <summary>
	/// 缓存大小
	/// </summary>
	/// <returns></returns>
    public static float GetCacheSize()
    {
        long tmp_Length = 0;
        DirectoryInfo tmp_FilesName = new DirectoryInfo(Configs.GetConfigs.m_CachePath);
        FileInfo[] tmp_FileInfo = tmp_FilesName.GetFiles();
        foreach (var tmp_File in tmp_FileInfo)
            tmp_Length += tmp_File.Length;
        Debug.Log(tmp_Length);
        return tmp_Length / 1024f / 1024f;
    }
}
