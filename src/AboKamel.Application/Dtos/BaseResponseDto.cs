using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsula.Application.Dtos;

public class BaseResponseDto<TKey>
{
    public TKey? Id { get; set; }
}
