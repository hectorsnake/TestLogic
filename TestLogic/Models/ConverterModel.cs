using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestLogic.Models
{
	public class ConverterModel
	{
		[Display(Name = "Base type")]
		public int BaseId {get; set;}

		[Display(Name = "Value")]
		public string Value { get; set; }

	}

	public class BaseType
	{
		public int Id {get; set;}
		public string Code {get; set;}

		public static List<BaseType> List()
		{
			List<BaseType> types = new List<BaseType>();
			types.Add(new BaseType() { Id = 2, Code = "Binary" });
			types.Add(new BaseType(){Id = 8, Code = "Octal"});
			types.Add(new BaseType() { Id = 16, Code = "Hexadecimal" });

			return types;
		}
	}

	public abstract class ConverterBase
	{
		protected readonly int _multiple;
		protected char[] _allowValues;

		public ConverterBase(int pMultiple)
		{
			_multiple = pMultiple;
		}

		public int ConvertToInt(string pValue)
		{
			char[] values = pValue.ToUpper().ToArray();

			validate(values);

			int value = 0;
			int multiplePerPosition = 1;

			for (int i = values.Length - 1; i >= 0; i--) {
				value += multiplePerPosition * getIntValue(values[i]);
				multiplePerPosition = multiplePerPosition * _multiple;
			}

			return value;
		}

		private int getIntValue(char pValue)
		{
			switch(pValue) {
				case '0': return 0;
				case '1': return 1;
				case '2': return 2;
				case '3': return 3;
				case '4': return 4;
				case '5': return 5;
				case '6': return 6;
				case '7': return 7;
				case '8': return 8;
				case '9': return 9;
				case 'A': return 10;
				case 'B': return 11;
				case 'C': return 12;
				case 'D': return 13;
				case 'E': return 14;
				case 'F': return 15;
				default: throw new Exception("Value not mapped: " + pValue);
			}
		}

		private void validate(char[] values)
		{
			foreach(char value in values) {
				if (!_allowValues.Contains(value))
					throw new Exception("Value not allow: " + value);
			}
		}
	}

	public class ConverterBinary : ConverterBase
	{
		public ConverterBinary()
			:base(2)
		{
			_allowValues = new char[2]{'0', '1' };
		}
	}

	public class ConverterOctal : ConverterBase
	{
		public ConverterOctal()
			: base(8)
		{
			_allowValues = "012345678".ToArray();
		}
	}

	public class ConverterHexadecimal : ConverterBase
	{
		public ConverterHexadecimal()
			: base(16)
		{
			_allowValues = "0123456789ABCDEF".ToArray();
		}
	}
}
