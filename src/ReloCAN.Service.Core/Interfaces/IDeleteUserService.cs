using Ardalis.Result;

namespace ReloCAN.Service.Core.Interfaces;

public interface IDeleteUserService
{
    public Task<Result> DeleteUser(int userId);
}
