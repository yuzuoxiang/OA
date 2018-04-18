using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tools.Tool
{
    public class PageToolBar
    {
        public PageToolBar()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">记录总数</param>
        public PageToolBar(int pageSize, int recordCount)
        {
            pagesize = pageSize;
            recordcount = recordCount;
        }

        public int PageIndex
        {
            get
            {
                if (pageIndex == 0)
                {
                    // GetToolBar();
                    setindex();
                }
                return pageIndex;
            }
        }
        private int pageIndex;

        private int pagesize = 20;
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
        private int recordcount;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get { return recordcount; }
            set { recordcount = value; }
        }
        /// <summary>
        /// 分页HTML
        /// </summary>
        public string PageHtml
        {
            get
            {
                //if (string.IsNullOrEmpty(pagehtml))
                //{
                GetToolBar();
                //} 

                return pagehtml;
            }
        }
        private int pageCount;
        private string pagehtml;

        /// <summary>
        /// 获取分布Html
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">记录总数</param>
        /// <returns></returns>
        private void GetToolBar(int pageSize, int recordCount)
        {
            pagesize = pageSize;
            recordcount = recordCount;
            GetToolBar();
        }
        private void setindex()
        {
            pageCount = recordcount / pagesize;
            if (recordcount % pagesize != 0)
            {
                pageCount++;
            }

            if (GetInt("pageindex") < 1)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = GetInt("pageindex");
                if (pageIndex > pageCount)
                {
                    pageIndex = pageCount;
                }
            }
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        private void GetToolBar()
        {
            setindex();
            StringBuilder tool = new StringBuilder();
            string parastring = "";

            if (recordcount <= 0)
            {
                pagehtml = string.Format("\n<div class=\"page_toolbar\"><div class=\"quotes\">\n对不起，没有找到任何记录</div></div>\n");
                return;
            }
            if (System.Web.HttpContext.Current.Request.QueryString.Count > 0)
            {
                for (int i = 0; i <= System.Web.HttpContext.Current.Request.QueryString.Count - 1; i++)
                {
                    if (System.Web.HttpContext.Current.Request.QueryString.GetKey(i) == "pageindex")
                        continue;
                    parastring = string.Concat(new object[] { parastring, "&", System.Web.HttpContext.Current.Request.QueryString.GetKey(i), "=", System.Web.HttpContext.Current.Request.QueryString[i] });
                }
            }
            if (pageIndex <= 1)
            {
                tool.AppendLine("   <span class=\"disabled\" style=\"border-left: 1px solid #C6C6C6\">首页</span>");
                tool.AppendLine("   <span class=\"disabled\">上一页</span>");
            }
            else
            {
                tool.AppendLine("   <a href=?pageindex=1" + parastring + " style=\"border-left: 1px solid #C6C6C6\">首页</a>");
                tool.AppendLine(string.Concat(new object[] { "   <a href=?pageindex=", pageIndex - 1, parastring, ">上一页</a>" }));
            }
            for (int i = ((pageIndex - 4) > 0) ? (pageIndex - 4) : 1; i <= (((pageIndex + 4) < pageCount) ? (pageIndex + 4) : pageCount); i++)
            {
                if (i == pageIndex)
                {
                    tool.AppendLine("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    tool.AppendLine(string.Concat(new object[] { "<a href=\"?pageindex=", i, parastring, "\">", i, "</a>" }));
                }
            }
            if ((pageIndex + 5) < pageCount)
            {
                tool.AppendLine("<a href=\"#\">...</a>");
                tool.AppendLine(string.Concat(new object[] { "<a href=\"?pageindex=", pageCount, parastring, "\">", pageCount, "</a>" }));
            }
            if (pageIndex >= pageCount)
            {
                tool.AppendLine("   <span class=\"disabled\">下一页</span>");
                tool.AppendLine("   <span class=\"disabled\">尾页</span>");
            }
            else
            {
                tool.AppendLine(string.Concat(new object[] { "   <a href=?pageindex=", pageIndex + 1, parastring, ">下一页</a>" }));
                tool.AppendLine(string.Concat(new object[] { "   <a href=?pageindex=", pageCount, parastring, ">尾页</a>" }));
            }

            pagehtml = string.Format("\n<div class=\"page_toolbar\"><div class=\"page_info\"><div class=\"quotes\">\n{0}  <span class=\"disabled\">共{1}条记录</span></div></div></div>\n", tool.ToString(), recordcount);
        }

        public int GetInt(string para)
        {
            string tempInt = GetString(para);
            int intReturn = -1;
            if (QuickValidate("^[1-9]*[0-9]*$", tempInt) == true)
            {
                intReturn = int.Parse(tempInt);
            }
            return intReturn;
        }

        public bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            System.Text.RegularExpressions.Regex myRegex = new System.Text.RegularExpressions.Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }

        public static string GetString(string para)
        {
            if (HttpContext.Current.Request.RequestType.ToLower().Equals("get"))
            {
                return GetQueryString(para);
            }
            return GetFormString(para);
        }

        public static string GetQueryString(string para)
        {
            string queryString = "";
            if (HttpContext.Current.Request.QueryString[para] != null)
            {
                queryString = HttpContext.Current.Request.QueryString[para].ToString();
            }
            else
            {
                queryString = "";
            }
            return queryString;
        }

        public static string GetFormString(string para)
        {
            string formString = "";
            if (HttpContext.Current.Request.Form[para] != null)
            {
                formString = HttpContext.Current.Request.Form[para].ToString();
            }
            else
            {
                formString = "";
            }
            return formString.Trim();
        }
    }
}
