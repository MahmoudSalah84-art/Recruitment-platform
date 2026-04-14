using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.Emails
{
	public record PasswordResetMessage(
	string To,
	string UserName,
	string ResetLink) : EmailMessage (To, "Password Reset Request")
	{
		public override string BuildBody() => $"""
        <html><body style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
          <div style="background:#f4f4f4;padding:30px;border-radius:8px">
            <h2 style="color:#2c3e50">Password Reset Request</h2>
            <p>Hi {UserName}, we received a request to reset your password.</p>
            <a href="{ResetLink}"
               style="background:#e74c3c;color:white;padding:12px 24px;
                      text-decoration:none;border-radius:5px;display:inline-block;margin:16px 0">
              Reset Password
            </a>
            <p style="color:#999;font-size:12px">This link expires in 1 hour. If you did not request this, ignore this email.</p>
          </div>
        </body></html>
        """;
	}
}
