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
    public MyThread(string a)
    {
        m_Myurl = a;
    }
    public void CreateGetHttpResponse()
    {

        HttpWebRequest request = WebRequest.Create(m_Myurl) as HttpWebRequest;
        request.Method = "GET";
        using (WebResponse wp = request.GetResponse())
        {
            using (StreamReader sr = new StreamReader(wp.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                Debug.Log(sr.ReadToEnd().ToString());
            }
        }

    }
}
public class AssetbundleUploader : EditorWindow
{

    private static AssetbundleUploader m_WINDOW;
    private string m_Buckname = "anywhere-v-1";
    private float m_Version = 1.0f;
    private string m_Place = null;
    private string m_Descript = null;

    private Object m_AssetbundleFile = null;
    private Texture m_ThumbnailFile;
    private const string m_Gateway = "https://aw.weacw.com/anywhere/uploadinfo.php";
    private FILETYPE m_FileType;

    private bool m_ShowInfo;
    public enum FILETYPE
    {
        VIDEO360,
        IMAGE360,
        ASSETBUNDLE
    }
    [MenuItem("ABTools/ABUpload")]
    private static void Init()
    {
        m_WINDOW = GetWindow<AssetbundleUploader>();
        m_WINDOW.titleContent = new GUIContent("Anywhere uploader", "");
        m_WINDOW.Show();
        UploadObject.OnUploaded = m_WINDOW.Uploaded;
        UploadObject.OnUploading = m_WINDOW.Uploading;

    }
    private void OnDisable()
    {
        UploadObject.OnUploaded = null;
        UploadObject.OnUploaded = null;
    }

    private void OnGUI()
    {

        if (!m_WINDOW)
        {
            Init();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        m_Place = EditorGUILayout.TextField("Place", m_Place);
        m_Version = EditorGUILayout.FloatField("Asset Version", m_Version, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.8f));
        m_AssetbundleFile = EditorGUILayout.ObjectField("Upload File", m_AssetbundleFile, typeof(object), false, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.8f));
        m_FileType = (FILETYPE)EditorGUILayout.EnumPopup("File Type", m_FileType, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.8f));

        m_Descript = EditorGUILayout.TextField("Descript", m_Descript, EditorStyles.textArea, GUILayout.Height(30));

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Thumbnail", GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
        m_ThumbnailFile = EditorGUILayout.ObjectField("", m_ThumbnailFile, typeof(Texture), false, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.15f)) as Texture;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Upload"))
        {
            if (m_ThumbnailFile == null || m_AssetbundleFile == null || m_Place == null)
            {
                Debug.LogError("File is empty");
                return;
            }
            string m_Filetypename = "";
            switch (m_FileType)
            {
                case FILETYPE.ASSETBUNDLE:
                    m_Filetypename = ".assetbundle";
                    break;
                case FILETYPE.IMAGE360:
                    m_Filetypename = ".png";
                    break;
                case FILETYPE.VIDEO360:
                    m_Filetypename = ".mp4";
                    break;
            }
            UploadObject.UploadFile(AssetDatabase.GetAssetPath(m_AssetbundleFile), m_Buckname, m_AssetbundleFile.name + m_Filetypename);
            UploadObject.UploadFile(AssetDatabase.GetAssetPath(m_ThumbnailFile), m_Buckname, m_ThumbnailFile.name + ".png");
            string tmp_Myurl = string.Format(m_Gateway + "?place={0}&type={1}&descript={2}&version={3}&assetName={4}&thumbnailName={5}",
                m_Place, m_FileType.ToString(), m_Descript, m_Version, m_AssetbundleFile.name+m_Filetypename, m_ThumbnailFile.name+".png");
            Debug.Log(tmp_Myurl);
            MyThread tmp_Mythrea = new MyThread(tmp_Myurl);
            Thread tmp_thread = new Thread(new ThreadStart(tmp_Mythrea.CreateGetHttpResponse));
            tmp_thread.Start();
        }

        if (GUILayout.Button("Clean"))
        {
            m_Descript = null;
            m_ThumbnailFile = null;
            m_AssetbundleFile = null;
            m_Version = 1;
            m_Place = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("WindowBackground");
        m_ShowInfo = EditorGUILayout.Foldout(m_ShowInfo, "Upload info");
        if (m_ShowInfo)
        {
            GUILayout.Label(string.Format("AB Name:{0}", m_AssetbundleFile == null ? "Empty" : m_AssetbundleFile.name));
            GUILayout.Label(string.Format("Thumbnail Name:{0}", m_ThumbnailFile == null ? "Empty" : m_ThumbnailFile.name));
        }
        EditorGUILayout.EndVertical();
        m_WINDOW.Repaint();
    }

    private void Uploaded()
    {
        Debug.Log("Uploaded");
    }
    private void Uploading()
    {
        Debug.Log("Uploading");
    }
}