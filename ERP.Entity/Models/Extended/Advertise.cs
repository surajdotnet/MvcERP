using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entity.Models.Extended
{
    public class Advertise
    {
        public int? Id { get; set; }
        public int? fkSubCategory { get; set; }
        public string Add_Id { get; set; }
        public string Add_Title { get; set; }
        public string Add_Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public Nullable<decimal> Selling_Price { get; set; }
        public bool IsMobileView { get; set; }
        public string Geo_Lat { get; set; }
        public string Geo_Long { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }
        public Nullable<System.DateTime> ValidTill { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> IsActive { get; set; }
    }
}
