//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Core.DbHelper.App.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShipmentAddress
    {
        public int Id { get; set; }
        public string Ship_Id { get; set; }
        public string Cust_Id { get; set; }
        public string ContactPname { get; set; }
        public string Add { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public Nullable<decimal> Mobile { get; set; }
        public Nullable<decimal> Phone { get; set; }
        public Nullable<decimal> fax { get; set; }
        public string POBox { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> Our { get; set; }
        public string Comp { get; set; }
    }
}
