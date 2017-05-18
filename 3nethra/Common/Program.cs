
using System;
using System.IO;

namespace Artelus.Common
{
    public class Program
    {
        public Program()
        {
            SetIdentity("", "".Split(','));
        }
        
        public static System.Security.Principal.GenericPrincipal Principal;//= new System.Security.Principal.GenericPrincipal(null, null);        

        public static void SetIdentity(string userId, string[] roles)
        {
            System.Security.Principal.GenericIdentity identity = new System.Security.Principal.GenericIdentity(userId);
            Principal = new System.Security.Principal.GenericPrincipal(identity, roles);
        }

        public static EnumRoles GetRoleName()
        {
            EnumRoles roleName = EnumRoles.Administrator;
            if (Principal.IsInRole(EnumRoles.Administrator.ToString()))
            {
                roleName = EnumRoles.Administrator;
            }

            return roleName;
        }

        public static int UserId() => int.Parse(Program.Principal?.Identity.Name ?? "0");

        public static string FileSize(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = new FileInfo(filename).Length;
                int order = 0;
                while (len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len / 1024;
                }

                // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                // show a single decimal place, and no space.
                return String.Format("{0:0.##} {1}", len, sizes[order]);
            }
            return string.Empty;
        }
    }
}
