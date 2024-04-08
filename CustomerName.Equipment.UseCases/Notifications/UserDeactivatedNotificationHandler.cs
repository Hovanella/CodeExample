using MediatR;
using Microsoft.Extensions.Logging;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.DeleteEquipmentUser;
using CustomerName.Portal.Identity.Contract.Events;

namespace CustomerName.Portal.Equipment.UseCases.Notifications;

public class UserDeactivatedNotificationHandler : INotificationHandler<UserDeactivatedNotification>
{
    private readonly ILogger<UserDeactivatedNotificationHandler> _logger;
    private readonly IDeactivateEquipmentUserDbQuery _deleteBookingUserDbQuery;

    public UserDeactivatedNotificationHandler(ILogger<UserDeactivatedNotificationHandler> logger, IDeactivateEquipmentUserDbQuery deleteBookingUserDbQuery)
    {
        _logger = logger;
        _deleteBookingUserDbQuery = deleteBookingUserDbQuery;
    }

    public async Task Handle(UserDeactivatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Equipment] Received User Deleted Event with ID {EventId}. User Id: {UserId}",
            notification.EventId, notification.UserId);

        var bookingUser = new DeactivateEquipmentUserDbQueryRequest(notification.UserId);

        await _deleteBookingUserDbQuery.DeactivateUserAsync(bookingUser, cancellationToken);

        _logger.LogInformation("[Equipment] Completed User Deleted Event with ID {EventId}", notification.EventId);
    }
}
