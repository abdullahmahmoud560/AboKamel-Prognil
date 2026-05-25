using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.Dtos;
public class ListResultDto<T>
{
    private IReadOnlyList<T>? _items;

    public IReadOnlyList<T> Items
    {
        get
        {
            return _items ?? (_items = new List<T>());
        }
        set
        {
            _items = value;
        }
    }

    //
    // Summary:
    public ListResultDto()
    {
    }


    public ListResultDto(IReadOnlyList<T> items)
    {
        Items = items;
    }
}
