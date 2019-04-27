using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public class SimpleDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now
        {
            get { return DateTime.UtcNow; }
        }
    }
}
