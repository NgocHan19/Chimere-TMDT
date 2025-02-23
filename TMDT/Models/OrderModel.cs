﻿namespace TMDT.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
		public string OrderCode { get; set; }
		public decimal ShippingCost { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
