using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Entity
{
    internal class User
    {
        public int Id = 1;
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }

    }
}
