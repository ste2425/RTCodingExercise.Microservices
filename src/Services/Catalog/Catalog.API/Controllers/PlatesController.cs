namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("plates")]
    public class PlateController : Controller
    {
        private readonly IPlatesRepository repo;
        public PlateController(IPlatesRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public PaginatedResponse<Plate> Index([FromQuery] int? page, [FromQuery] string? sort, [FromQuery] string? filter)
        {
            return this.repo.GetPlates(page ?? 0, sort ?? string.Empty, filter ?? string.Empty);
        }
    }
}
