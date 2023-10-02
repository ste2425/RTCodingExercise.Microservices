
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catalog.API.Controllers
{
    public class PlatesRepository : IPlatesRepository
    {
        private const int RECORDS_PER_PAGE = 10;

        private readonly ApplicationDbContext db;
        public PlatesRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public PaginatedResponse<Plate> GetPlates(int page, string sort, string filter)
        {
            var query = db.Plates.AsQueryable();

            query = ApplySort(query, sort);

            query = ApplyFilter(query, filter);

            return ApplyPagination(query, page);
        }

        private PaginatedResponse<Plate> ApplyPagination(IQueryable<Plate> query, int page)
        {
            // This could be put somewhere more generic and shared accross the system
            // It could allow any data access to be paginated
            var totalRecords = query.Count();
            var totalPages = totalRecords / RECORDS_PER_PAGE;
            var skipRecords = RECORDS_PER_PAGE * page;

            // Skip/Take pagination is not the most efficient
            // KeySet may be better however it involves a different user experience
            var results = query
                .Skip(skipRecords)
                .Take(RECORDS_PER_PAGE)
                .ToList();

            return new PaginatedResponse<Plate>(results, page, RECORDS_PER_PAGE, totalPages);
        }

        private IQueryable<Plate> ApplyFilter(IQueryable<Plate> query, string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return query;

            // neat algo for doing fuzzy search, scalar function is added in DB via seed script
            // could allow the user to select how forgiving the match is in the UI maybe
            return query.Where(x => db.Levenshtein(x.Registration, filter) <= 3);
        }

        private IQueryable<Plate> ApplySort(IQueryable<Plate> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query;

            var parts = sort.Split(',');
            var field = parts[0];
            var direction = parts[1];

            // EF.property will auto create shadow properties if it does not exist
            // Validation should be applied anyway as were using un-santized input from the UI
            switch (direction)
            {
                case "asc":
                    return query.OrderBy(x => EF.Property<object>(x, field));
                case "desc":
                    return query.OrderByDescending(x => EF.Property<object>(x, field));
                default:
                    return query;
            }
        }
    }
}