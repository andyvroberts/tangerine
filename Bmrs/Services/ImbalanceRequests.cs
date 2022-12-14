using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Bmrs.Services
{
    using Bmrs.Models;
    using Bmrs.Common;

    public static class ImbalanceRequests
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// perform the HTTP request from the BMRS API, retrieve one day of imbalance price records.
        /// Note: Historically there used to be 2 prices (buy & sell), although today only buy prices are created
        ///   by the balancing mechanism.  This converged the price for MWh irrespective of surpluss or shortfall.
        /// <summary>
        public static async Task<IEnumerable<ImbalancePriceItem>> BmrsPriceRequestAsync(string apiKey, string settDate)
        {
            List<ImbalancePriceItem> webRecs = new List<ImbalancePriceItem>();
            var uri = Constants.SystemPrice.Replace("<KEY>", apiKey);
            uri = uri.Replace("<SD>", settDate);
            uri = uri.Replace("<SP>", "*");

            HttpResponseMessage priceResponse = await client.GetAsync(uri).ConfigureAwait(false);
            Stream priceStream = await priceResponse.Content.ReadAsStreamAsync();

            XmlSerializer ser = new XmlSerializer(typeof(B1770Response), new XmlRootAttribute("response"));
            var webResults = (B1770Response)ser.Deserialize(priceStream);

            if (webResults.IsUsable)
                webRecs.AddRange(webResults.Body.ItemList.Items);

            priceResponse.Dispose();
            return webRecs;
        }

        /// <summary>
        /// perform the HTTP request from the BMRS API, retrieve one day of imbalance volumes.
        /// (this code looks duplicate and could be removed by creating a method with generic types.  this has not been 
        ///  implemented in case of changes to the response from BMRS - which is being replaced by Elexon)
        /// <summary>
        public static async Task<IEnumerable<ImbalanceVolumeItem>> BmrsVolumeRequestAsync(string apiKey, string settDate)
        {
            List<ImbalanceVolumeItem> webRecs = new List<ImbalanceVolumeItem>();
            var uri = Constants.SystemVolume.Replace("<KEY>", apiKey);
            uri = uri.Replace("<SD>", settDate);
            uri = uri.Replace("<SP>", "*");

            HttpResponseMessage volumeResponse = await client.GetAsync(uri).ConfigureAwait(false);
            Stream volumeStream = await volumeResponse.Content.ReadAsStreamAsync();

            XmlSerializer ser = new XmlSerializer(typeof(B1780Response), new XmlRootAttribute("response"));
            var webResults = (B1780Response)ser.Deserialize(volumeStream);

            if (webResults.IsUsable)
                webRecs.AddRange(webResults.Body.ItemList.Items);

            volumeResponse.Dispose();
            return webRecs;
        }

    }
    
}