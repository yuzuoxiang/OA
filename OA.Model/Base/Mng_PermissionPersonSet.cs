using System;
using System.Text;
namespace OA.Model
{
    //Mng_PermissionPersonSet
    public partial class Mng_PermissionPersonSet
    {
        public Mng_PermissionPersonSet()
        {
        }
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// AdminId
        /// </summary>		
        public int? AdminId { get; set; }
        /// <summary>
        /// PermissionId
        /// </summary>		
        public int? PermissionId { get; set; }

    }
}