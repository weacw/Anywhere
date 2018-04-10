using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
namespace Anywhere
{
    public class NativeShareBridge : Singleton<NativeShareBridge>
    {
#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _NativeShare(string scriptTarget, string _text, string _image, string _encodedMedia);
#endif

        public delegate void OnShareSuccess(string _platform);
        public delegate void OnShareCancel(string _platform);

        public OnShareSuccess onShareSuccess = null;
        public OnShareCancel onShareCancel = null;
        private void Start()
        {
            this.gameObject.name = "NativeShareBridge";
        }
        /// <summary>
        /// 调用系统分享
        /// </summary>
        /// <param name="_descript">分享描述</param>
        /// <param name="bytes">分享logo</param>
        /// <param name="_shareurl">分享链接</param>
        public void SocialShare(string _descript, byte[] bytes, string _shareurl)
        {
#if UNITY_IPHONE
            if (_shareurl == null) return;
            _NativeShare(this.name, _descript, System.Convert.ToBase64String(bytes), _shareurl);
#endif
        }

        private void OnNativeShareSuccess(string _result)
        {
            if (onShareSuccess == null) return;
            onShareSuccess.Invoke(_result);
        }

        private void OnNativeShareCancel(string _result)
        {
            if (onShareCancel == null) return;
            onShareCancel.Invoke(_result);
        }
    }
}