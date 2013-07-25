using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Collections.Specialized;
using ActiveDirectorySearcher;

namespace ActiveDirectoryTools
{
    public class ADSearcher
    {
        private string domain;
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private DirectoryEntry dentry;
        public DirectoryEntry DEntry
        {
            get { return dentry; }
            set { dentry = value; }
        }

        public ADSearcher(string domainName, string user, string pass)
        {
            domain = domainName;
            username = user;
            password = pass;

            //-----------------------------------
            //  Split the domain parameter to
            //  form the correct domain string
            //-----------------------------------

            string domainString = string.Empty;

            string[] dc = domain.Split('.');
            foreach (string x in dc)
            {
                if (dc[dc.Length - 1].Equals(x))
                {
                    domainString = domainString + "DC=" + x;
                }
                else
                {
                    domainString = domainString + "DC=" + x + ",";
                }
            }
            
            //----------------------------------------
            //  Create the new directory entry,
            //  which will be used to connected to AD
            //----------------------------------------
            dentry = new DirectoryEntry("LDAP://" + domainString, username, password); 
        }
    
        /// <summary>
        /// Search Active Directory computer's description field.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<ADComputer> SearchComputerDescription(string search)
        {
            if (search == null || search == string.Empty)
            {
                throw new System.ArgumentNullException("Parameter cannot be null or empty");
            }
            return SearchComputers(search, "Description");
        }

        /// <summary>
        /// Search Active Directory computer's name field.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<ADComputer> SearchComputerName(string search)
        {
            if (search == null || search == string.Empty)
            {
                throw new System.ArgumentNullException("Parameter cannot be null or empty");
            }
            return SearchComputers(search, "Name");
        }

        /// <summary>
        /// Search Active Directory user's description field.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<ADUser> SearchUserDescription(string search)
        {
            if (search == null || search == string.Empty)
            {
                throw new System.ArgumentNullException("Parameter cannot be null or empty");
            }
            return SearchUsers(search, "Description");
        }

        /// <summary>
        /// Search Active Directory user's username field.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<ADUser> SearchUserName(string search)
        {
            if (search == null || search == string.Empty)
            {
                throw new System.ArgumentNullException("Parameter cannot be null or empty");
            }
            return SearchUsers(search, "userPrincipalName");
        }

        private List<ADComputer> SearchComputers(string search, string searchField)
        {            
            //Create a searcher using the previously specified ldap connection entry.
            DirectorySearcher searcher = new DirectorySearcher(dentry);

            //Specify the searcher filter for searching.
            searcher.Filter = "(&(&(objectCategory=computer)(" + searchField + "=*" + search + "*)))";

            //Search AD.
            SearchResultCollection searchResults = searcher.FindAll();
                        
            List<ADComputer> ADComputers = new List<ADComputer>();

            //Iterate through all of the search results, creating a new ADComputer object and adding it to the above list.
            for (int i = 0; i < searchResults.Count; i++)
            {
                try
                {
                    ADComputers.Add(new ADComputer
                    {
                        ComputerName = searchResults[i].Properties["name"][0].ToString(),
                        ComputerDescription = searchResults[i].Properties["description"][0].ToString(),
                        OperatingSystem = searchResults[i].Properties["operatingsystem"][0].ToString(),
                        OperatingSystemVersion = searchResults[i].Properties["operatingsystemversion"][0].ToString()
                    });
                }
                catch 
                {
                    //Add a fake computer, noting the error item.
                    ADComputers.Add(new ADComputer
                    {
                        ComputerName = "Error with Results on item #" + i
                    });
                }
            }
            return ADComputers;
        }

        private List<ADUser> SearchUsers(string search, string searchField)
        {
            //Create a searcher using the previously specified ldap connection entry.
            DirectorySearcher searcher = new DirectorySearcher(dentry);

            //Specify the searcher filter for searching.
            searcher.Filter = "(&(&(objectCategory=user)(" + searchField + "=*" + search + "*)))";

            //Search AD.
            SearchResultCollection searchResults = searcher.FindAll();

            List<ADUser> ADUsers = new List<ADUser>();

            //Iterate through all of the search results, creating a new ADUser object and adding it to the above list.
            for (int i = 0; i < searchResults.Count; i++)
            {
                try
                {
                    ADUsers.Add(new ADUser
                    {
                        UserLogonName = searchResults[i].Properties["userPrincipalName"][0].ToString(),
                        UserName = searchResults[i].Properties["name"][0].ToString(),
                        UserDescription = searchResults[i].Properties["description"][0].ToString(),
                        LogonCount = searchResults[i].Properties["logonCount"][0].ToString()
                    });
                }
                catch 
                {
                    //Add a fake user, noting the error item.
                    ADUsers.Add(new ADUser
                    {
                        UserLogonName = "Error with Results on item #" + i
                    });
                }
            }
            return ADUsers;
        }
    }
}
