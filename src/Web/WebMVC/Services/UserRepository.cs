using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Services
{
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
                Role = "manager"
            });
            users.Add(new UserDTO
            {
                Name = "Banjo Kazooie",
                UserName = "Banjo",
                Password = "test1234",
                Role = "developer"
            });
            users.Add(new UserDTO
            {
                Name = "Crash Bandicoot",
                UserName = "crash",
                Password = "test1234",
                Role = "admin"
            });
        }
        public UserDTO GetUser(UserModel userModel)
        {
            return users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                && x.Password == userModel.Password).FirstOrDefault();
        }
    }
}