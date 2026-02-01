using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRcodeStorage.Models
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int? IdCategory { get; set; }
        public string? Place{ get; set; }
        public int? IdMaker { get; set; }
        public string? Description { get; set; }

    }
}
