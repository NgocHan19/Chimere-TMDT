﻿using System.ComponentModel.DataAnnotations;

namespace TMDT.Models
{
	public class VNPayInfoModel
	{
		[Key]
		public int Id { get; set; }
		public string OrderDescription { get; set; }
		public string TransactionId { get; set; }
		public string OrderId { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentId { get; set; }
		public DateTime DateCreate { get; set; }
	}
}
