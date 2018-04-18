﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tools.Http
{
    public class WebRequestHelper
    {
        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Post<T>(string url, IDictionary<string, string> parameters)
        {
            var strResult = PostResponseStr(url, parameters);
            try
            {
                return JsonConvert.DeserializeObject<T>(strResult);
            }
            catch (Exception ex)
            {
                Logs.Log.Debug("Post请求时错误", err: ex);
            }
            return default(T);
        }

        /// <summary>
        /// Post 请求 string 返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string PostResponseStr(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in parameters.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                }
                i++;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(buffer.ToString());

            //写数据
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //   request.Proxy = null;
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //读数据
            request.Timeout = 300000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = null;
            string strResult = string.Empty;
            Stream streamReceive = null;
            StreamReader streamReader = null;
            try
            {
                //请求数据
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    streamReceive = response.GetResponseStream();
                    streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    strResult = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (response = (HttpWebResponse)ex.Response)
                    {
                        streamReceive = response.GetResponseStream();
                        streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                        strResult = streamReader.ReadToEnd();
                    }
                }
                else
                {
                    strResult = ex.Message;
                }
            }
            finally
            {
                //关闭流
                if (reqstream != null)
                    reqstream.Dispose();
                if (streamReader != null)
                    streamReader.Dispose();
                if (streamReceive != null)
                    streamReceive.Dispose();
                if (request != null)
                    request.Abort();
                if (response != null)
                    response.Dispose();

                Logs.Log.Info(strResult);
            }

            return (strResult);
        }
        #endregion

        #region Get请求
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="paraQuery"></param>
        /// <param name="paraHeader"></param>
        /// <returns></returns>
        public static T Get<T>(string url, IDictionary<string, string> paraQuery, IDictionary<string, string> paraHeader)
        {
            var strResult = GetResponseStr(url, paraQuery, paraHeader);
            return JsonConvert.DeserializeObject<T>(strResult);
        }

        /// <summary>
        /// Get WebApi 内容 string类型返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paraQuery">加入参数</param>
        /// <param name="paraHeader">加入头信息</param>
        /// <returns></returns>
        public static string GetResponseStr(string url, IDictionary<string, string> paraQuery, IDictionary<string, string> paraHeader)
        {
            //加入参数
            int i = 0;
            bool isExistPara = url.Contains('?') ? true : false;//判断是否存在参数
            if (paraQuery != null)
                foreach (string key in paraQuery.Keys)
                {
                    if (i == 0 && !isExistPara)
                        url += string.Format("?{0}={1}", key, paraQuery[key]);
                    else
                        url += string.Format("&{0}={1}", key, paraQuery[key]);
                    i++;
                }
            Logs.Log.Info(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //加入头信息
            if (paraHeader != null)
                foreach (string key in paraHeader.Keys)
                    request.Headers.Add(key, paraHeader[key]);

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 90000;
            request.Headers.Set("Pragma", "no-cache");

            HttpWebResponse response = null;
            Stream streamReceive = null;
            StreamReader streamReader = null;
            string strResult = string.Empty;
            try
            {
                //请求数据
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    streamReceive = response.GetResponseStream();
                    streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    strResult = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (response = (HttpWebResponse)ex.Response)
                    {
                        streamReceive = response.GetResponseStream();
                        streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                        strResult = streamReader.ReadToEnd();
                    }
                }
                else
                {
                    strResult = ex.Message;
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Dispose();
                if (streamReceive != null)
                    streamReceive.Dispose();
                if (request != null)
                    request.Abort();
                if (response != null)
                    response.Dispose();
            }
            return strResult;
        }
        #endregion
    }
}
