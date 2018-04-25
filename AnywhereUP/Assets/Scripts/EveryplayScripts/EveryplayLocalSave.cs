using System.Collections;
using System.IO;
using UnityEngine;
namespace Anywhere
{
    public static class EveryplayLocalSave
    {
#if !UNITY_EDITOR && UNITY_IOS
    [System.Runtime.InteropServices.DllImport( "__Internal" )]
    private static extern void _FlipVideoSynchronous( string path, string outputPath );

	[System.Runtime.InteropServices.DllImport( "__Internal" )]
    private static extern void _FlipVideoAsynchronous( string path, string outputPath );

	private static EveryplayLocalSaveHelper asyncHelper = null;
#endif

        private static string m_savePath = null;

        public static bool IsBusy { get; private set; }
        public static string SavedPath { get; private set; }

        public static bool SaveTo(string path)
        {
            if (SaveToInternal(path, false))
            {
                Debug.Log("Video saved: " + SavedPath);
                return true;
            }

            return false;
        }

        public static IEnumerator SaveToAsync(string path)
        {
            if (SaveToInternal(path, true))
            {
                while (IsBusy)
                    yield return null;

                if (SavedPath != null)
                    Debug.Log("Video saved: " + SavedPath);
                // FileStream fs = new FileStream(SavedPath, FileMode.Open);
                // long size = fs.Length;
                // byte[] array = new byte[size];
                // fs.Read(array, 0, array.Length);
                // fs.Close();
                // Debug.Log(array.Length);
                yield return  new WaitForEndOfFrame();
                Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.SOCIAL_SHARE, new SocialHelper()
                {
                    m_Body = "This is test",
                    m_FilePath = SavedPath,
                    m_Subject = "Share to your friends",
                    m_MimeType = "media/mp4",
                    m_URL = ""
                });
            }
            else
            {
                IsBusy = false;
            }

            yield break;
        }

        private static bool SaveToInternal(string path, bool async)
        {
            if (IsBusy)
            {
                Debug.LogError("Another save operation is in progress");
                return false;
            }

            SavedPath = null;

            if (path == null || path.Length == 0)
                throw new System.ArgumentException("Parameter 'path' is null or empty!");

            string recordedVideoDir = null;
#if UNITY_ANDROID
		recordedVideoDir = Path.Combine( new DirectoryInfo( Application.temporaryCachePath ).FullName, "sessions" );
#elif UNITY_IOS
            recordedVideoDir = new DirectoryInfo(Application.persistentDataPath).Parent.FullName + "/tmp/Everyplay/session";
#endif

            FileInfo[] files = new DirectoryInfo(recordedVideoDir).GetFiles("*.mp4", SearchOption.AllDirectories);
            if (files.Length > 0)
                recordedVideoDir = files[0].FullName;
            else
            {
                Debug.LogError("Couldn't find a recorded Everyplay session!");
                return false;
            }

            if (!File.Exists(path))
            {
                string directory = Path.GetDirectoryName(path);
                if (directory != null && directory.Length > 0)
                    Directory.CreateDirectory(directory);
            }

#if UNITY_EDITOR || UNITY_ANDROID
            File.Copy(recordedVideoDir, path, true);
            SavedPath = path;
#elif UNITY_IOS
		if( SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal )
		{
			File.Copy( recordedVideoDir, path, true );
			SavedPath = path;
		}
		else
		{
			if( !async )
			{
				_FlipVideoSynchronous( recordedVideoDir, path );
				if( !File.Exists( path ) )
					return false;

				SavedPath = path;
			}
			else
			{
				IsBusy = true;
				m_savePath = path;

				if( asyncHelper == null )
				{
					asyncHelper = new GameObject( "EveryplayLocalSaveHelper" ).AddComponent<EveryplayLocalSaveHelper>();
					asyncHelper.VideoProcessed = OnVideoProcessed;
					Object.DontDestroyOnLoad( asyncHelper.gameObject );
				}

				_FlipVideoAsynchronous( recordedVideoDir, path );
			}
		}
#endif

            return true;
        }

        private static void OnVideoProcessed(bool success)
        {
            IsBusy = false;

            if (success && File.Exists(m_savePath))
                SavedPath = m_savePath;

            m_savePath = null;
        }
    }
}