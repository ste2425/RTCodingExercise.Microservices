using Catalog.API.Models;

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
        public Task<PaginatedResponse<Plate>> Index([FromQuery] int? page, [FromQuery] string? sort, [FromQuery] string? filter)
        {
            return repo.GetPlates(page ?? 0, sort ?? string.Empty, filter ?? string.Empty);
        }

        [HttpDelete]
        [Route("{plateId}")]
        public async Task<IActionResult> Delete(Guid plateId)
        {
            var deleted = await repo.DeletePlate(plateId);

            if (!deleted)
                return NotFound();
            else
                return Ok();
        }

        [HttpPut]
        [Route("{plateId}")]
        public async Task<IActionResult> Update([FromRoute] Guid plateId, [FromBody] PlateModel plate)
        {
            var updatedPlate = await repo.UpdatePlate(plateId, plate);

            if (updatedPlate == null)
                return NotFound();
            else
                return Ok(updatedPlate);
        }

        [HttpPost]
        public Task<Plate> Create([FromBody] PlateModel plate)
        {
            return repo.CreatePlate(plate);
        }
    }
}
