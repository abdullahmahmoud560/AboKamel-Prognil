using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Dtos;


/// <summary>
/// Data transfer object used to request paged results, including the number of records to skip.
/// Inherits the maximum result count from <see cref="LimitedResultRequestDto"/>.
/// </summary>
public class PagedResultRequestDto : LimitedResultRequestDto
{
    /// <summary>
    /// Gets or sets the number of records to skip before starting to return results.
    /// Useful for implementing pagination.
    /// </summary>
    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; }
}