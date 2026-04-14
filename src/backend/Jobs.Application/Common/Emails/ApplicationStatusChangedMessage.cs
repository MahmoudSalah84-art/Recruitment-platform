using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Common.Emails
{
	public record ApplicationStatusChangedMessage(
	string To,
	string UserName,
	string JobTitle,
	string CompanyName,
	string Status,
	string? Note) : EmailMessage(To, $"Application Update: {Status}")
	{
		public override string BuildBody()
		{
			var (color, emoji) = Status switch
			{
				"Accepted" => ("#27ae60", "🎉"),
				"Rejected" => ("#e74c3c", "😔"),
				_ => ("#3498db", "📋")
			};

			return $"""
            <html><body style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
              <div style="background:#f4f4f4;padding:30px;border-radius:8px">
                <h2 style="color:{color}">Application Update {emoji}</h2>
                <p>Hi {UserName}, your application for <strong>{JobTitle}</strong> at <strong>{CompanyName}</strong> has been updated.</p>
                <p>Status: <strong style="color:{color}">{Status}</strong></p>
                {(Note is not null ? $"<p>Note: {Note}</p>" : "")}
              </div>
            </body></html>
            """;
		}
	}
}
