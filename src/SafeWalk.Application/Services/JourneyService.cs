using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Infrastructure;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Application.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITrustedContactRepository _trustedContactRepository;
        private readonly IAlertRepository _alertRepository;
        private readonly INotificationService _notificationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JourneyService(
            IJourneyRepository journeyRepository,
            IUserRepository userRepository,
            ITrustedContactRepository trustedContactRepository,
            IAlertRepository alertRepository,
            INotificationService notificationService,
            IDateTimeProvider dateTimeProvider)
        {
            _journeyRepository = journeyRepository;
            _userRepository = userRepository;
            _trustedContactRepository = trustedContactRepository;
            _alertRepository = alertRepository;
            _notificationService = notificationService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<JourneyDto>> StartJourneyAsync(
            Guid userId,
            string startLocation,
            string destination,
            DateTime expectedArrivalUtc,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return Result<JourneyDto>.Fail("User not found.");
            }

            var journey = new JourneyDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                StartLocation = startLocation,
                Destination = destination,
                StartTimeUtc = _dateTimeProvider.UtcNow,
                ExpectedArrivalTimeUtc = expectedArrivalUtc,
                Status = "InProgress"
            };

            await _journeyRepository.CreateAsync(journey, cancellationToken);

            return Result<JourneyDto>.Ok(journey);
        }

        public async Task<Result<bool>> CompleteJourneyAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default)
        {
            var journey = await _journeyRepository.GetByIdAsync(journeyId, cancellationToken);
            if (journey is null)
            {
                return Result<bool>.Fail("Journey not found.");
            }

            if (journey.Status == "Completed")
            {
                return Result<bool>.Ok(true);
            }

            journey.Status = "Completed";
            journey.ActualArrivalTimeUtc = _dateTimeProvider.UtcNow;

            await _journeyRepository.UpdateAsync(journey, cancellationToken);

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> EscalateIfOverdueAsync(
            Guid journeyId,
            CancellationToken cancellationToken = default)
        {
            var journey = await _journeyRepository.GetByIdAsync(journeyId, cancellationToken);
            if (journey is null)
            {
                return Result<bool>.Fail("Journey not found.");
            }

            var now = _dateTimeProvider.UtcNow;
            if (journey.Status != "InProgress" || now <= journey.ExpectedArrivalTimeUtc)
            {
                return Result<bool>.Ok(false); // Not overdue or already finished
            }

            // Create alert entity
            var alert = new AlertDto
            {
                Id = Guid.NewGuid(),
                JourneyId = journey.Id,
                Message = $"SafeWalk alert: {journey.Destination} not reached on time.",
                SentAtUtc = now
            };

            await _alertRepository.CreateAsync(alert, cancellationToken);

            // Notify trusted contacts
            var contacts = await _trustedContactRepository.GetByUserIdAsync(journey.UserId, cancellationToken);
            foreach (var contact in contacts)
            {
                await _notificationService.SendSmsAsync(
                    contact.ContactPhoneNumber,
                    alert.Message,
                    cancellationToken);
            }

            // Mark journey as escalated
            journey.Status = "Escalated";
            await _journeyRepository.UpdateAsync(journey, cancellationToken);

            return Result<bool>.Ok(true);
        }
    }
}

