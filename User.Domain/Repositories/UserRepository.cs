using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;

namespace User.Domain.Repositories;

public class UserRepository : IUserRepository
{
    //z jakiego kontekstu korzysta repozytorium
    private readonly User.Domain.Repositories.DbContext _context;
    private readonly IMapper _mapper;

    //konstruktor repozytorium
    public UserRepository(User.Domain.Repositories.DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User.Domain.Models.Response.UserResponseDTO> GetUserAsync(int userId)
    {
        var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        return _mapper.Map<UserResponseDTO>(user);

    }
}
