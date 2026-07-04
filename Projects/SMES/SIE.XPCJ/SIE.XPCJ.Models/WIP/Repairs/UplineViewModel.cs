using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Repairs
{
    [Serializable]
    public class UplineViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UplineViewModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewModel">维修视图模型</param>
        public UplineViewModel(List<GotoProcessViewModel> process)
        {
            //默认上线工序
            UplineProcess = process.FirstOrDefault(p => p.IsDefault);

            ProcessList.AddRange(process);
        }

        /// <summary>
        /// 上线工序ID
        /// </summary>
        public string UplineProcessId
        {
            get; set;
        }

        /// <summary>
        /// 上线工序
        /// </summary>
        public GotoProcessViewModel UplineProcess
        {
            get; set;
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public List<GotoProcessViewModel> ProcessList { get; set; } = new List<GotoProcessViewModel>();

    }
}
