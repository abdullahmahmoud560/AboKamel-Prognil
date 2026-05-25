using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DependencyInjection;
/// <summary>
/// Marker interface used to indicate that a service should be registered with a transient lifetime
/// in the dependency injection container. transient services are created each time they are requested
/// </summary>

public interface ITransientService
{
}
