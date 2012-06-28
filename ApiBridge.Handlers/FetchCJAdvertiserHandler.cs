using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;
using ApiBridge.Contracts;
using ApiBridge.Bus.Core;

namespace ApiBridge.Handlers
{
    //public class FetchCJAdvertiserHandler : ICommandHandler<FetchCJAdvertisersEvent>
    //{
    //    public List<Advertiser> Advertisers { get; set; }

    //    public FetchCJAdvertiserHandler()
    //    {
    //        Advertisers = new List<Advertiser>();
    //    }

    //    public void Handle(ICommandReceiver<FetchCJAdvertisersEvent> ev)
    //    {
    //        Console.WriteLine("Handling For: " + ev.Body.PublisherId);

    //        RequestPager pager = new RequestPager();
    //        while (pager.PageNumber <= pager.TotalPageCount + 1)
    //        {
    //            this.Advertisers = FetchAdvertisers(ev.Body.PublisherId, ev.Body.WebServiceToken, pager);

    //            foreach (Advertiser a in this.Advertisers)
    //            {
    //                List<Link> links =
    //                    FetchLinks(ev.Body.PublisherId, ev.Body.WebServiceToken, a.CID.ToString(), a.Id)
    //                    .Take(100).ToList();

    //                a.Links = new List<Link>();

    //                foreach (CJLink l in links)
    //                {
    //                    a.Links.Add(l);
    //                }

    //                // Populate the advertiser entity from a CJ.Advertiser 
    //                Advertiser contractAdv = GetAdvertiserContract(a);

    //                // Publish the advertiser and all of its Links

    //                FetchCJAdvertiserResponse response = new FetchCJAdvertiserResponse();
    //                response.Advertiser = contractAdv;

    //                BusConfiguration.Instance.Bus.PublishAsync<FetchCJAdvertiserResponse>(response, (x) =>
    //                {
    //                    if (x.IsSuccess)
    //                    {
    //                        Console.WriteLine("Publishing: " + a.AdvertiserName + " Page " + pager.PageNumber);
    //                    }
    //                });

    //                Advertisers.Add(contractAdv);
    //            }
    //        }
    //    }

    //    Advertiser GetAdvertiserContract(CJAdvertiser a)
    //    {
    //        Advertiser contractAdv = new Advertiser();

    //        contractAdv.Id = Guid.Parse(a.OrgId);
    //        contractAdv.MerchantId = a.CID.ToString();
    //        contractAdv.Status = 1;
    //        contractAdv.CreationDate = DateTime.UtcNow;
    //        contractAdv.Context = "CJ.COM";
    //        contractAdv.ReferenceLink = a.CID.ToString();
    //        contractAdv.AdvertiserName = a.AdvertiserName;
    //        contractAdv.Language = a.Language;
    //        contractAdv.NetworkRank = a.NetworkRank;
    //        contractAdv.PerformanceIncentives = a.PerformanceIncentives;
    //        contractAdv.ProgramHost = a.ProgramHost;
    //        contractAdv.ProgramUrl = a.ProgramUrl;
    //        contractAdv.ResolvedDomain = a.ResolvedDomain;

    //        decimal sevenDayEpc, threeMonthepc;

    //        if (!String.IsNullOrEmpty(a.SevenDayEpc)
    //            && Decimal.TryParse(a.SevenDayEpc.Replace("$", ""), out sevenDayEpc))
    //            contractAdv.SevenDayEpc = sevenDayEpc.ToString();

    //        if (!String.IsNullOrEmpty(a.ThreeMonthEpc)
    //            && Decimal.TryParse(a.ThreeMonthEpc.Replace("$", ""), out threeMonthepc))
    //            contractAdv.ThreeMonthEpc = threeMonthepc.ToString();

    //        contractAdv.VerifiedDomain = a.VerifiedDomain;
    //        GetLinks(a, contractAdv);

    //        return contractAdv;
    //    }

    //    /// <summary>
    //    /// Gets the links. Note There will be no DestinationHost
    //    /// or DomainId. Refer to PublishLinks in workflowservice to see
    //    /// exactly what information is missing at the time this function is run.
    //    /// </summary>
    //    /// <param name="contractAdv">The contract adv.</param>
    //    private static void GetLinks(CJAdvertiser a, Advertiser contractAdv)
    //    {
    //        contractAdv.Links = new List<Link>();
    //        foreach (CJLink l in a.Links)
    //        {
    //            Link domainUrl = new Link();
    //            domainUrl.Id = Guid.Parse(l.DomainUrlId);
    //            domainUrl.ReferenceLink = l.Id.ToString();
    //            domainUrl.CreationDate = DateTime.UtcNow;
    //            domainUrl.AdvertiserName = l.AdvertiserName;
    //            domainUrl.Category = l.Category;
    //            domainUrl.CID = l.CID.ToString();
    //            domainUrl.Context = "CJ.COM";
    //            domainUrl.CreativeHeight = l.CreativeHeight;
    //            domainUrl.CreativeWidth = l.CreativeWidth;
    //            //domainUrl.DestinationHost = l.VerifiedDomain.ToUpper();
    //            domainUrl.Language = l.Language;
    //            domainUrl.LinkCodeHTML = l.LinkCodeHTML;
    //            domainUrl.LinkCodeJavascript = l.LinkCodeJavascript;
    //            domainUrl.LinkDescription = l.LinkDescription;
    //            domainUrl.LinkDestination = l.LinkDestination;
    //            domainUrl.LinkDomain = l.LinkDomain;
    //            domainUrl.Url = l.LinkHref;
    //            domainUrl.RetryCount = 0;

    //            domainUrl.RelationshipStatus = l.RelationshipStatus;
    //            domainUrl.Status = 1;
    //            if (l.LinkType == "Text Link")
    //                domainUrl.Type = 1;
    //            else
    //            {
    //                domainUrl.Type = 2;
    //                if (domainUrl.LinkCodeHTML != null)
    //                {
    //                    domainUrl.LinkImgUrl = ServiceUtil.GetParsedSrc(l.LinkCodeHTML);
    //                }
    //            }

    //            contractAdv.Links.Add(domainUrl);
    //        }
    //    }
    //}
}
