using Jobs.Domain.Entities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Jobs.API.Controllers.Companies
{
    public class CompaniesController
    {
    }
}

//| Method | Route                       | Description |
//| ------ | ---------------------      | ------------------- |
//| GET    | `/ api / companies`        | List companies |
//| GET    | `/ api / companies /{ id}` | Get company details |
//| POST   | `/api/companies`           | Create company      |
//| PUT    | `/api/companies/{id}`      | Update company |
//| DELETE | `/ api / companies /{ id}` | Delete company |

//GET / api / companies
//GET / api / companies /{ id}
//POST / api / companies
//PUT / api / companies /{ id}
