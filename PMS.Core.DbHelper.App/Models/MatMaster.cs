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
    
    public partial class MatMaster
    {
        public string MAPCD { get; set; }
        public string MAPRCD { get; set; }
        public string MAPDS { get; set; }
        public string MADTLS { get; set; }
        public string MASIC { get; set; }
        public string MACAT { get; set; }
        public string MABRAND { get; set; }
        public string MATYPE { get; set; }
        public string MAGRP { get; set; }
        public string MASUBGRP { get; set; }
        public string MACLASS { get; set; }
        public string MABINNO { get; set; }
        public string MAUOM { get; set; }
        public string MA_DEF_UOM { get; set; }
        public Nullable<decimal> MA_REORD_LEVEL { get; set; }
        public Nullable<decimal> MA_REORD_QTY { get; set; }
        public Nullable<decimal> MA_MIN_QTY { get; set; }
        public Nullable<decimal> MA_MAX_QTY { get; set; }
        public string MALOCIMP { get; set; }
        public string MAPACK { get; set; }
        public string MAMAKE { get; set; }
        public string MAJBCAT { get; set; }
        public Nullable<decimal> MALEAD { get; set; }
        public string MA_COS_GL { get; set; }
        public string MA_SAL_GL { get; set; }
        public string MA_CONS_GL { get; set; }
        public bool MA_NONSTK { get; set; }
        public Nullable<bool> MA_INACTIVE { get; set; }
        public Nullable<bool> MA_BATCH { get; set; }
        public Nullable<decimal> MATOLPCT { get; set; }
        public string mapcd_old { get; set; }
        public string MA_SUP_CD { get; set; }
        public string MAPSUP { get; set; }
        public Nullable<bool> ma_del_inv { get; set; }
        public Nullable<bool> MA_RTN { get; set; }
        public Nullable<bool> MA_SALE { get; set; }
        public Nullable<bool> MA_PURCHASE { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Flag { get; set; }
        public string MA_INV_GL { get; set; }
        public string MA_REV_ACC { get; set; }
        public string MA_PRDCT_LN { get; set; }
    }
}