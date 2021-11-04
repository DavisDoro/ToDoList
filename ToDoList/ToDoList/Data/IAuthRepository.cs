using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Data
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string email);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
