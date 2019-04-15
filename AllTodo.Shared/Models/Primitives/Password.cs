using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecureCodingToDo
{
    class Password
    {
        private static readonly int MINIMUM_LENGTH = 8;
        private static readonly int MAXIMUM_LENGTH = 25;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[a-zA-Z0-9!]*$");

    }
}
