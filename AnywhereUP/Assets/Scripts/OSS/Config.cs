using System;
using Aliyun.OSS.Common;
using UnityEngine;
using System.IO;

namespace Aliyun.OSS
{
    internal class Config
    {
        public static string AccessKeyId = "LTAI8ubxEX622ZV8";

        public static string AccessKeySecret = "BiQvCgfGE5cc6hUF8UUVxjIMWgIIly";

        public static string Endpoint = "oss-cn-beijing.aliyuncs.com";

        public static string DirToDownload = Path.Combine(Application.persistentDataPath , "");

        public static string FileToUpload = Path.Combine(Application.dataPath, "");

        public static string BigFileToUpload = "";
    }
}