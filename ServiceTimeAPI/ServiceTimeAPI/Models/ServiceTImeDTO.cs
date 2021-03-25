using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTimeAPI.Models
{
    public class ProChartServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }

        public int AATConvos { get; set; } = 0;
        public int AATServiceTime { get; set; } = 0;
        public int AllocationsConvos { get; set; } = 0;
        public int AllocationsServiceTime { get; set; } = 0;
        public int BatchConvos { get; set; } = 0;
        public int BatchServiceTime { get; set; } = 0;
        public int ChartProcessConvos { get; set; } = 0;
        public int ChartProcessServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int FTPConvos { get; set; } = 0;
        public int FTPServiceTime { get; set; } = 0;
        public int GasAnaConvos { get; set; } = 0;
        public int GasAnaServiceTime { get; set; } = 0;
        public int LabelRequestConvos { get; set; } = 0;
        public int LabelRequestServiceTime { get; set; } = 0;
        public int MeterConvos { get; set; } = 0;
        public int MeterServiceTime { get; set; } = 0;
        public int ReadSlipsConvos { get; set; } = 0;
        public int ReadSlipsServiceTime { get; set; } = 0;
        public int ReportsConvos { get; set; } = 0;
        public int ReportsServiceTime { get; set; } = 0;
        public int ScannerConvos { get; set; } = 0;
        public int ScannerServiceTime { get; set; } = 0;
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        // starting Child tags
        public int BatchGeneralConvos { get; set; } = 0;
        public int BatchGeneralServiceTime { get; set; } = 0;
        public int BatchDeleteConvos { get; set; } = 0;
        public int BatchDeleteServiceTime { get; set; } = 0;
        public int BatchErrorConvos { get; set; } = 0;
        public int BatchErrorServiceTime { get; set; } = 0;
        public int BatchInquiryConvos { get; set; } = 0;
        public int BatchInquiryServiceTime { get; set; } = 0;
        public int BatchRushConvos { get; set; } = 0;
        public int BatchRushServiceTime { get; set; } = 0;
        public int BatchReScanConvos { get; set; } = 0;
        public int BatchReScanServiceTime { get; set; } = 0;
        public int ChartPickupConvos { get; set; } = 0;
        public int ChartPickupServiceTime { get; set; } = 0;
        public int ChartAuditsConvos { get; set; } = 0;
        public int ChartAuditsServiceTime { get; set; } = 0; 
        public int ChartReviewConvos { get; set; } = 0;
        public int ChartReviewServiceTime { get; set; } = 0; 
        public int RPRConvos { get; set; } = 0;
        public int RPRServiceTime { get; set; } = 0;
        public int GasAnaUpdatesConvos { get; set; } = 0;
        public int GasAnaUpdatesServiceTime { get; set; } = 0;
        public int MeterSetupConvos { get; set; } = 0;
        public int MeterSetupServiceTime { get; set; } = 0;
        public int MeterMovesConvos { get; set; } = 0;
        public int MeterMovesServiceTime { get; set; } = 0;
        public int ShutInConvos { get; set; } = 0;
        public int ShutInServiceTime { get; set; } = 0;
        public int RSMEConvos { get; set; } = 0;
        public int RSMEServiceTime { get; set; } = 0;
        public int ReadSheetConvos { get; set; } = 0;
        public int ReadSheetServiceTime { get; set; } = 0;
        public int ProChartReportConvos { get; set; } = 0;
        public int ProChartReportServiceTime { get; set; } = 0;
        public int NewScanReqConvos { get; set; } = 0;
        public int NewScanReqServiceTime { get; set; } = 0;
        public int ScannerPartsConvos { get; set; } = 0;
        public int ScannerPartsServiceTime { get; set; } = 0;
        public int ShippingScannerConvos { get; set; } = 0;
        public int ShippingScannerServiceTime { get; set; } = 0;
        public int ACISConvos { get; set; } = 0;
        public int ACISServiceTime { get; set; } = 0;
        public int STConvos { get; set; } = 0;
        public int STServiceTime { get; set; } = 0;
        public int AppAccessConvos { get; set; } = 0;
        public int AppAccessServiceTime { get; set; } = 0;
        public int ClientDeactConvos { get; set; } = 0;
        public int ClientDeactServiceTime { get; set; } = 0;
        public int ExistclientConvos { get; set; } = 0;
        public int ExistclientServiceTime { get; set; } = 0;
        public int NewClientActConvos { get; set; } = 0;
        public int NewClientActServiceTime { get; set; } = 0;
        public int TempPwReqConvos { get; set; } = 0;
        public int TempPwReqServiceTime { get; set; } = 0;
        // starting grand child tags
        public int CSRConvos { get; set; } = 0;
        public int CSRServiceTime { get; set; } = 0;
        public int CSREConvos { get; set; } = 0;
        public int CSREServiceTime { get; set; } = 0;
        public int CSRTSConvos { get; set; } = 0;
        public int CSRTSServiceTime { get; set; } = 0;
        public int CustomReportsConvos { get; set; } = 0;
        public int CustomReportsServiceTime { get; set; } = 0;
        public int ChartEvalConvos { get; set; } = 0;
        public int ChartEvalServiceTime { get; set; } = 0;
        public int ChartStatusConvos { get; set; } = 0;
        public int ChartStatusServiceTime { get; set; } = 0;
        public int DeliveryFilesConvos { get; set; } = 0;
        public int DeliveryFilesServiceTime { get; set; } = 0;
        public int EDIConvos { get; set; } = 0;
        public int EDIServiceTime { get; set; } = 0;
        public int EstimatesConvos { get; set; } = 0;
        public int EstimatesServiceTime { get; set; } = 0;
        public int GasmetMeasuredConvos { get; set; } = 0;
        public int GasmetMeasuredServiceTime { get; set; } = 0;
        public int GasmetTestedConvos { get; set; } = 0;
        public int GasmetTestedServiceTime { get; set; } = 0;
        public int MDEConvos { get; set; } = 0;
        public int MDEServiceTime { get; set; } = 0;
        public int MECRConvos { get; set; } = 0;
        public int MECRServiceTime { get; set; } = 0;
        public int PrismMeasuredConvos { get; set; } = 0;
        public int PrismMeasuredServiceTime { get; set; } = 0;
        public int ProductionSummaryConvos { get; set; } = 0;
        public int ProductionSummaryServiceTime { get; set; } = 0;
        public int ReceiptFilesConvos { get; set; } = 0;
        public int ReceiptFilesServiceTime { get; set; } = 0;
        public int UpdatesConvos { get; set; } = 0;
        public int UpdatesServiceTime { get; set; } = 0;
        public int VarianceConvos { get; set; } = 0;
        public int VarianceServiceTime { get; set; } = 0;
        public int VDRConvos { get; set; } = 0;
        public int VDRServiceTime { get; set; } = 0;
        public int VolumeSummaryConvos { get; set; } = 0;
        public int VolumeSummaryServiceTime { get; set; } = 0;
        public int WCRConvos { get; set; } = 0;
        public int WCRServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
    }
    public class NetFlowServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }

        public int USRConvos { get; set; } = 0;
        public int USRServiceTime { get; set; } = 0;
        public int AddRegConvos { get; set; } = 0;
        public int AddRegServiceTime { get; set; } = 0;
        public int AMSConvos { get; set; } = 0;
        public int AMSServiceTime { get; set; } = 0;
        public int AppInqConvos { get; set; } = 0;
        public int AppInqServiceTime { get; set; } = 0;
        public int AATConvos { get; set; } = 0;
        public int AATServiceTime { get; set; } = 0;
        public int CommsConvos { get; set; } = 0;
        public int CommsServiceTime { get; set; } = 0;
        public int CommissionConvos { get; set; } = 0;
        public int CommissionServiceTime { get; set; } = 0;
        public int ContractConvos { get; set; } = 0;
        public int ContractServiceTime { get; set; } = 0;
        public int DecommissionConvos { get; set; } = 0;
        public int DecommissionServiceTime { get; set; } = 0;
        public int ModemsConvos { get; set; } = 0;
        public int ModemsServiceTime { get; set; } = 0;
        public int NFTSConvos { get; set; } = 0;
        public int NFTSServiceTime { get; set; } = 0;
        public int ReportsConvos { get; set; } = 0;
        public int ReportsServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int UpdateConvos { get; set; } = 0;
        public int UpdateServiceTime { get; set; } = 0;
        public int ImpConvos { get; set; } = 0;
        public int ImpServiceTime { get; set; } = 0;
        public int MetersConvos { get; set; } = 0;
        public int MetersServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
    }
    public class ProTrendServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
        public int EAUConvos { get; set; } = 0;
        public int EAUServiceTime { get; set; } = 0;
        public int CMDConvos { get; set; } = 0;
        public int CMDServiceTime { get; set; } = 0;
        public int ClientSupConvos { get; set; } = 0;
        public int ClientSupServiceTime { get; set; } = 0;
        public int DataExpConvos { get; set; } = 0;
        public int DataExpServiceTime { get; set; } = 0;
        public int DataImpConvos { get; set; } = 0;
        public int DataImpServiceTime { get; set; } = 0;
        public int DRGConvos { get; set; } = 0;
        public int DRGServiceTime { get; set; } = 0;
        public int DataValConvos { get; set; } = 0;
        public int DataValServiceTime { get; set; } = 0;
        public int DevReqConvos { get; set; } = 0;
        public int DevReqServiceTime { get; set; } = 0;
        public int BFTConvos { get; set; } = 0;
        public int BFTServiceTime { get; set; } = 0;
        public int ImpConvos { get; set; } = 0;
        public int ImpServiceTime { get; set; } = 0;
        public int PFSConvos { get; set; } = 0;
        public int PFSServiceTime { get; set; } = 0;
        public int SchConvos { get; set; } = 0;
        public int SchServiceTime { get; set; } = 0;
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
    }
    public class ProMonitorServiceTimeDTO
    {
        public int Convos { get; set; } = 0;
        public int ServiceTime { get; set; } = 0;

        public int BillableConvos { get; set; } = 0;
        public int BillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> BillableTagServiceTime { get; set; }

        public int NonBillableConvos { get; set; } = 0;
        public int NonBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> NonBillableTagServiceTime { get; set; }

        public int UnknownBillableConvos { get; set; } = 0;
        public int UnknownBillableServiceTime { get; set; } = 0;
        //public Dictionary<string, int> UnknownBillableTagServiceTime { get; set; }
        public int USRUConvos { get; set; } = 0;
        public int USRUServiceTime { get; set; } = 0;
        public int DataImpConvos { get; set; } = 0;
        public int DataImpServiceTime { get; set; } = 0;
        public int NoParentConvos { get; set; } = 0;
        public int NoParentServiceTime { get; set; } = 0;
    }
}
