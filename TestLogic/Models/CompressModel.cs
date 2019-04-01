using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestLogic.Models
{
	public class CompressModel
	{
		[Display(Name = "String to compress")]
		public string StringValue { get; set; }

	}
}
