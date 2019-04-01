using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestLogic.Models
{
	public class TwoStrings
	{
		[Display(Name = "First string")]
		public string String1 {get; set;}

		[Display(Name = "Second string")]
		public string String2 {get; set;}
	}
}
