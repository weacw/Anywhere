/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class IesWithCipherParameters : IesParameters
    {
        private int cipherKeySize;

        /**
         * @param derivation the derivation parameter for the KDF function.
         * @param encoding the encoding parameter for the KDF function.
         * @param macKeySize the size of the MAC key (in bits).
         * @param cipherKeySize the size of the associated Cipher key (in bits).
         */
        public IesWithCipherParameters(
            byte[]  derivation,
            byte[]  encoding,
            int     macKeySize,
            int     cipherKeySize) : base(derivation, encoding, macKeySize)
        {
            this.cipherKeySize = cipherKeySize;
        }

        public int CipherKeySize
        {
            get
            {
                return cipherKeySize;
            }
        }
    }

}

#endif
