namespace Capsula.Application.Dtos;

public class BaseAuditableResponseDto<TKey> : BaseResponseDto<TKey>
{
    public string? CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
}
