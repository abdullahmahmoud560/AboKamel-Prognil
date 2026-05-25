using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DependencyInjection;
/// <summary>
/// Marker interface used to indicate that a service should be registered with a scoped lifetime
/// in the dependency injection container. Scoped services are created once per client request.
/// </summary>
public interface IScopedService
{
}
