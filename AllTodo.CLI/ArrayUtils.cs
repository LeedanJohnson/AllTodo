using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.CLI
{
    public class ArrayUtils
    {
        public static string[] RemoveFirst(string[] val)
        {
            string[] retval = new string[val.Length - 1];
            Array.Copy(val, 1, retval, 0, retval.Length);
            return retval;
        }
    }
}
