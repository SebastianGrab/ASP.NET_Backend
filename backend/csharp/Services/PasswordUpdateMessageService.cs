using Data;
using Models;

public class PasswordUpdateMessageService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromDays(7);
    private readonly ILogger<UserMessage> _logger;

    public PasswordUpdateMessageService(IServiceProvider serviceProvider, ILogger<UserMessage> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PasswordUpdateMessageService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            CreateProtocolUpdateMessage();
            await Task.Delay(_interval, stoppingToken);
            _logger.LogInformation("PasswordUpdateMessageService executed.");
        }
        
        _logger.LogInformation("ProtocolCleanupService stopped.");
    }

    private void CreateProtocolUpdateMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ProtocolContext>();
            var usersToUpdate = dbContext.Users
                .Where(u => (DateTime.UtcNow - u.LastPasswordChangeDate).TotalDays > 365)
                .ToList();

            foreach (var userToUpdate in usersToUpdate)
            {
                var newUserMessage = new UserMessage
                {
                    Subject = "Bitte ändern Sie Ihr Passwort.",
                    MessageContent = "Ihre letzte Passwortänderung ist lange her. Bitte erstellen Sie ein neues Passwort.",
                    ReferenceObject = "User",
                    ReferenceObjectId = userToUpdate.Id,
                    SentAt = DateTime.UtcNow,
                    SentFrom = "System",
                    IsRead = false,
                    IsArchived = false,
                    userId = userToUpdate.Id,
                    User = userToUpdate
                };

                dbContext.UserMessages.Add(newUserMessage);
                dbContext.SaveChanges();
            }
        }
    }
}
