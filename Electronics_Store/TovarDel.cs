//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Electronics_Store
{
    using System;
    using System.Collections.Generic;
    
    public partial class TovarDel
    {
        public int id { get; set; }
        public int idTovar { get; set; }
        public int idDel { get; set; }
        public int count { get; set; }
    
        public virtual Delivery Delivery { get; set; }
        public virtual Tovar Tovar { get; set; }
    }
}