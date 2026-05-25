using Capsula.Application.Dtos.Mobile.Supports;
using Capsula.Domain.Entities.Supports;
using Services.Core.DependencyInjection;

namespace Capsula.Application.Contracts.Mobile.Supports;

public interface ISupportService : ICrudService<SupportRequestDto, Support, SupportResponseDto, SupportDetailedResponseDto, int>, IApplicationService, IScopedService
{
}
