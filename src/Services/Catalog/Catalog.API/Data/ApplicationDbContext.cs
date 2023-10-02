namespace Catalog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Plate> Plates { get; set; }


        [DbFunction("levenshtein", "DBO")]
        public int Levenshtein(string s1, string s2)
        {
            throw new NotImplementedException();
        }
    }
}
