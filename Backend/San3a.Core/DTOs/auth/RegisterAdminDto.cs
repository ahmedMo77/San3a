using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.auth
{
    public class RegisterAdminDto : RegisterAppUserDto
    {
        #region Properties
        public bool IsSuperAdmin { get; set; }
        #endregion
    }
}
