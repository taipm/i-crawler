//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MathCrawler
{
    using System;
    using System.Collections.Generic;
    
    public partial class LinkDocument
    {
        public System.Guid Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public decimal Size { get; set; }
        public string FileType { get; set; }
        public System.DateTime UploadDate { get; set; }
        public Nullable<System.Guid> LinkId { get; set; }
    
        public virtual Link Link { get; set; }
    }
}
