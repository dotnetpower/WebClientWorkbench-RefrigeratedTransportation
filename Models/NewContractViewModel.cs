using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace WebClientWorkbench.Models
{
    public class NewContractViewModel
    {
		public string Device { get; set; }
		public string Owner { get; set; }
		public string Observer { get; set; }
		public double MinHumidity { get; set; }
		public double MaxHumidity { get; set; }
		public double MinTemperature { get; set; }
		public double MaxTemperature { get; set; }
	}
}
