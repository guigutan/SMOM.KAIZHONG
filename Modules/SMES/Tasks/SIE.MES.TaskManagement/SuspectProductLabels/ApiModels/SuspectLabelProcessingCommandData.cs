using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ApiModels
{
    [Serializable]
    public class SuspectLabelProcessingCommandData
    {
        public List<ProcessingDefectDtlViewModel> details { get; set; } = new List<ProcessingDefectDtlViewModel>();

        public ProcessingViewModel viewModel { get; set; } = new ProcessingViewModel();

    }
}
