namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IEmailService
	{
		Task SendEmailVerificationAsync(string email, string userName, string verificationToken);
		Task SendPasswordResetAsync(string email, string userName, string resetToken);
		Task SendWelcomeEmailAsync(string email, string userName);
		Task SendPasswordChangedNotificationAsync(string email, string userName);
	}
}
