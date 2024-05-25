
namespace BmrsRetriever
{
    public static class Mapper
    {
        public static ImbalancePrice FromXmlToImbalancePrice(this ImbalancePriceItem x)
        {
            return new ImbalancePrice
            {
                SettlementDate = x.SettlementDate,
                SettlementPeriod = x.SettlementPeriod,
                ImbalanceType = x.TimeSeriesId != null && x.TimeSeriesId.EndsWith('1') ? "BUY" : "SELL",
                TimeSeriesId = x.TimeSeriesId,
                BusinessType = x.BusinessType,
                ControlArea = x.ControlArea,
                PriceGbp = x.ImbalancePriceAmountGbp,
                PriceCategory = x.PriceCategory,
                CurveType = x.CurveType,
                Resolution = x.Resolution,
                DocumentType = x.DocumentType,
                ProcessType = x.ProcessType,
                ActiveFlag = x.ActiveFlag,
                Status = x.DocStatus,
                DocumentId = x.DocumentId,
                RevisionNumber = x.DocumentRevNum
            };
        }

        public static ImbalanceVolume FromXmlToImbalanceVolume(this ImbalanceVolumeItem v)
        {
            return new ImbalanceVolume
            {
                SettlementDate = v.SettlementDate,
                SettlementPeriod = v.SettlementPeriod,
                ImbalanceType = v.TimeSeriesId != null && v.TimeSeriesId.EndsWith('1') ? "BUY" : "SELL",
                TimeSeriesId = v.TimeSeriesId,
                BusinessType = v.BusinessType,
                ControlArea = v.ControlArea,
                Quantity = v.ImbalanceQuantityMaw,
                Direction = v.ImbalanceDirection,
                CurveType = v.CurveType,
                Resolution = v.Resolution,
                DocumentType = v.DocumentType,
                ProcessType = v.ProcessType,
                ActiveFlag = v.ActiveFlag,
                Status = v.DocStatus,
                DocumentId = v.DocumentId,
                RevisionNumber = v.DocumentRevNum
            };
        }
    }
}