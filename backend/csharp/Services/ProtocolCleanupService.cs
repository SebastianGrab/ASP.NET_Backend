using Data;
using Helper.SearchObjects;

public class ProtocolCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(12);
    private readonly ILogger<ProtocolSearchObject> _logger;

    public ProtocolCleanupService(IServiceProvider serviceProvider, ILogger<ProtocolSearchObject> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProtocolCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            DeleteOldProtocols();
            await Task.Delay(_interval, stoppingToken);
            _logger.LogInformation("ProtocolCleanupService executed.");
        }
        
        _logger.LogInformation("ProtocolCleanupService stopped.");
    }

    private void DeleteOldProtocols()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ProtocolContext>();
            var cutoffDate = DateTime.UtcNow.AddYears(-10);
            var oldProtocols = dbContext.Protocols
                .Where(p => p.CreatedDate <= cutoffDate)
                .ToList();

            foreach (var oldProtocol in oldProtocols)
            {
                var _protocolId = oldProtocol.Id;

                var _additionalUsers = dbContext.AdditionalUsers.Where(p => p.protocolId == _protocolId).ToList();
                var _protocolContent = dbContext.ProtocolContents.Where(p => p.protocolId == _protocolId).ToList();
                var _protocolPdfFile = dbContext.ProtocolPdfFiles.Where(p => p.protocolId == _protocolId).ToList();

                dbContext.AdditionalUsers.RemoveRange(_additionalUsers);
                dbContext.Protocols.Remove(oldProtocol);
                dbContext.ProtocolContents.RemoveRange(_protocolContent);
                dbContext.ProtocolPdfFiles.RemoveRange(_protocolPdfFile);
                dbContext.SaveChanges();
            }
        }
    }
}
