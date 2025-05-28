using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;

namespace User.Domain.Repositories;

public interface IUserRepository
{
    #region User
    Task<User.Domain.Models.Response.UserResponseDTO> GetUserAsync(int id);
    #endregion
}