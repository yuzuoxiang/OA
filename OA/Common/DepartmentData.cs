﻿using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA.Common
{
    public class DepartmentData
    {
        private static object locker = new object();

        private static List<Mng_DepartmentClass> departList = null;

        public DepartmentData()
        {
            Load();
        }

        public static Mng_DepartmentClass GetModel(int id)
        {
            return departList.Find(o => o.Id == id);
        }

        public static List<Mng_DepartmentClass> GetList()
        {
            return departList;
        }

        public static void Load()
        {
            lock (locker)
            {
                if (departList == null)
                    departList = Bll.BllMng_DepartmentClass.Instance().GetAll();
            }
        }

        public static void Reload()
        {
            lock (locker)
            {
                departList = Bll.BllMng_DepartmentClass.Instance().GetAll();
            }
        }
    }
}