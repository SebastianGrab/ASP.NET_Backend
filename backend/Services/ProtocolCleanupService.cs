using Data;

public class ProtocolCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(4);

    public ProtocolCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DeleteOldProtocols();
            await Task.Delay(_interval, stoppingToken);
        }
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
