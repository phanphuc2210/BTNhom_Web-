//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTNhom_MocPhuc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTHD
    {
        public int IDHD { get; set; }
        public string MASP { get; set; }
        public int SOLUONG { get; set; }
        public int DONGIA { get; set; }
    
        public virtual HOADON HOADON { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
