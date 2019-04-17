using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
