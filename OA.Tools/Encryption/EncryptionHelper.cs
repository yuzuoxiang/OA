using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Encryption
{
    public class EncryptionHelper
    {
        #region 3DES加密类
        private static Des3 des3 = new Des3();
        public static string Des3_Encrypt(string str)
        {
            return des3.EncryptString(str);
        }

        public static string Des3_Decrypt(string str)
        {
            return des3.DecryptString(str);
        }

        /// <summary>
        /// 加解密类
        /// </summary>
        class Des3
        {
            //密钥
            private const string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM4";
            //向量，必须是12个字符
            private const string sIV = "jsafojxliqd=";

            //构造一个对称算法
            private SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();

            #region 加密解密函数

            /// <summary>
            /// 字符串的加密
            /// </summary>
            /// <param name="Value">要加密的字符串</param>
            /// <returns>加密后的字符串</returns>
            public string EncryptString(string Value)
            {
                try
                {
                    ICryptoTransform ct;
                    MemoryStream ms;
                    CryptoStream cs;
                    byte[] byt;
                    mCSP.Key = Convert.FromBase64String(sKey);
                    mCSP.IV = Convert.FromBase64String(sIV);
                    //指定加密的运算模式
                    mCSP.Mode = CipherMode.CBC;
                    //获取或设置加密算法的填充模式
                    mCSP.Padding = PaddingMode.Zeros;
                    ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                    byt = Encoding.UTF8.GetBytes(Value);
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                    cs.Write(byt, 0, byt.Length);
                    cs.FlushFinalBlock();
                    cs.Close();

                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// 解密字符串
            /// </summary>
            /// <param name="Value">加密后的字符串</param>
            /// <returns>解密后的字符串</returns>
            public string DecryptString(string Value)
            {
                try
                {
                    ICryptoTransform ct;//加密转换运算
                    MemoryStream ms;//内存流
                    CryptoStream cs;//数据流连接到数据加密转换的流
                    byte[] byt;
                    //将3DES的密钥转换成byte
                    mCSP.Key = Convert.FromBase64String(sKey);
                    //将3DES的向量转换成byte
                    mCSP.IV = Convert.FromBase64String(sIV);
                    mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
                    mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;
                    ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                    byt = Convert.FromBase64String(Value);
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                    cs.Write(byt, 0, byt.Length);
                    cs.FlushFinalBlock();
                    cs.Close();

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            #endregion
        }
        #endregion

        #region AES加密类
        private static AES aes = new AES();
        public static string Aes_Encrypt(string str, string strKey)
        {
            return aes.Encrypt(str, strKey);
        }

        public static string Aes_Decrypt(string str, string strKey)
        {
            return aes.Decrypt(str, strKey);
        }

        /// <summary>
        /// 加解密类
        /// </summary>
        class AES
        {
            /// <summary>  
            /// AES加密算法  
            /// </summary>  
            /// <param name="plainText">明文字符串</param>  
            /// <param name="strKey">密钥</param>  
            /// <returns>返回加密后的密文字节数组</returns>  
            public string Encrypt(string text, string strKey)
            {
                try
                {
                    //分组加密算法  
                    SymmetricAlgorithm des = Rijndael.Create();
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(text);//得到需要加密的字节数组      
                    //设置密钥及密钥向量  
                    des.Key = Encoding.UTF8.GetBytes(strKey);
                    des.IV = Encoding.UTF8.GetBytes(strKey); ;
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
                    cs.Close();
                    ms.Close();


                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in cipherBytes)
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    return ret.ToString();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            /// <summary>  
            /// AES解密  
            /// </summary>  
            /// <param name="cipherText">密文字节数组</param>  
            /// <param name="strKey">密钥</param>  
            /// <returns>返回解密后的字符串</returns>  
            public string Decrypt(string text, string strKey)
            {
                try
                {

                    List<byte> list = new List<byte>(text.Length / 2);
                    for (int i = 0; i < text.Length; i += 2)
                    {
                        list.Add(Convert.ToByte(text.Substring(i, 2), 16));
                    }

                    byte[] cipherText = list.ToArray();

                    SymmetricAlgorithm des = Rijndael.Create();
                    des.Key = Encoding.UTF8.GetBytes(strKey);
                    des.IV = Encoding.UTF8.GetBytes(strKey); ;

                    byte[] decryptBytes = new byte[cipherText.Length];
                    MemoryStream ms = new MemoryStream(cipherText);
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();

                    return System.Text.Encoding.UTF8.GetString(decryptBytes);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="code">16 or 32</param>
        /// <returns></returns>
        public static string Md5(string str, int code = 32)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            if (code == 16) //16位MD5加密（取32位加密的9~25字符）  
                return sBuilder.ToString().ToLower().Substring(8, 16);
            if (code == 32) //32位加密  
                return sBuilder.ToString().ToLower();

            return "00000000000000000000000000000000";
        }

        /// <summary>
        /// 可以自定义生成MD5加密字符传前的混合KEY
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] ShortUrl(string url, string key = "Leejor")
        {
            //要使用生成URL的字符
            string[] chars = new string[]{
                "a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p",
                "q","r","s","t","u","v","w","x",
                "y","z","0","1","2","3","4","5",
                "6","7","8","9","A","B","C","D",
                "E","F","G","H","I","J","K","L",
                "M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"
            };

            //对传入网址进行MD5加密
            string hex = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key + url, "md5");

            string[] resUrl = new string[4];

            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    outChars += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组
                resUrl[i] = outChars;
            }
            return resUrl;
        }
        #endregion
    }
}
