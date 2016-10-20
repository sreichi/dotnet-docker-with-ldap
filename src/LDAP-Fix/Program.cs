using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Novell.Directory.Ldap;

namespace LDAP_Fix
{
    public class Program
    {
        public static void Main(String[] args)
        {
            int LdapPort = 10389;
            int searchScope = LdapConnection.SCOPE_ONE;
            int LdapVersion = LdapConnection.Ldap_V3; ;
            bool attributeOnly = true;
            String[] attrs = { LdapConnection.NO_ATTRS };
            String ldapHost = "192.168.2.130";
            String loginDN = "";
            String password = "";
            String searchBase = "";
            String searchFilter = "";
            LdapConnection lc = new LdapConnection();

            try
            {
                // connect to the server
                lc.Connect(ldapHost, LdapPort);
                // bind to the server
                lc.Bind(LdapVersion, loginDN, password);

                LdapSearchResults searchResults =
                    lc.Search(searchBase,      // container to search
                    searchScope,     // search scope
                    searchFilter,    // search filter
                    attrs,           // "1.1" returns entry name only
                    attributeOnly);  // no attributes are returned

                // print out all the objects
                while (searchResults.hasMore())
                {
                    LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = searchResults.next();
                    }
                    catch (LdapException e)
                    {
                        Console.WriteLine("Error: " + e.ToString());

                        // Exception is thrown, go for next entry
                        continue;
                    }

                    Console.WriteLine("\n" + nextEntry.DN);
                }
                // disconnect with the server
                lc.Disconnect();
            }
            catch (LdapException e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
            Environment.Exit(0);
        }
    }
}
