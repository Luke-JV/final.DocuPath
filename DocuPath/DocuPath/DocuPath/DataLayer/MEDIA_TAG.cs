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
    
    public partial class MEDIA_TAG
    {
        public int ContentTagID { get; set; }
        public int MediaID { get; set; }
        public string MediaTagOtherDescription { get; set; }
    
        public virtual CONTENT_TAG CONTENT_TAG { get; set; }
        public virtual CONTENT_TAG CONTENT_TAG1 { get; set; }
        public virtual MEDIA MEDIA { get; set; }
    }
}
