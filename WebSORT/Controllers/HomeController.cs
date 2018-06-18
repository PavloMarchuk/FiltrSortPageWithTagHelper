using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSORT.Models;
using WebSORT.Models.ForPagination;
using WebSORT.Models.SortFilterPaging;


//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebSORT.Models;
//using System.Linq;

namespace WebSORT.Controllers
{
	public class HomeController : Controller
	{
		UsersContext db;
		public HomeController(UsersContext context)
		{
			this.db = context;
		}
		public async Task<IActionResult> Index(SortState sortOrder = SortState.NameAsc)
		{
			IQueryable<User> users = db.Users.Include(x => x.Company);

			switch (sortOrder)
			{
				case SortState.NameDesc:
					users = users.OrderByDescending(s => s.Name);
					break;
				case SortState.AgeAsc:
					users = users.OrderBy(s => s.Age);
					break;
				case SortState.AgeDesc:
					users = users.OrderByDescending(s => s.Age);
					break;
				case SortState.CompanyAsc:
					users = users.OrderBy(s => s.Company.Name);
					break;
				case SortState.CompanyDesc:
					users = users.OrderByDescending(s => s.Company.Name);
					break;
				default:
					users = users.OrderBy(s => s.Name);
					break;
			}
			IndexViewModel viewModel = new IndexViewModel
			{
				Users = await users.AsNoTracking().ToListAsync(),
				SortViewModel = new WebSORT.Models.SortViewModel(sortOrder)
			};
			return View(viewModel);
		}

		public ActionResult About(int? company, string name)
		{
			IQueryable<User> users = db.Users.Include(p => p.Company);
			if (company != null && company != 0)
			{
				users = users.Where(p => p.CompanyId == company);
			}
			if (!String.IsNullOrEmpty(name))
			{
				users = users.Where(p => p.Name.Contains(name));
			}

			List<Company> companies = db.Companies.ToList();
			// устанавливаем начальный элемент, который позволит выбрать всех
			companies.Insert(0, new Company { Name = "Все", Id = 0 });

			UsersListViewModel viewModel = new UsersListViewModel
			{
				Users = users.ToList(),
				Companies = new SelectList(companies, "Id", "Name"),
				Name = name
			};
			return View(viewModel);
		}

		public async Task<IActionResult> Pagination(int page = 1)
		{
			int pageSize = 3;   // количество элементов на странице

			IQueryable<User> source = db.Users.Include(x => x.Company);
			var count = await source.CountAsync();
			var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
			IndexViewModelPagination viewModel = new IndexViewModelPagination
			{
				PageViewModel = pageViewModel,
				Users = items
			};
			return View(viewModel);
		}
		public async Task<IActionResult> PaginationWithHelper(int page = 1)
		{
			int pageSize = 3;
			IQueryable<User> source = db.Users.Include(x => x.Company);
			var count = await source.CountAsync();
			var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
			IndexViewModelPagination viewModel = new IndexViewModelPagination
			{
				PageViewModel = pageViewModel,
				Users = items
			};
			return View(viewModel);
		}


		public async Task<IActionResult> SortFilterPaging(int? company, string name, int page = 1,
			SortState sortOrder = SortState.NameAsc)
		{
			int pageSize = 3;

			//фильтрация
			IQueryable<User> users = db.Users.Include(x => x.Company);

			if (company != null && company != 0)
			{
				users = users.Where(p => p.CompanyId == company);
			}
			if (!String.IsNullOrEmpty(name))
			{
				users = users.Where(p => p.Name.Contains(name));
			}

			// сортировка
			switch (sortOrder)
			{
				case SortState.NameDesc:
					users = users.OrderByDescending(s => s.Name);
					break;
				case SortState.AgeAsc:
					users = users.OrderBy(s => s.Age);
					break;
				case SortState.AgeDesc:
					users = users.OrderByDescending(s => s.Age);
					break;
				case SortState.CompanyAsc:
					users = users.OrderBy(s => s.Company.Name);
					break;
				case SortState.CompanyDesc:
					users = users.OrderByDescending(s => s.Company.Name);
					break;
				default:
					users = users.OrderBy(s => s.Name);
					break;
			}

			// пагинация
			var count = await users.CountAsync();
			var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			// формируем модель представления
			SortFilterPagingVM viewModel = new SortFilterPagingVM
			{
				PageViewModel = new PageViewModel(count, page, pageSize),
				SortViewModel = new Models.SortFilterPaging.SortViewModel(sortOrder),
				FilterViewModel = new FilterViewModel(db.Companies.ToList(), company, name),
				Users = items
			};
			return View(viewModel);
		}

		public async Task<IActionResult> FiltrSortPageWithHelper(int? company, string name, int page = 1,
			SortState sortOrder = SortState.NameAsc)
		{
			int pageSize = 3;

			//фильтрация
			IQueryable<User> users = db.Users.Include(x => x.Company);

			if (company != null && company != 0)
			{
				users = users.Where(p => p.CompanyId == company);
			}
			if (!String.IsNullOrEmpty(name))
			{
				users = users.Where(p => p.Name.Contains(name));
			}

			// сортировка
			switch (sortOrder)
			{
				case SortState.NameDesc:
					users = users.OrderByDescending(s => s.Name);
					break;
				case SortState.AgeAsc:
					users = users.OrderBy(s => s.Age);
					break;
				case SortState.AgeDesc:
					users = users.OrderByDescending(s => s.Age);
					break;
				case SortState.CompanyAsc:
					users = users.OrderBy(s => s.Company.Name);
					break;
				case SortState.CompanyDesc:
					users = users.OrderByDescending(s => s.Company.Name);
					break;
				default:
					users = users.OrderBy(s => s.Name);
					break;
			}

			// пагинация
			var count = await users.CountAsync();
			var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			// формируем модель представления
			SortFilterPagingVM viewModel = new SortFilterPagingVM
			{
				PageViewModel = new PageViewModel(count, page, pageSize),
				SortViewModel = new Models.SortFilterPaging.SortViewModel(sortOrder),
				FilterViewModel = new FilterViewModel(db.Companies.ToList(), company, name),
				Users = items
			};
			return View(viewModel);
		}
	}
}
