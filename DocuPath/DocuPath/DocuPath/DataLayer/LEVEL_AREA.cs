//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocuPath.DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class LEVEL_AREA
    {
        public int AccessAreaID { get; set; }
        public int AccessLevelID { get; set; }
        public string LevelAreaDescription { get; set; }
    
        public virtual ACCESS_AREA ACCESS_AREA { get; set; }
        public virtual ACCESS_LEVEL ACCESS_LEVEL { get; set; }
    }
}
