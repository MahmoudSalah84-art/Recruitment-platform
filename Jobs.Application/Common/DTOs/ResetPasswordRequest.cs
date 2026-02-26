using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.DTOs
{
	public record ResetPasswordRequest(string Email, string Token, string NewPassword) { }

}
