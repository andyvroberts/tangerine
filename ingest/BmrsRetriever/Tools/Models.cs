
using System.Xml.Serialization;

namespace BmrsRetriever
{
    // Imbalance Price
    [XmlRoot("response")]
    public class B1770Response
    {
        [XmlElement("responseMetadata")]
        public B1770Metadata? Meta { get; set; }
        [XmlElement("responseBody")]
        public B1770ResponseBody? Body { get; set; }

        public bool IsUsable
        {
            get
            {
                if (Meta != null && Body != null)
                {
                    if (Meta.HttpCode == "200")
                    {
                        if (Body.ItemList != null & Body.ItemList?.Items?.Length > 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }
    }

    [XmlType("responseMetadata")]
    public class B1770Metadata
    {
        [XmlElement(ElementName = "httpCode")]
        public string? HttpCode { get; set; }
        [XmlElement(ElementName = "errorType")]
        public string? ErrorType { get; set; }
        [XmlElement(ElementName = "description")]
        public string? Description { get; set; }
        [XmlElement(ElementName = "cappingApplied")]
        public string? CappingApplied { get; set; }
        [XmlElement(ElementName = "cappingLimit")]
        public int CappingLimit { get; set; }
        [XmlElement(ElementName = "queryString")]
        public string? QueryString { get; set; }
    }

    [XmlType("responseBody")]
    public class B1770ResponseBody
    {
        [XmlElement(ElementName = "dataItem")]
        public string? DataItem { get; set; }
        [XmlElement("responseList")]
        public B1770ResponseList? ItemList { get; set; }
    }

    [XmlType("responseList")]
    public class B1770ResponseList
    {
        [XmlElement("item")]
        public ImbalancePriceItem[]? Items { get; set; }
    }

    [XmlType("item")]
    public class ImbalancePriceItem
    {
        [XmlElement(ElementName = "timeSeriesID")]
        public string? TimeSeriesId { get; set; }
        [XmlElement(ElementName = "businessType")]
        public string? BusinessType { get; set; }
        [XmlElement(ElementName = "controlArea")]
        public string? ControlArea { get; set; }
        [XmlElement(ElementName = "settlementDate")]
        public string? SettlementDate { get; set; }
        [XmlElement(ElementName = "settlementPeriod")]
        public int SettlementPeriod { get; set; }
        [XmlElement(ElementName = "imbalancePriceAmountGBP")]
        public double ImbalancePriceAmountGbp { get; set; }
        [XmlElement(ElementName = "priceCategory")]
        public string? PriceCategory { get; set; }
        [XmlElement(ElementName = "curveType")]
        public string? CurveType { get; set; }
        [XmlElement(ElementName = "resolution")]
        public string? Resolution { get; set; }
        [XmlElement(ElementName = "documentType")]
        public string? DocumentType { get; set; }
        [XmlElement(ElementName = "processType")]
        public string? ProcessType { get; set; }
        [XmlElement(ElementName = "activeFlag")]
        public string? ActiveFlag { get; set; }
        [XmlElement(ElementName = "docStatus")]
        public string? DocStatus { get; set; }
        [XmlElement(ElementName = "documentID")]
        public string? DocumentId { get; set; }
        [XmlElement(ElementName = "documentRevNum")]
        public int DocumentRevNum { get; set; }
    }

    // imbalance volume
    [XmlRoot("response")]
    public class B1780Response
    {
        [XmlElement("responseMetadata")]
        public B1780Metadata? Meta { get; set; }
        [XmlElement("responseBody")]
        public B1780ResponseBody? Body { get; set; }

        public bool IsUsable
        {
            get
            {
                if (Meta != null && Body != null)
                {
                    if (Meta.HttpCode == "200")
                    {
                        if (Body.ItemList != null & Body.ItemList?.Items?.Length > 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }
    }

    [XmlType("responseMetadata")]
    public class B1780Metadata
    {
        [XmlElement(ElementName = "httpCode")]
        public string? HttpCode { get; set; }
        [XmlElement(ElementName = "errorType")]
        public string? ErrorType { get; set; }
        [XmlElement(ElementName = "description")]
        public string? Description { get; set; }
        [XmlElement(ElementName = "cappingApplied")]
        public string? CappingApplied { get; set; }
        [XmlElement(ElementName = "cappingLimit")]
        public int CappingLimit { get; set; }
        [XmlElement(ElementName = "queryString")]
        public string? QueryString { get; set; }
    }

    [XmlType("responseBody")]
    public class B1780ResponseBody
    {
        [XmlElement(ElementName = "dataItem")]
        public string? DataItem { get; set; }
        [XmlElement("responseList")]
        public B1780ResponseList? ItemList { get; set; }
    }

    [XmlType("responseList")]
    public class B1780ResponseList
    {
        [XmlElement("item")]
        public ImbalanceVolumeItem[]? Items { get; set; }
    }

    [XmlType("item")]
    public class ImbalanceVolumeItem
    {
        [XmlElement(ElementName = "timeSeriesId")]
        public string? TimeSeriesId { get; set; }
        [XmlElement(ElementName = "businessType")]
        public string? BusinessType { get; set; }
        [XmlElement(ElementName = "controlArea")]
        public string? ControlArea { get; set; }
        [XmlElement(ElementName = "settlementDate")]
        public string? SettlementDate { get; set; }
        [XmlElement(ElementName = "settlementPeriod")]
        public int SettlementPeriod { get; set; }
        [XmlElement(ElementName = "imbalanceQuantityMAW")]
        public double ImbalanceQuantityMaw { get; set; }
        [XmlElement(ElementName = "curveType")]
        public string? CurveType { get; set; }
        [XmlElement(ElementName = "resolution")]
        public string? Resolution { get; set; }
        [XmlElement(ElementName = "documentType")]
        public string? DocumentType { get; set; }
        [XmlElement(ElementName = "processType")]
        public string? ProcessType { get; set; }
        [XmlElement(ElementName = "activeFlag")]
        public string? ActiveFlag { get; set; }
        [XmlElement(ElementName = "docStatus")]
        public string? DocStatus { get; set; }
        [XmlElement(ElementName = "documentID")]
        public string? DocumentId { get; set; }
        [XmlElement(ElementName = "documentRevNum")]
        public int DocumentRevNum { get; set; }
        [XmlElement(ElementName = "imbalanceQuantityDirection", IsNullable = true)]
        public string? ImbalanceDirection { get; set; }
    }
}