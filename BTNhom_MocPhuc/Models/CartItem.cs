using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTNhom_MocPhuc.Models
{
    [Serializable]
    public class CartItem
    {
        public SANPHAM sanPham { get; set; }
        public int soLuong { get; set; }
    }
}