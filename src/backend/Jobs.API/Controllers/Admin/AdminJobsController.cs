using System;
using System.Collections.Generic;

namespace Jobs.API.Controllers.AdminController
{
    public class AdminJobsController
    {
    }
}

//| Method |         Route                  | Description         |
//| ------ | ------------------------------ | ------------------- |
//| GET    | `/ api / admin / jobs`         | List all jobs       |
//| PUT    | `/api/admin/jobs/{id}/ approve`| Approve job posting |
//| PUT    | `/api/admin/jobs/{id}/ reject` | Reject job posting  |
//| DELETE | `/api/admin/jobs/{id}`         | Force delete job    |
