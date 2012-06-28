using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ApiBridge.Contracts
{
    public class Advertiser
    {
        [DataMember]
        public System.Guid Id { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public Guid WebIdentityId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string AdvertiserContactEmail { get; set; }

        [DataMember]
        public string AdvertiserContactName { get; set; }

        [DataMember]
        public string AdvertiserContactPhone { get; set; }

        [DataMember]
        public string AdvertiserContactTitle { get; set; }

        [DataMember]
        public string AdvertiserName { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string ReferenceLink { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string DefaultHitCommission { get; set; }

        [DataMember]
        public string DefaultLeadCommission { get; set; }

        [DataMember]
        public string DefaultSaleCommission { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public int LinkCount { get; set; }

        [DataMember]
        public string NetworkRank { get; set; }

        [DataMember]
        public string PerformanceIncentives { get; set; }

        [DataMember]
        public string ProgramHost { get; set; }

        [DataMember]
        public string ProgramUrl { get; set; }

        [DataMember]
        public string ResolvedDomain { get; set; }

        [DataMember]
        public string SevenDayEpc { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ThirtyDayEpc { get; set; }

        [DataMember]
        public string ThreeMonthEpc { get; set; }

        [DataMember]
        public string VerifiedDomain { get; set; }

        [DataMember]
        public string Zip { get; set; }

        [DataMember]
        public System.DateTime CreationDate { get; set; }

        [DataMember]
        public byte Status { get; set; }

        [DataMember]
        public List<Link> Links { get; set; }

        //[DataMember]
        //public 

    }
}
