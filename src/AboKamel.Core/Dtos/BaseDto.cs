using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Dtos;

/// <summary>
/// Base data transfer object (DTO) class.
/// </summary>
public class BaseDto<T>
{
    public T Id { get; set; }
}
