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
        // starting Child tag
        public int BatchGeneralConvos { get; set; } = 0;
        public int BatchGeneralServiceTime { get; set; } = 0;
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
