using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosoft.ScaleReader
{
    public class ErrorEventArgs : EventArgs
    {
        internal ErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }
    }
}
