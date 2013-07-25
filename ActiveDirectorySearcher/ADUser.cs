using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiveDirectorySearcher
{
    public class ADUser
    {
        public string UserLogonName { get; set; }
        public string UserName { get; set; }
        public string UserDescription { get; set; }
        public string LogonCount { get; set; }
    }
}
