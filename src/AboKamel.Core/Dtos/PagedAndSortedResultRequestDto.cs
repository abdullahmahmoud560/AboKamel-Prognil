using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Dtos;
/// <summary>
/// Data transfer object that extends pagination with sorting capabilities.
/// Inherits paging functionality from <see cref="PagedResultRequestDto"/>.
/// </summary>
public class PagedAndSortedResultRequestDto : PagedResultRequestDto
{
    /// <summary>
    /// Gets or sets the sorting expression (e.g., "Name ASC", "CreatedDate DESC").
    /// </summary>
    public virtual string? Sorting { get; set; }
}
