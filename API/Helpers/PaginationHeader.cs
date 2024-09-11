using System;

namespace API.Helpers;

public class PaginationHeader(int currentPage,int itemPerPage,int totalItems,int totalPages)
{
    public int CurrentPage { get; set; } = currentPage;
    public int itemPerPage { get; set; } = itemPerPage;
    public int TotalPages { get; set; } =totalPages;
    public int totalItems { get; set; } = totalItems;
}
