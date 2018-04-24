using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public partial class Mng_User
    {
        [SugarColumn(IsIgnore = true)]
        public string DepartMentName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string GroupName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string ParUserName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsRecorded { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsApproved { get; set; }

        [SugarColumn(IsIgnore = true)]
        public int Score { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string ClassName { get; set; }
    }
}
