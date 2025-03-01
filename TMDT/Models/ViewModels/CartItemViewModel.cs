﻿using TMDT.Models;

namespace TMDT.Models.ViewModel
{
	public class CartItemViewModel
	{
		public List<CartItemModel> CartItems { get; set; }
		public decimal GrandTotal { get; set; }
		public decimal ShippingCost { get; set; }
        public string ShippingAddress { get; set; }
    }
}
