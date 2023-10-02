namespace Catalog.API.Controllers
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int RecordsPerPage { get; private set; }

        public PaginatedResponse(List<T> data, int currentPage, int recordsPerPage, int totalPages)
        {
            Data = data;
            CurrentPage = currentPage;
            RecordsPerPage = recordsPerPage;
            TotalPages = totalPages;
        }
    }
}