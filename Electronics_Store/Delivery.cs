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
    
    public partial class Delivery
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Delivery()
        {
            this.TovarDels = new HashSet<TovarDel>();
        }
    
        public int id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string status { get; set; }
        public int countTov { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TovarDel> TovarDels { get; set; }
    }
}
