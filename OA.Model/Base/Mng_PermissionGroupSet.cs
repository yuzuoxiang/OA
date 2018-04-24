using System;
using System.Text;
namespace OA.Model
{
    //Mng_PermissionGroupSet
    public class Mng_PermissionGroupSet
    {
        public Mng_PermissionGroupSet()
        {
        }
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// GroupId
        /// </summary>		
        public int? GroupId { get; set; }
        /// <summary>
        /// PermissionId
        /// </summary>		
        public int? PermissionId { get; set; }

    }
}