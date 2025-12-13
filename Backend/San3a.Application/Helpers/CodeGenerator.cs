using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Helpers
{
    public class CodeGenerator
    {
        public static string GenerateCode(int length = 4)
        {
            var random = new Random();
            var code = "";

            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 10).ToString();
            }

            return code;
        }
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
