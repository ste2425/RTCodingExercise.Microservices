using Catalog.API.Models;

namespace Catalog.API.Controllers
{
    public interface IPlatesRepository
    {
        Task<PaginatedResponse<Plate>> GetPlates(int page, string sort, string filter);

        Task<bool> DeletePlate(Guid plateId);

        Task<Plate> CreatePlate(PlateModel plate);

        Task<Plate?> UpdatePlate(Guid id, PlateModel plate);
    }
}