using MediatR;
using Microsoft.Extensions.Logging;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Identity.Contract.Events;

namespace CustomerName.Portal.Equipment.UseCases.Notifications;

internal class UserUpdatedNotificationHandler : INotificationHandler<UserUpdatedNotification>
{
    private readonly IEquipmentUserRepository _equipmentUserRepository;
    private readonly ILogger<UserUpdatedNotificationHandler> _logger;

    public UserUpdatedNotificationHandler(
        IEquipmentUserRepository equipmentUserRepository,
        ILogger<UserUpdatedNotificationHandler> logger)
    {
        _equipmentUserRepository = equipmentUserRepository;
        _logger = logger;
    }

    public async Task Handle(
        UserUpdatedNotification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[Equipment] Received User Updated Event with ID {EventId}. User Id: {UserId}",
            notification.EventId,
            notification.UserId);

        var bookingUser = new EquipmentUser
        {
            UserId = notification.UserId,
            FirstName = notification.FirstName,
            LastName = notification.LastName,
            DepartmentId = notification.DepartmentId,
            Email = notification.Email
        };

        await _equipmentUserRepository.CreateOrUpdateUserAsync(
            bookingUser,
            cancellationToken);

        _logger.LogInformation(
            "[Equipment] Completed User Updated Event with ID {EventId}",
            notification.EventId);
    }
}
