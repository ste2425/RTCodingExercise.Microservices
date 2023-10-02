using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Services
{
    public interface IUserRepository
    {
        UserDTO GetUser(UserModel userMode);
    }
}