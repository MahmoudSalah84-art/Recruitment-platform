using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jobs.API.Controllers.Auth
{
    public class ExternalAuthController
    {
    }
}

//| Method | Route                      | Description        |
//| ------ | --------------------       | ------------------- |
//| POST   | `/ api / auth / google`    | Login with Google   |
//| POST   | `/api/auth/facebook`       | Login with Facebook |
//| POST   | `/api/auth/linkedin`       | Login with LinkedIn |
