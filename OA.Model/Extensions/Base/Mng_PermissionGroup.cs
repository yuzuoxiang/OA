using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public partial class Mng_PermissionGroup
    {
        [SugarColumn(IsIgnore = true)]
        public string actStr { get; set; }
    }
}
