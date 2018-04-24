using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Bll
{
    public class BllMng_DepartmentClass : BllSys_Class<Mng_DepartmentClass>
    {
        //private static readonly string DbName = "OA";

        public new static BllMng_DepartmentClass Instance()
        {
            return new BllMng_DepartmentClass();
        }


    }
}
