using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model.Extensions.Base
{
    public partial class Mng_DepartmentClass
    {
        [SugarColumn(IsIgnore = true)]
        public string actStr { get; set; }
    }
}
