using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Dexin.Buiness
{
    class Externs
    {
         [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); 
    }
}
