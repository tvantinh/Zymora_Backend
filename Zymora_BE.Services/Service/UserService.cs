using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Repositories.IUnitOfWork;
using Zymora_BE.Contract.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Zymora_BE.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // check user by email or username
        public Task<bool> CheckUserExists(string email, string UserName)
        {
            return _unitOfWork.GetGenericRepository<User>().Entities.AnyAsync(u => u.Email == email || u.UserName == UserName);
        }
        // check user by id
        public async Task<User?> CheckUserExistsByUserName(string UserName)
        {
            return await _unitOfWork.GetGenericRepository<User>().Entities.FirstOrDefaultAsync(u => u.UserName == UserName);
        }

        public Task<User> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetAll()
        {
            return _unitOfWork.GetGenericRepository<User>().GetAllAsync();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
    
}
