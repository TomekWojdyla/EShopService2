using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Identity;
using User.Domain.Models.Response;
using User.Domain.Models.Entities;
using User.Domain.Repositories;

namespace User.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private IUserRepository _userRepository;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async UserResponseDTO GetUser(int userId)
    {
        var user = await _userRepository.GetUserAsync(userId);

        //var user = new User.Domain.Models.Entities.User()
        //{
        //    Username = "aaa",
        //    PasswordHash = "bbb",
        //    IsActive = true,
        //    Id = userId,
        //    Email = "User@email.com"
        //};

        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        return _mapper.Map<UserResponseDTO>(user);
    }

    UserResponseDTO IUserService.GetUser(int UserId)
    {
        throw new NotImplementedException();
    }
}
