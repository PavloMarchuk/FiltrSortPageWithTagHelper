using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSORT.Models
{
    public class IndexViewModel
    {
		public IEnumerable<User> Users { get; set; }
		public SortViewModel SortViewModel { get; set; }
	}
}
