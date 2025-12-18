using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Infrastructure;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IJourneyRepository _journeyRepository;
        private readonly ITrustedContactRepository _trustedContactRepository;
        private readonly INotificationService _notificationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AlertService(
            IAlertRepository alertRepository,
            IJourneyRepository journeyRepository,
            ITrustedContactRepository trustedContactRepository,
            INotificationService notificationService,
            IDateTimeProvider dateTimeProvider)
        {
            _alertRepository = alertRepository;
            _journeyRepository = journeyRepository;
            _trustedContactRepository = trustedContactRepository;
            _notificationService = notificationService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<bool>> SendJourneyAlertAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default)
        {
            var journey = await _journeyRepository.GetByIdAsync(journeyId, cancellationToken);
            if (journey is null)
            {
                return Result<bool>.Fail("Journey not found.");
            }

            var alert = new AlertDto
            {
                Id = Guid.NewGuid(),
                JourneyId = journeyId,
                Message = $"Emergency alert from SafeWalk for journey to {journey.Destination}.",
                SentAtUtc = _dateTimeProvider.UtcNow
            };

            await _alertRepository.CreateAsync(alert, cancellationToken);

            var contacts = await _trustedContactRepository.GetByUserIdAsync(journey.UserId, cancellationToken);
            foreach (var contact in contacts)
            {
                await _notificationService.SendSmsAsync(
                    contact.ContactPhoneNumber,
                    alert.Message,
                    cancellationToken);
            }

            return Result<bool>.Ok(true);
        }
    }
}

