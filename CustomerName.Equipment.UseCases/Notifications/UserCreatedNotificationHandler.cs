using MediatR;
using Microsoft.Extensions.Logging;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Identity.Contract.Events;

namespace CustomerName.Portal.Equipment.UseCases.Notifications;

internal class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly IEquipmentUserRepository _equipmentUserRepository;
    private readonly ILogger<UserCreatedNotificationHandler> _logger;

    public UserCreatedNotificationHandler(
        IEquipmentUserRepository equipmentUserRepository,
        ILogger<UserCreatedNotificationHandler> logger)
    {
        _equipmentUserRepository = equipmentUserRepository;
        _logger = logger;
    }

    public async Task Handle(
        UserCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[Equipment] Received User Created Event with ID {EventId}. User Id: {UserId}",
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
            "[Equipment] Completed User Created Event with ID {EventId}",
            notification.EventId);
    }
}
