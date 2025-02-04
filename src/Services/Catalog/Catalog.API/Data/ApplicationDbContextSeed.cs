﻿using Newtonsoft.Json;

namespace Catalog.API.Data
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env, ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                await SeedFuzzySearch(context, logger);
                await SeedCustomData(context, env, logger);
            }
            catch (Exception ex)
            {
                // used for initilisaton of docker containers
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex.Message, $"There is an error migrating data for ApplicationDbContext");

                    await SeedAsync(context, env, logger, settings, retryForAvaiability);
                }
            }
        }

        // This could be expanded upon to make it a more generic solution for seeding any scalar functions
        public async Task SeedFuzzySearch(ApplicationDbContext context, ILogger<ApplicationDbContextSeed> logger)
        {
            try
            {
                string sql = @"
/* 
	Levenshtein Edit Distance Algorithm implementation taken from
	https://www.sqlteam.com/forums/topic.asp?TOPIC_ID=51540
*/
CREATE OR ALTER FUNCTION levenshtein(@s1 nvarchar(3999), @s2 nvarchar(3999))
RETURNS int
AS
BEGIN
 DECLARE @s1_len int, @s2_len int
 DECLARE @i int, @j int, @s1_char nchar, @c int, @c_temp int
 DECLARE @cv0 varbinary(8000), @cv1 varbinary(8000)

 SELECT
  @s1_len = LEN(@s1),
  @s2_len = LEN(@s2),
  @cv1 = 0x0000,
  @j = 1, @i = 1, @c = 0

 WHILE @j <= @s2_len
  SELECT @cv1 = @cv1 + CAST(@j AS binary(2)), @j = @j + 1

 WHILE @i <= @s1_len
 BEGIN
  SELECT
   @s1_char = SUBSTRING(@s1, @i, 1),
   @c = @i,
   @cv0 = CAST(@i AS binary(2)),
   @j = 1

  WHILE @j <= @s2_len
  BEGIN
   SET @c = @c + 1
   SET @c_temp = CAST(SUBSTRING(@cv1, @j+@j-1, 2) AS int) +
    CASE WHEN @s1_char = SUBSTRING(@s2, @j, 1) THEN 0 ELSE 1 END
   IF @c > @c_temp SET @c = @c_temp
   SET @c_temp = CAST(SUBSTRING(@cv1, @j+@j+1, 2) AS int)+1
   IF @c > @c_temp SET @c = @c_temp
   SELECT @cv0 = @cv0 + CAST(@c AS binary(2)), @j = @j + 1
 END

 SELECT @cv1 = @cv0, @i = @i + 1
 END

 RETURN @c
END";

                await context.Database.ExecuteSqlRawAsync(sql);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task SeedCustomData(ApplicationDbContext context, IWebHostEnvironment env, ILogger<ApplicationDbContextSeed> logger)
        {
            try
            {
                var plates = ReadApplicationRoleFromJson(env.ContentRootPath, logger);

                await context.Plates.AddRangeAsync(plates);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public List<Plate> ReadApplicationRoleFromJson(string contentRootPath, ILogger<ApplicationDbContextSeed> logger)
        {
            string filePath = Path.Combine(contentRootPath, "Setup", "plates.json");
            string json = File.ReadAllText(filePath);

            var plates = JsonConvert.DeserializeObject<List<Plate>>(json) ?? new List<Plate>();

            return plates;
        }
    }
}
