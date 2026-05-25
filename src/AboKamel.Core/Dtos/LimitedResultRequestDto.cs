using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Dtos;

public class LimitedResultRequestDto
{
    //
    // Summary:
    //     Default value: 10.
    public static int DefaultMaxResultCount { get; set; } = 10;


    //
    // Summary:
    //     Default value: 1,000.
    public static int MaxMaxResultCount { get; set; } = 1000;


    //
    // Summary:
    //     Maximum result count should be returned. This is generally used to limit result
    //     count on paging.
    [Range(1, int.MaxValue)]
    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;



}
