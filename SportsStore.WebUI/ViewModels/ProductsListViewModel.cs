using System.Collections.Generic;

namespace SportsStore.WebUI.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductDisplayViewModel> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}