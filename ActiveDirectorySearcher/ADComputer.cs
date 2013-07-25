using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiveDirectorySearcher
{
    public class ADComputer
    {
        public string ComputerName { get; set; }
        public string ComputerDescription { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingSystemVersion { get; set; }
    }
}
