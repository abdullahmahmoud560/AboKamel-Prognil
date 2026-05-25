using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Results;


public static class ResultExtensions
{
    /// <summary>
    /// Transforms a Result's type from a source type to a destination type. If the Result is successful, the func parameter is invoked on the Result's source value to map it to a destination type.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="result"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception> 
    public static ResultAbstract<TDestination> Map<TSource, TDestination>(this ResultAbstract<TSource> result, Func<TSource, TDestination> func)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok: return func(result);
            case ResultStatus.NotFound:
                return result.Errors.Any()
                    ? ResultAbstract<TDestination>.NotFound(result.Errors.ToArray())
                    : ResultAbstract<TDestination>.NotFound();
            case ResultStatus.Unauthorized: return ResultAbstract<TDestination>.Unauthorized();
            case ResultStatus.Forbidden: return ResultAbstract<TDestination>.Forbidden();
            case ResultStatus.Invalid: return ResultAbstract<TDestination>.Invalid(result.ValidationErrors);
            case ResultStatus.Error: return ResultAbstract<TDestination>.Error(result.Errors.ToArray());
            case ResultStatus.Conflict:
                return result.Errors.Any()
                                    ? ResultAbstract<TDestination>.Conflict(result.Errors.ToArray())
                                    : ResultAbstract<TDestination>.Conflict();
            case ResultStatus.CriticalError: return ResultAbstract<TDestination>.CriticalError(result.Errors.ToArray());
            case ResultStatus.Unavailable: return ResultAbstract<TDestination>.Unavailable(result.Errors.ToArray());
            default:
                throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
        }
    }
}