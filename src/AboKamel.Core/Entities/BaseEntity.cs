using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Entities;
/// <summary>
/// Represents a base entity with a unique identifier.
/// </summary>
public class BaseEntity<T> : IEntity<T>
{
    public T Id { get; set; }
}
