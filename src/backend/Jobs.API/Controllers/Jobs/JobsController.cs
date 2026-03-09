using Jobs.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Jobs.API.Controllers.Jobs
{
    public class JobsController
    {
    }
}

//| Method | Route | Description |
//| ------ | ----------------------- | --------------------------- |
//| GET | `/ api / jobs`             | Get all jobs(with filters) |
//| GET | `/ api / jobs /{ id}`        | Job details |
//| POST | `/ api / jobs`             | Create job(Employer only) |
//| PUT | `/ api / jobs /{ id}`        | Update job |
//| DELETE | `/ api / jobs /{ id}`        | Delete job |
//| GET | `/ api / jobs / recommended` | Recommended jobs for user   |

