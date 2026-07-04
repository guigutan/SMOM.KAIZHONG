using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Piles;
using SIE.Warehouses;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Common.DataQuery
{
    /// <summary>
    /// 垛表配置信息
    /// </summary>
    public class PileConfig
    {
        /// <summary>
        /// 垛表打印模板
        /// </summary>
        public PrintTemplate PrintTemplate { get; set; }

        /// <summary>
        /// 垛表编码配置信息
        /// </summary>
        public NoConfigValue ConfigValue { get; set; }
    }

    /// <summary>
    /// 垛表数据查询器
    /// </summary>
    public class PileDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取垛表编码配置
        /// </summary>
        /// <returns>object</returns>
        public virtual object GetPileConfig()
        {
            PileConfig pileConfig = new PileConfig();
            var printTemplates = GetPilePrintTemplate();
            if (printTemplates.Count == 1)
            {
                pileConfig.PrintTemplate = printTemplates.FirstOrDefault();
            }

            pileConfig.ConfigValue = RT.Service.Resolve<PileController>().GetPileCodeConfig();
            return pileConfig;
        }

        /// <summary>
        /// 获取垛表打印模板
        /// </summary>
        /// <returns>垛表打印模板</returns>
        public virtual EntityList<PrintTemplate> GetPilePrintTemplate()
        {
            string qualifiedName = typeof(PilePrintable).GetQualifiedName();
            var printTemplates = RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, string.Empty, null, SIE.Common.Prints.PrintType.Label);

            return printTemplates;
        }

        /// <summary>
        /// 批量生成垛表数据
        /// </summary>
        /// <param name="pileViewModel">批量生成ViewModel</param>
        /// <returns>垛表数据</returns>
        public virtual EntityList<SIE.Inventory.Piles.Pile> BatchGeneratePileData(BatchGeneratePileViewModel pileViewModel)
        {
            if (pileViewModel == null)
            {
                return new EntityList<SIE.Inventory.Piles.Pile>();
            }

            var pileCtl = RT.Service.Resolve<PileController>();
            var piles = pileCtl.BatchGeneratePileData(pileViewModel.TurnoverBoxModel, pileViewModel.GenerateQty);
            return piles;
        }

        /// <summary>
        /// 批量生成垛表数据
        /// </summary>
        /// <param name="pileViewModel">批量生成ViewModel</param>
        /// <returns>打印数据</returns>
        public virtual PrintDataCommon GenerateAndPrintPileData(BatchGeneratePileViewModel pileViewModel)
        {
            var result = new PrintDataCommon();

            if (pileViewModel == null)
            {
                return result;
            }

            var pileCtl = RT.Service.Resolve<PileController>();
            var piles = pileCtl.BatchGeneratePileData(pileViewModel.TurnoverBoxModel, pileViewModel.GenerateQty);

            PrintPiles(pileViewModel.TemplateId.Value, piles, result);

            return result;
        }

        /// <summary>
        /// 批量生成垛表数据
        /// </summary>
        /// <param name="ids">垛表ID集合</param>
        /// <param name="templateId">打印模板ID</param>
        /// <returns>打印数据</returns>
        public virtual PrintDataCommon PrintPileData(List<double> ids, double templateId)
        {
            var result = new PrintDataCommon();

            var pileCtl = RT.Service.Resolve<PileController>();
            var piles = pileCtl.GetPileByIds(ids);

            PrintPiles(templateId, piles, result);

            return result;
        }

        /// <summary>
        /// 打印垛表数据
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="piles">垛表数据</param>
        /// <param name="result">打印数据</param>
        /// <exception cref="ValidationException">异常信息</exception>
        private void PrintPiles(double templateId, EntityList<SIE.Inventory.Piles.Pile> piles, PrintDataCommon result)
        {
            // 1.获取打印模板
            var template = RF.GetById<PrintTemplate>(templateId);
            if (template == null)
            {
                throw new ValidationException("打印模板不存在!".L10N());
            }

            //2.根据类型获取报表处理对像
            if (template.State == State.Disable)
            {
                throw new ValidationException("当前模板已被禁用,请重新选择!".L10N());
            }

            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new PilePrintable();

            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            result.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<SIE.Inventory.Piles.Pile> printData = new List<SIE.Inventory.Piles.Pile>();
                printData.AddRange(piles);

                return printData;
            });

            result.Type = template.Type;
        }
    }
}
