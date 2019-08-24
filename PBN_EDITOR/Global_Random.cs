using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBN_EDITOR
{
    class Global_Random
    {
        public static Random rd;
        public static void init()
        {
            rd = new Random();
        }
    }
}
