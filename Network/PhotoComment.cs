//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Network
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhotoComment
    {
        public int id { get; set; }
        public int id_owner { get; set; }
        public string body { get; set; }
        public System.DateTime date_creation { get; set; }
        public bool deleted { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
        public string likes { get; set; }
    }
}