using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace affiliate_proj;

public class NoOpEmailSender : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink) 
        => Task.CompletedTask;

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) 
        => Task.CompletedTask;

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) 
        => Task.CompletedTask;
}