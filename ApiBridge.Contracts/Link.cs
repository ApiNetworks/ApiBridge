using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ApiBridge.Contracts
{
    public class Link
    {
        [DataMember]
        public System.Guid Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> DomainId { get; set; }

        [DataMember]
        public string ReferenceLink { get; set; }

        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public string AdvertiserName { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string CID { get; set; }

        [DataMember]
        public Nullable<int> CreativeHeight { get; set; }

        [DataMember]
        public Nullable<int> CreativeWidth { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string LinkCodeHTML { get; set; }

        [DataMember]
        public string LinkCodeJavascript { get; set; }

        [DataMember]
        public string LinkDescription { get; set; }

        [DataMember]
        public string LinkDestination { get; set; }

        [DataMember]
        public string LinkDomain { get; set; }

        [DataMember]
        public Nullable<System.DateTime> LinkEndDate { get; set; }

        [DataMember]
        public Nullable<System.DateTime> LinkStartDate { get; set; }

        [DataMember]
        public string LinkImgUrl { get; set; }

        [DataMember]
        public string RelationshipStatus { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string UrlHost { get; set; }

        [DataMember]
        public string DestinationUrl { get; set; }

        [DataMember]
        public string DestinationHost { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public int RetryCount { get; set; }

        [DataMember]
        public Nullable<System.DateTime> LastUpdate { get; set; }

        [DataMember]
        public string ResponseCode { get; set; }

        [DataMember]
        public Nullable<byte> IsValid { get; set; }

        [DataMember]
        public byte Status { get; set; }

        [DataMember]
        public System.DateTime CreationDate { get; set; }
    }
}
