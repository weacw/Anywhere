using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Image360LoaderModule")]
    public class Image360LoaderModule : BaseModule
    {

        private Texture LoadTexture(string _texturename, int _t2dwith, int _t2dheight)
        {

            byte[] m_T2dbyts = File.ReadAllBytes(Configs.GetConfigs.m_CachePath + "/Texture/" + _texturename + ".png");
            Texture2D m_T2d = new Texture2D(_t2dwith, _t2dheight);
            m_T2d.LoadImage(m_T2dbyts);
            return m_T2d;
        }
    }
}
