using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Barcodes.WipBatchs.Datas
{
    [Serializable]
    public class SyncWipBatchResponse
    {
        public string errMsg { get; set; }

        public List<SyncWipBatchFailResponse> failResponses { get; set; } = new List<SyncWipBatchFailResponse>();
    }

    [Serializable]
    public class SyncWipBatchFailResponse
    {
        public double Id { get; set; }

        public string Msg { get; set; }
    }
}
