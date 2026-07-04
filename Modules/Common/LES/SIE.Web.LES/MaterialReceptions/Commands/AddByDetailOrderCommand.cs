using SIE.Domain.Validation;
using SIE.LES.MaterialReceptions.Controllers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.LES.MaterialReceptions.APIModels;

namespace SIE.Web.LES.MaterialReceptions.Commands
{
    /// <summary>
    /// 按明细接收添加
    /// </summary>
    public class AddByDetailOrderCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var webData = args.Data.ToJsonObject<FromWebData>();
            var item = RT.Service.Resolve<MaterialReceptionController>().AddByDetailorOrder(webData.LabelNo, webData.ScanType, webData.ScanRecords);
            return item;
        }
    }

    /// <summary>
    /// web前端传入扫描的号码和已扫列表
    /// </summary>
    [Serializable]
    public class FromWebData
    {
        /// <summary>
        /// 扫描号码
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 扫描方式
        /// </summary>
        public int ScanType { get; set; }

        /// <summary>
        /// 已扫列表
        /// </summary>
        public List<MaterialReceptionAddViewModel> ScanRecords { get; set; }
    }
}
