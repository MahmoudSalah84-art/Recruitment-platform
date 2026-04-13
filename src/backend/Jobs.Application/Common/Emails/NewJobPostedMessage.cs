using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.Emails
{
	public record NewJobPostedMessage(
	string To,
	string UserName,
	string JobTitle,
	string CompanyName,
	string JobLink) : EmailMessage(To, $"New Job Match: {JobTitle} at {CompanyName}")
	{
		public override string BuildBody() => $"""
        <html><body style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
          <div style="background:#f4f4f4;padding:30px;border-radius:8px">
            <h2 style="color:#2c3e50">New Job Match Found! 🔍</h2>
            <p>Hi {UserName}, a new job matching your profile has been posted.</p>
            <p><strong>{JobTitle}</strong> at <strong>{CompanyName}</strong></p>
            <a href="{JobLink}"
               style="background:#3498db;color:white;padding:12px 24px;
                      text-decoration:none;border-radius:5px;display:inline-block;margin:16px 0">
              View Job
            </a>
          </div>
        </body></html>
        """;
	}
}
