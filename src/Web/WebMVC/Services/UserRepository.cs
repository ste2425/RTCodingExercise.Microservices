using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Services
{
    public class Roles
    {
        public static string HeadOfComercialOps = "HCO";
        public static string HeadOfMarketing = "HM";
        public static string SalesDirector = "SD";
        public static string User = "USER";
    }

    public class UserRepository : IUserRepository
    {
        private readonly List<UserDTO> users = new List<UserDTO>();

        public UserRepository()
        {
            users.Add(new UserDTO
            {
                Name = "Ben Andrews",
                UserName = "ben",
                Password = "test1234",
                Role = Roles.HeadOfComercialOps
            });
            users.Add(new UserDTO
            {
                Name = "Banjo Kazooie",
                UserName = "Banjo",
                Password = "test1234",
                Role = Roles.HeadOfMarketing
            });
            users.Add(new UserDTO
            {
                Name = "Crash Bandicoot",
                UserName = "crash",
                Password = "test1234",
                Role = Roles.SalesDirector
            });
            users.Add(new UserDTO
            {
                Name = "Spyro the Dragon",
                UserName = "spyro",
                Password = "test1234",
                Role = Roles.User
            });
        }
        public UserDTO GetUser(UserModel userModel)
        {
            return users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                && x.Password == userModel.Password).FirstOrDefault();
        }
    }
}