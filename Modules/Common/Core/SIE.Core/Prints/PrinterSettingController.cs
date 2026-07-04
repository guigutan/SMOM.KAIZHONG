using SIE.Domain;
using System.Linq;

namespace SIE.Core.Prints
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class PrinterSettingController : DomainController
    {
        /// <summary>
        /// 初始化默认模板
        /// </summary>
        public virtual void InitDefaultTpl()
        {
            var tplName = "默认模板设置";
            var tpl = Query<PrinterSettingTpl>().Where(p => p.TplName == tplName).FirstOrDefault();
            if (tpl != null)
                return;
            tpl = CreatePrinterSettingTpl(tplName);
            RF.Save(tpl);
        }

        /// <summary>
        /// 创建打印参数设置
        /// </summary>
        /// <param name="tplName"></param>
        /// <returns></returns>
        public virtual PrinterSettingTpl CreatePrinterSettingTpl(string tplName, double? empId = null, string productCode = null)
        {
            var tpl = new PrinterSettingTpl()
            {
                EmployeeId = empId,
                ProductCode = productCode,

                TplName = tplName,
                PageWidth = 80,
                PageHeight = 60,
                MarginsLeft = 0,
                MarginsRight = 0,
                MarginsBottom = 0,
                MarginsTop = 0,

                Resolution = 400,

                QrcodeX = 5,
                QrcodeY = 8,
                QrcodeWidth = 40,
                QrcodeHeight = 40,

                ProjectFontName = "Arial",
                ProjectFontSize = 6,
                ProjectFontBold = true,
                ProjectNameX = 2,
                ProjectNameY = 48,

                CodeStrX = 45,
                CodeStrY = 5,
                CodeStrFontName = "Arial",
                CodeStrFontSize = 6,
                CodeStrFontBold = true,
                CodeStrLineSize = 6,
                CodeStrLineHeight = 8,
            };
            return tpl;
        }

        /// <summary>
        /// 获取员工产品打印设置参数
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public virtual PrinterSettingTpl GetPrinterSettingTpl(double? empId, string productCode)
        {
            var tpl = Query<PrinterSettingTpl>().Where(p => p.EmployeeId == empId && p.ProductCode == productCode).FirstOrDefault();
            return tpl;
        }
    }
}
