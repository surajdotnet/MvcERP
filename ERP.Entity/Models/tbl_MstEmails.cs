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
    
    public partial class tbl_MstEmails
    {
        public int pkId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Smtp_Server { get; set; }
        public Nullable<int> IsEnableSSL { get; set; }
        public Nullable<int> Incomming_Port { get; set; }
        public Nullable<int> Outgoing_Port { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> IsActive { get; set; }
    }
}
