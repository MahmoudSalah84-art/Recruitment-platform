using System;
using static Azure.Core.HttpHeader;

namespace Jobs.API.Controllers.common
{
    public class CommonController
    {
    }
}

//| Method |        Route                     | Description           |
//| ------ | -------------------------------- | --------------------- |
//| GET    | `/ api / common / countries`     | Get countries list    |
//| GET    | `/api/common/cities/{countryId}` | Get cities by country |
//| GET    | `/api/common/skills`             | List skills           |
//| GET    | `/api/common/categories`         | Job categories        |


//GET / api / common / app - info
//GET / api / common / enums
