using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics_Store
{
    public partial class Продажа
    {
        public string FIO
        {
            get
            {
                return $"{Пользователь.fullname} {Пользователь.name} {Пользователь.papaname}";
            }
        }
    }
}
