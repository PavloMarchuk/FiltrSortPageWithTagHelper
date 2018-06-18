using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebSORT.Models.SortFilterPaging
{
    public class FilterViewModel
    {
		public FilterViewModel(List<Company> companies, int? company, string name)
		{
			// устанавливаем начальный элемент, который позволит выбрать всех
			companies.Insert(0, new Company { Name = "Все", Id = 0 });
			Companies = new SelectList(companies, "Id", "Name", company);
			SelectedCompany = company;
			SelectedName = name;
		}
		public SelectList Companies { get; private set; } // список компаний
		public int? SelectedCompany { get; private set; }   // выбранная компания
		public string SelectedName { get; private set; }    // введенное имя
	}
}
