using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics_Store
{
    public partial class МагазинЭлектроникиEntities : DbContext
    {
        private static МагазинЭлектроникиEntities _context;

        public static МагазинЭлектроникиEntities GetContext()
        {
            if (_context == null) _context = new МагазинЭлектроникиEntities();
            return _context;
        }
    }
}
