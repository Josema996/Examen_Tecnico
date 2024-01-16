using System.Collections.Generic;
using System.Threading.Tasks;
using ApiDevBP.Models;

public interface IUserService
{
    Task<bool> SaveUser(UserModel user);
    Task<IEnumerable<UserModel>> GetUsers();
    Task<bool> UpdateUser(int id, UserModel updatedUser);
    Task<bool> DeleteUser(int id);
}