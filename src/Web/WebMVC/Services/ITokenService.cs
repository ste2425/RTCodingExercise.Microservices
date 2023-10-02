using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDTO user);
    }
}