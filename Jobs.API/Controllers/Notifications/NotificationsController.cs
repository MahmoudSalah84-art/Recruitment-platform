using MediatR;
using System;

namespace Jobs.API.Controllers.Notifications
{
    public class NotificationsController
    {
    }
}

//| Method | Route | Description |
//| ------ | ------------------------------ | ------------------------- |
//| GET | `/ api / notifications`           | Get notifications |
//| PUT | `/ api / notifications /{ id}/ read` | Mark notification as read |
//| PUT | `/ api / notifications / read - all`  | Mark all as read |
//| DELETE | `/ api / notifications /{ id}`      | Delete notification |

////GET / api / notifications
////GET / api / notifications /{ id}
////PATCH / api / notifications /{ id}/ read
////DELETE / api / notifications /{ id}
