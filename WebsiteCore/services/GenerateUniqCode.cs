using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteCore.services
{
   public class GenerateUniqCode
    {
        public static string GenerateActiveCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
