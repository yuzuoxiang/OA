using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools.Http
{
    public class HtmlCommon
    {
        #region 网站通用
        /// <summary>
        /// 判断文件后缀是否是图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImg(string fileName)
        {
            string imgFile = ".gif.jpg.jpeg.bmp.png";
            return imgFile.Contains(GetFileExt(fileName));
        }

        /// <summary>
        /// 获取文件后缀
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.') + 1);
        }


        /// <summary>
        /// 图片Flash HTML代码
        /// </summary>
        /// <param name="picAddr">图片或flash上传地址</param>
        /// <param name="config">上传配置文件</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static string GetPicHtml(string picAddr, string url, bool isnofollow, string width, string height)
        {
            if (string.IsNullOrEmpty(picAddr)) return "";
            string postfix = picAddr.Substring(picAddr.LastIndexOf('.'));
            if (postfix == ".swf")
            {
                return "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"" + width + "\" height=\"" + height + "\"><param name=\"movie\" value=\"" + picAddr + "\" /><param name=\"quality\" value=\"high\" /><embed src=\"" + picAddr + "\" quality=\"high\" width=\"" + width + "\" height=\"" + height + "\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed></object>";
            }
            else
            {
                return "<a href=\"" + url + "\" target=\"_blank\" " + (isnofollow ? "rel=\"nofollow\"" : "") + "><img src=\"" + picAddr + "\" border=\"0\"/></a>";
            }
        }

        /// <summary>
        /// 生成QQ临时聊天代码
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetQQ(string qq)
        {
            string[] qqsplit;
            string[] qqkey;
            string qqinx = "";
            string tempqq = " ";
            if (qq == "")
            {
                tempqq = " ";
            }
            else
            {
                qqsplit = qq.Split('/');
                if (qqsplit.Length >= 0)
                {
                    for (int i = 0; i < qqsplit.Length; i++)
                    {
                        qqkey = qqsplit[i].Split('#');
                        if (qqkey.Length > 0)
                        {
                            qqinx = qqkey[0].Trim() + "";
                        }
                        else
                        {
                            qqinx = "";
                        }
                        if (qqinx != "")
                        {
                            tempqq += "<a target=\"_blank\" href=\"http://wpa.qq.com/msgrd?v=1&uin=" + qqinx.Trim() + "&site=www.dzsc.com&menu=yes\" title=\"QQ:" + qqinx.Trim() + "\"><img border=\"0\" src=\"http://wpa.qq.com/pa?p=2:" + qqinx.Trim() + ":4\" alt=\"QQ:" + qqinx.Trim() + "\" onerror=\"this.src='http://img1.dzsc.com/img/common/qq.png'\"></a>";
                        }
                    }
                }
            }
            return tempqq;
        }

        /// <summary>
        /// 生成msn聊天代码
        /// </summary>
        /// <param name="msn"></param>
        /// <returns></returns>
        public static string GetMSN(string msn)
        {
            string temmsn = "";
            if (msn != "")
            {
                string[] temp = msn.Split('/');
                if (temp.Length >= 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Trim() != "")
                        {
                            temmsn += "<a href=\"msnim:chat?contact=" + temp[i].Trim() + "\"><img src=\"http://img1.dzsc.com/img/common/msn.png\" border=\"0\" alt=\"MSN:" + temp[i].Trim() + "\" ></a>";
                        }
                    }
                }
            }
            return temmsn;
        }

        /// <summary>
        /// 根据ID获取放置的文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocById(int id)
        {
            string docPath = "";
            string strId = id.ToString();
            int strLen = strId.Length;
            int strDepth = (int)Math.Floor((decimal)strLen / 3);
            for (int i = 0; i < strDepth; i++)
            {
                docPath += strId.Substring(0, 3) + "/";
                strId = strId.Substring(3);
            }
            docPath += "000/";
            return docPath;
        }

        /// <summary>
        /// 输出小图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetSmallPic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";

            return pic.Insert(pic.LastIndexOf('.'), "s");
        }

        /// <summary>
        /// 输出中图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetMiddlePic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";
            return pic.Insert(pic.LastIndexOf('.'), "m");
        }

        /// <summary>
        /// 输出大图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetBigPic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";
            return pic.Insert(pic.LastIndexOf('.'), "b");
        }

        /// <summary>
        /// 获取第一张图片
        /// </summary>
        /// <param name="uptype"></param>
        /// <param name="picStr"></param>
        /// <param name="getSmallPic"></param>
        /// <returns></returns>
        public static string GetFirstPic(string picStr, bool getSmallPic)
        {
            if (string.IsNullOrEmpty(picStr)) return "";

            return getSmallPic ? GetSmallPic(picStr.Split('|')[0]) : picStr.Split('|')[0];
        }
        #endregion

        #region 网络文件获取
        /// <summary>
        /// 下载网络文件(传递前导页，破简单的反盗链)
        /// </summary>
        /// <param name="url">网络文件地址</param>
        /// <param name="savePath">保存文件的路径</param>
        /// <param name="referer">需要传递的前导页</param>
        /// <returns>下载文件的大小(K)</returns>
        public static float DownLoadFile(string url, string savePath, string referer)
        {
            try
            {
                long ThisLength = 0;
                //https，协议需要根据实际情况改变
                if (url.ToLower().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)";
                myHttpWebRequest.Referer = referer;
                myHttpWebRequest.Timeout = 10 * 1000;
                myHttpWebRequest.Method = "GET";

                HttpWebResponse res = myHttpWebRequest.GetResponse() as HttpWebResponse;
                System.IO.Stream stream = res.GetResponseStream();


                byte[] b = new byte[1024];
                int nReadSize = 0;
                nReadSize = stream.Read(b, 0, 1024);

                System.IO.FileStream fs = System.IO.File.Create(savePath);
                try
                {

                    while (nReadSize > 0)
                    {
                        ThisLength += nReadSize;
                        fs.Write(b, 0, nReadSize);
                        nReadSize = stream.Read(b, 0, 1024);
                    }
                }
                catch
                {
                    ThisLength = 0;
                }
                finally
                {
                    fs.Close();
                }
                res.Close();
                stream.Close();
                myHttpWebRequest.Abort();
                if (ThisLength < 1024 && ThisLength > 0)
                {
                    return 1;
                }
                return (float)(ThisLength / 1024);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// WebClient下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static bool DownLoadFile(string url, string savePath)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                if (url.ToLower().Contains("https"))
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebHeaderCollection headers = new WebHeaderCollection();
                headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)");

                myWebClient.DownloadFile(url, savePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// WebClient下载文件（非阻塞）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static bool DownLoadFileAsync(string url, string savePath, string referer)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                if (url.ToLower().Contains("https"))
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Uri downloadUrl = new Uri(url);

                WebHeaderCollection headers = new WebHeaderCollection();
                headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)");
                headers.Add(HttpRequestHeader.Referer, referer);

                myWebClient.Headers = headers;
                myWebClient.DownloadFileAsync(downloadUrl, savePath);
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 根据URL提取图片名称并加上随机数
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <param name="bs">小图标记 _s</param>
        /// <returns></returns>
        public static string GetPictureNameFromUrl(string url, string bs)
        {
            if (string.IsNullOrEmpty(url) || !url.Contains("."))
                return string.Empty;
            string picname = bs + (url.Length > 0 ? url.Substring(url.LastIndexOf('.')) : "");
            if (string.IsNullOrEmpty(bs))
                picname = new Random().Next(100000).ToString() + picname;
            try
            {
                if (string.IsNullOrEmpty(url))
                    return picname = new Random().Next(100000).ToString() + picname;
                string name = url.Substring(url.LastIndexOf('/') + 1, url.LastIndexOf('.') - url.LastIndexOf('/') - 1);
                return string.Format("{0}{1}", name, picname);
            }
            catch
            {
                return picname;
            }
        }

        /// <summary>
        /// 下载图片通用函数
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="folderName">文件夹名</param>
        /// <param name="datePath">是否使用时间做为文件路径（适合图片很多，不会修改的图片）</param>
        /// <param name="picName">图片保存名称</param>
        /// <returns></returns>
        public static string DownLoadPicture(string url, string savePath, string folderName, bool datePath = true, string picName = "")
        {
            if (string.IsNullOrEmpty(picName))
                picName = GetPictureNameFromUrl(url, string.Empty);
            return DownLoadPicture(url, savePath, folderName, picName, datePath);
        }

        /// <summary>
        /// 下载图片通用函数
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="picName">图片名</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="folderName">文件夹名</param>
        /// <param name="dirPath">相对文件夹路径</param>
        /// <returns></returns>
        public static string DownLoadPicture(string url, string savePath, string folderName, string picName, bool datePath)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return string.Empty;

                string dirPath = string.Empty;
                if (datePath)
                {
                    DateTime datetime = DateTime.Now;
                    dirPath = string.Format("\\{0}\\{1}\\{2}\\{3}\\{4}\\", folderName, datetime.Year, datetime.Month, datetime.Day, datetime.Hour);
                }
                else
                {
                    dirPath = string.Format(@"\{0}\", folderName);
                }

                if (savePath.EndsWith("\\") || savePath.EndsWith("/"))
                    savePath = savePath.Substring(0, savePath.Length - 1);

                string sp = string.Format(@"{0}{1}", savePath, dirPath);
                if (!Directory.Exists(sp))
                    Directory.CreateDirectory(sp);

                string path = string.Format("{0}{1}", dirPath, picName);
                DownLoadFile(url, sp + "\\" + picName, string.Empty);
                return path;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
