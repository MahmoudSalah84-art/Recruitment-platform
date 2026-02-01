using Jobs.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Serilog;

namespace Jobs.API.Controllers.Auth
{
    public class AuthController
    {
    }
}



//| Method |          Route              |    Description       |
//| ------ | --------------------------- | -------------------- |
//| POST   | `/ api / auth / register`   | Register new user    |
//| POST   | `/ api / auth / login`      | Login and get token  |
//| POST   | `/api/auth/refresh-token`   | Refresh JWT token    |
//| POST   | `/api/auth/logout`          | Revoke refresh token |
//| POST   | `/api/auth/forgot-password` | Send reset link      |
//| POST   | `/api/auth/reset-password`  | Reset password       |
//| POST   | `/api/auth/verify-email`    | Verify email         |
