using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Image360LoaderModule")]
    public class Image360LoaderModule : BaseModule
    {
        internal void PlayImage360(Notification _notif)
        {
            Image360Helper tmp_Image360Helper = _notif.param as Image360Helper;
            if (tmp_Image360Helper.m_BeginInstance != null) tmp_Image360Helper.m_BeginInstance.Invoke();
            GameObject tmp_Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tmp_Sphere.name = "TMP_IMAGE360";
            Transform tmp_Transform = tmp_Sphere.transform;
            tmp_Transform.position = Vector3.zero;
            tmp_Transform.localScale = Vector3.one;
            tmp_Transform.rotation = Quaternion.identity;
            tmp_Sphere.SetActive(false);
            tmp_Sphere.AddComponent<SphereUpdate>().m_Target = GameObject.FindGameObjectWithTag("MainCamera").transform;
            Material tmp_Material = tmp_Sphere.GetComponent<Renderer>().material;
            tmp_Material.shader = Shader.Find("Custom/Video360");

            byte[] tmp_Bytes = null;
            Texture2D tmp_Texture = null;
            string tmp_Path = Configs.GetConfigs.m_CachePath;
            Loom.RunAsync(() =>
            {
                new System.Threading.Thread(() =>
                {
                    tmp_Bytes = File.ReadAllBytes(tmp_Path + "/" + tmp_Image360Helper.m_ImageName + ".jpg");
                }).Start();
                Loom.QueueOnMainThread((parm) =>
                {
                    tmp_Texture = new Texture2D(1024, 1024);
                    tmp_Texture.LoadImage(tmp_Bytes);
                    tmp_Texture.Compress(true);
                    tmp_Texture.Apply(false);
                    tmp_Material.SetTexture("_MainTex", tmp_Texture);
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_SETUP, new ContentSetupHelper() { m_Content = tmp_Sphere });
                    if (tmp_Image360Helper.m_EndIntance != null) tmp_Image360Helper.m_EndIntance.Invoke();
                }, null);
            });
        }
    }
}
