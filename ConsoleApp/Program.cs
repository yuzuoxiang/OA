using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugarTool;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var db= SugarDao.GetInstance("OA");

            db.DbFirst.Where("Mng_DepartmentClass").CreateClassFile(@"C:\Users\Administrator\Desktop\Model");
        }
    }
}
