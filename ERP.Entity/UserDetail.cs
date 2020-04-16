using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entity
{
    class UserDetail
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int fkBossId { get; set; }
        public string Mobile_Primary { get; set; }
        public string Mobile_Alternate { get; set; }
        public string Email_Primary { get; set; }
        public string Email_Alternate { get; set; }
        public string MerchantImage { get; set; }
        public string DateofBirth { get; set; }
        public string DateofJoining { get; set; }
        public int IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string RoleIds { get; set; }
    }
}
