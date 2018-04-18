using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Tool
{
    public class Tables
    {
        private int colnum = 0;
        private StringBuilder table = new StringBuilder();
        private StringBuilder temp = new StringBuilder();
        public Tables()
        {
            this.table.AppendLine("     <table class=\"layui-table\" lay-even=\"\" lay-skin=\"row\">");
        }
        public void AddCol(string value, bool center = false, int colSpan = 0, int rowSpan = 0)
        {
            this.temp.AppendFormat("     <td {0} {1} {2} >{3}</td>\n",
                center ? "style=\"text-align: center;\"" : "",
                colSpan > 0 ? "colspan=" + colSpan : "",
                rowSpan > 0 ? "rowspan=" + rowSpan : "",
                value.Trim().Equals("") ? "&nbsp;" : value);
        }

        public void AddHead(string[] heads)
        {
            this.temp.AppendFormat($"     <thead>");
            foreach (var item in heads)
            {
                this.temp.AppendFormat($"     <th >{item}</th>\n");
            }
            this.temp.AppendFormat($"     </thead>");
            this.AddRow();
        }
        public void AddHeadHtml(string headHtml)
        {
            this.temp.AppendFormat($"     <thead>");
            this.temp.AppendFormat($"     {headHtml}\n");
            this.temp.AppendFormat($"     </thead>");
            this.AddRow();
        }

        public void AddHeadCol(string width, string value)
        {
            this.temp.AppendFormat($"     <th width=\"{width}\">{value}</th>\n");
            this.colnum++;
        }

        public void AddHeadCol(string value)
        {
            this.temp.AppendFormat($"     <th >{value}</th>\n");
            this.colnum++;
        }

        public void AddRow()
        {
            this.table.AppendLine("  <tr>");
            this.table.AppendLine(this.temp.ToString());
            this.table.AppendLine("  </tr>");
            this.temp.Remove(0, this.temp.Length);
        }
        public string GetTable()
        {
            this.table.AppendLine("    </table>");
            string tmp = this.table.ToString();
            this.temp = null;
            this.table = null;
            return tmp;
        }
    }
}
