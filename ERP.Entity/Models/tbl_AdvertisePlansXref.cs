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
    
    public partial class tbl_AdvertisePlansXref
    {
        public int Id { get; set; }
        public Nullable<int> fkMerchantId { get; set; }
        public Nullable<int> fkPlanId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifedBy { get; set; }
        public Nullable<System.DateTime> ModifedDate { get; set; }
        public Nullable<int> IsActive { get; set; }
    
        public virtual tbl_Mst_AdvertisePlans tbl_Mst_AdvertisePlans { get; set; }
        public virtual tbl_MstMerchants tbl_MstMerchants { get; set; }
    }
}
