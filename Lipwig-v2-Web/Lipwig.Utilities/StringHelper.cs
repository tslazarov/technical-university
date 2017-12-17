using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Utilities
{
    public class StringHelper
    {
        public static string EncodeEmailForUrl(string email)
        {
            return email.Replace(".", "%20-%20").Replace("@", "%40");
        }

        public static string DecodeEmailFromUrl(string email)
        {
            return email.Replace(" - ", ".");
        }
    }
}
