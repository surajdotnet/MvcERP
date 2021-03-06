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
    
    public partial class tbl_MstMerchants
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_MstMerchants()
        {
            this.tbl_AdvertisePlansXref = new HashSet<tbl_AdvertisePlansXref>();
            this.tbl_MstMerchantsRoleXref = new HashSet<tbl_MstMerchantsRoleXref>();
        }
    
        public int pkMerchantId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public Nullable<int> fkBossId { get; set; }
        public string Mobile_Primary { get; set; }
        public string Mobile_Alternate { get; set; }
        public string Email_Primary { get; set; }
        public string Email_Alternate { get; set; }
        public string MerchantImage { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public Nullable<System.DateTime> DateofJoining { get; set; }
        public Nullable<int> IsMobileVerified { get; set; }
        public Nullable<System.DateTime> MobileVerifiedDate { get; set; }
        public Nullable<int> IsEmailVerified { get; set; }
        public Nullable<System.DateTime> EmailVerifiedDate { get; set; }
        public Nullable<System.DateTime> LastLoggedInDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifedDate { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordResetCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AdvertisePlansXref> tbl_AdvertisePlansXref { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_MstMerchantsRoleXref> tbl_MstMerchantsRoleXref { get; set; }
    }
}
