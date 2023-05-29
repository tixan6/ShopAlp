using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopAlp.Models
{
    public class Pager
    {
        public int TotalItems { get; private set; }
        public int CurrntPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPage { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public Pager() { }
        public Pager(int totalItems, int page, int pageSize = 10) 
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int correntPage = page;

            int startPage = - 5;
            int endPage = + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalPages;
            CurrntPage = correntPage;
            PageSize = pageSize;
            TotalPage = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
    
}
