//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KGERP.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverMobile { get; set; }
        public System.DateTime SendDate { get; set; }
        public string request_type { get; set; }
        public string campaign_uid { get; set; }
        public string sms_uid { get; set; }
        public string api_response_code { get; set; }
        public string api_response_message { get; set; }
        public string MessageBody { get; set; }
    }
}
