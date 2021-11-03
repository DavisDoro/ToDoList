using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Data
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
    }
}
