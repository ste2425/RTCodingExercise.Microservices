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
                UserName = "joydipkanjilal",
                Password = "test1234",
                Role = "manager"
            });
            users.Add(new UserDTO
            {
                UserName = "michaelsanders",
                Password = "test1234",
                Role = "developer"
            });
            users.Add(new UserDTO
            {
                UserName = "stephensmith",
                Password = "test1234",
                Role = "tester"
            });
            users.Add(new UserDTO
            {
                UserName = "rodpaddock",
                Password = "test1234",
                Role = "admin"
            });
            users.Add(new UserDTO
            {
                UserName = "rexwills",
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