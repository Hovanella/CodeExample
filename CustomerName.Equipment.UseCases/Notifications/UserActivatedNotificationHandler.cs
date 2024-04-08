using MediatR;
using Microsoft.Extensions.Logging;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.ActivateEquipmentUser;
using CustomerName.Portal.Identity.Contract.Events;

namespace CustomerName.Portal.Equipment.UseCases.Notifications;

internal class UserActivatedNotificationHandler : INotificationHandler<UserActivatedNotification>
{
    private readonly IActivateEquipmentUserDbQuery _activateEquipmentUserDbQuery;
    private readonly ILogger<UserActivatedNotificationHandler> _logger;

    public UserActivatedNotificationHandler(
        ILogger<UserActivatedNotificationHandler> logger,
        IActivateEquipmentUserDbQuery activateEquipmentUserDbQuery)
    {
        _logger = logger;
        _activateEquipmentUserDbQuery = activateEquipmentUserDbQuery;
    }

    public async Task Handle(UserActivatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Booking] Received User Activated Event with ID {EventId}. User Id: {UserId}",
            notification.EventId, notification.UserId);

        var request = new ActivateEquipmentUserRequest(notification.UserId);

        await _activateEquipmentUserDbQuery.ActivateUserAsync(request, cancellationToken);

        _logger.LogInformation("[Booking] Completed User Activated Event with ID {EventId}", notification.EventId);
    }
}
