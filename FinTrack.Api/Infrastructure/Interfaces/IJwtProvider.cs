using FinTrack.Models;

namespace FinTrack.Api.Infrastructure.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
