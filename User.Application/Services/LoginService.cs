using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;

namespace User.Application.Services;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    protected Queue<int> _userLoggedIdsQueue;

    public LoginService(IJwtTokenService jwtTokenService, Queue<int> userLoggedIdsQueue )
    {
        _jwtTokenService = jwtTokenService;
        _userLoggedIdsQueue = userLoggedIdsQueue;
    }

    public string Login(string username, string password)
    {
        if (username == "admin" && password == "haslo")
        {
            var roles = new List<string> { "Client", "Employee", "Administrator" };
            var token = _jwtTokenService.GenerateToken(123, roles);
            _userLoggedIdsQueue.Enqueue(123);
            return token;
        }
        else if (username == "employee" && password == "haslo")
        {
            var roles = new List<string> { "Client", "Employee" };
            var token = _jwtTokenService.GenerateToken(456, roles);
            _userLoggedIdsQueue.Enqueue(456);
            return token;
        }
        else if (username == "client" && password == "haslo")
        {
            var roles = new List<string> { "Client" };
            var token = _jwtTokenService.GenerateToken(789, roles);
            _userLoggedIdsQueue.Enqueue(789);
            return token;
        }
        else
        {
            throw new InvalidCredentialsException();
        }
    }
}
