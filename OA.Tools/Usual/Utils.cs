using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Tools.Config;

namespace Tools.Usual
{
    public class Utils
    {
        #region 文件处理
        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <param name="fileName">需要删除的图片，多张删除用'|'隔开</param>
        public static void DeleteFile(string filePath, string fileName)
        {
            string[] files = fileName.Split('|');
            foreach (string file in files)
            {
                if (file.Trim() != "")
                {
                    //删除大图
                    string deleteFile = System.Web.HttpContext.Current.Server.MapPath(filePath + file.Trim());
                    if (File.Exists(deleteFile))
                        File.Delete(deleteFile);

                    //删除小图
                    int deleteExt = deleteFile.Trim().LastIndexOf('.');
                    if (deleteExt > 0)
                    {
                        string deleteSmallFile = deleteFile.Substring(0, deleteExt) + "s" + deleteFile.Substring(deleteExt);
                        if (File.Exists(deleteSmallFile))
                            File.Delete(deleteSmallFile);
                    }
                }
            }
        }

        /// <summary>
        /// 更具配置删除文件
        /// </summary>
        /// <param name="uptype">文件配置</param>
        /// <param name="fileName">文件地址</param>
        public static void DeletePic(string uptype, string fileName)
        {
            if (UploadProvice.Instance().Settings[uptype] == null)
                return;
            //删除大图
            string path = UploadProvice.Instance().Settings[uptype].FilePath + fileName;
            if (File.Exists(path))
                File.Delete(path);
        }
        #endregion

        /// <summary>
        /// 获取分割字符串后ID数组(空则返回Null)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string[] SplitStr(string target, string split)
        {
            return Regex.Split(target, split, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取中英文混合字符串的文字长度(1个英文占1个长度，1个汉字占2个长度)
        /// </summary>
        /// <param name="stringWithEnglishAndChinese">中英文混合的字符串</param>
        /// <returns>字符串长度(1个英文占1个长度，1个汉字占2个长度)</returns>
        public static int GetEnglishLength(string stringWithEnglishAndChinese)
        {
            int lng = 0;
            for (int i = 0; i < stringWithEnglishAndChinese.Length; i++)
            {
                byte[] b = System.Text.Encoding.Default.GetBytes(stringWithEnglishAndChinese.Substring(i, 1));
                if (b.Length > 1)
                    lng += 2;
                else
                    lng += 1;
            }
            return lng;
        }

        /// <summary>
        /// 获取当前路径文件夹名称
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string GetLastFolderName(string dirPath)
        {
            return dirPath.Split('/').ToList()[dirPath.Split('/').Length - 2];
        }

        /// <summary>
        /// 转换Url
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConverUrl(string value)
        {
            return Regex.Replace(value, @"[\W]+", "-");
        }

        /// <summary>
        /// 转换字段名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConverFieldName(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }

        /// <summary>
        /// 根据Key获取value
        /// by；willian  date：2016-4-26
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(Dictionary<string, string> dic, string key)
        {
            return dic.FirstOrDefault(q => q.Key == key).Value;
        }

        /// <summary>
        /// 根据条件筛选列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetSelect(DataTable dt, string where)
        {
            return new DataView(dt, where, "", DataViewRowState.CurrentRows).ToTable();
        }

        /// <summary>
        /// 获取唯一数据的字段
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PrimaryKeyColumns"></param>
        /// <returns></returns>
        public static DataTable GetDistinctPrimaryKeyColumnTable(DataTable dt, string[] PrimaryKeyColumns)
        {
            DataView dv = dt.DefaultView;
            DataTable dtDistinct = dv.ToTable(true, PrimaryKeyColumns);

            //第一个参数是关键,设置为 true，则返回的 System.Data.DataTable 将包含所有列都具有不同值的行。默认值为 false。
            return dtDistinct;
        }

        /// <summary>
        /// 获取搜索关键字列表 空格区分
        /// by:willian date:2015-12-31
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static List<string> GetListKeywords(string keyword)
        {
            List<string> listKeyword = new List<string>();
            string[] keylist = Regex.Split(keyword, @"\s+");
            for (int i = 0; i < keylist.Length; i++)
            {
                if (keylist[i] != "")
                    listKeyword.Add(keylist[i]);
            }
            return listKeyword;
        }
    }
}
