using Dolcecuore.Application.Common.Commands;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Basket.Entities;

namespace Dolcecuore.Services.Basket.Commands;

public record AddAuditLogEntryCommand(AuditLogEntry AuditLogEntry) : ICommand;

public class AddAuditLogEntryCommandHandler : ICommandHandler<AddAuditLogEntryCommand>
{
    private readonly IRepository<AuditLogEntry, Guid> _auditLogRepository;
    private readonly IRepository<EventLog, long> _eventLogRepository;
    private readonly ICurrentUser _currentUser;

    public AddAuditLogEntryCommandHandler(
        IRepository<AuditLogEntry, Guid> auditLogRepository,
        IRepository<EventLog, long> eventLogRepository,
        ICurrentUser currentUser)
    {
        _auditLogRepository = auditLogRepository;
        _eventLogRepository = eventLogRepository;
        _currentUser = currentUser;
    }

    public async Task HandleAsync(AddAuditLogEntryCommand command, CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLogEntry
        {
            UserId = command.AuditLogEntry.UserId,
            CreatedDateTime = command.AuditLogEntry.CreatedDateTime,
            Action = command.AuditLogEntry.Action,
            ObjectId = command.AuditLogEntry.ObjectId,
            Log = command.AuditLogEntry.Log,
        };

        await _auditLogRepository.AddOrUpdateAsync(auditLog, cancellationToken);
        await _auditLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        await _eventLogRepository.AddOrUpdateAsync(new EventLog
        {
            EventType = "AUDIT_LOG_ENTRY_CREATED",
            TriggeredById = _currentUser.UserId, 
            CreatedDateTime = auditLog.CreatedDateTime,
            ObjectId = auditLog.Id.ToString(),
            Message = auditLog.AsJsonString(),
            Published = false,
        }, cancellationToken);

        await _eventLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}