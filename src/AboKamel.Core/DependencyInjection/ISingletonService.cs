using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DependencyInjection;
/// <summary>
/// Marker interface used to indicate that a service should be registered with a singleton lifetime
/// in the dependency injection container. Singleton services are created once and shared across the entire application.
/// </summary>
public interface ISingletonService
{
}
