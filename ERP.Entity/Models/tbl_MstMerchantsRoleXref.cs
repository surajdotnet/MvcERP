//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Entity.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_MstMerchantsRoleXref
    {
        public int pkId { get; set; }
        public Nullable<int> fkMerchantId { get; set; }
        public Nullable<int> fkRoleId { get; set; }
        public Nullable<int> IsDefaultRole { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> IsActive { get; set; }
    
        public virtual tbl_MstMerchants tbl_MstMerchants { get; set; }
        public virtual tbl_MstRoles tbl_MstRoles { get; set; }
    }
}