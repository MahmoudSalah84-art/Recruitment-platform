using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace Jobs.API.Controllers.Security
{
    public class SessionsController
    {
    }
}

//| Method | Route | Description |
//| ------ | ------------------------------- | --------------- |
//| GET | `/ api / security / sessions`        | Active sessions |
//| DELETE | `/ api / security / sessions /{ id}`   | Revoke session |
//| PUT | `/ api / security / change - password` | Change password |

//GET / api / security / sessions
//DELETE / api / security / sessions /{ id}
