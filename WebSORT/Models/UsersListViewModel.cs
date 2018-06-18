using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSORT.Models
{
    public class UsersListViewModel
    {
		public IEnumerable<User> Users { get; set; }
		public SelectList Companies { get; set; }
		public string Name { get; set; }
	}
}
