using SafeWalk.Application.Common;
using SafeWalk.Application.DTOs;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Application.Interfaces.Services;

namespace SafeWalk.Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IJourneyRepository _journeyRepository; // you already have journeys working
        private readonly ITrustedContactRepository _trustedContactRepository;

        public AlertService(
            IAlertRepository alertRepository,
            IJourneyRepository journeyRepository,
            ITrustedContactRepository trustedContactRepository)
        {
            _alertRepository = alertRepository;
            _journeyRepository = journeyRepository;
            _trustedContactRepository = trustedContactRepository;
        }

        public async Task<Result<bool>> PanicAsync(Guid journeyId, CancellationToken cancellationToken = default)
        {
            var journey = await _journeyRepository.GetByIdAsync(journeyId, cancellationToken);
            if (journey == null) return Result<bool>.Fail("Journey not found.");

            // Persist alert
            var alert = new AlertDto
            {
                Id = Guid.NewGuid(),
                JourneyId = journeyId,
                Message = $"Emergency alert from SafeWalk for journey to {journey.Destination}.",
                SentAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _alertRepository.CreateAsync(alert, cancellationToken);

            // Link contacts + simulate send
            var contacts = await _trustedContactRepository.GetByUserIdAsync(journey.UserId, cancellationToken);
            foreach (var c in contacts)
            {
                Console.WriteLine($"[ALERT -> {c.ContactPhoneNumber}] {alert.Message}");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> CheckOverdueAsync(Guid journeyId, CancellationToken cancellationToken = default)
        {
            var journey = await _journeyRepository.GetByIdAsync(journeyId, cancellationToken);
            if (journey == null) return Result<bool>.Fail("Journey not found.");

            var overdue = DateTime.UtcNow > journey.ExpectedArrivalTimeUtc && journey.ActualArrivalTimeUtc == null;
            if (!overdue) return Result<bool>.Ok(false);

            var alert = new AlertDto
            {
                Id = Guid.NewGuid(),
                JourneyId = journeyId,
                Message = $"SafeWalk alert: {journey.Destination} not reached on time.",
                SentAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _alertRepository.CreateAsync(alert, cancellationToken);

            var contacts = await _trustedContactRepository.GetByUserIdAsync(journey.UserId, cancellationToken);
            foreach (var c in contacts)
            {
                Console.WriteLine($"[OVERDUE -> {c.ContactPhoneNumber}] {alert.Message}");
            }

            return Result<bool>.Ok(true);
        }
    }
}

