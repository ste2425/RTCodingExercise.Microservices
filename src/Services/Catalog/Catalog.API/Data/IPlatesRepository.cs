namespace Catalog.API.Controllers
{
    public interface IPlatesRepository
    {
        PaginatedResponse<Plate> GetPlates(int page, string sort, string filter);
    }
}