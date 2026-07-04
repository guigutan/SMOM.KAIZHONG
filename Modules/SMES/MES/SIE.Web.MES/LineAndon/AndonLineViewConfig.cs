using SIE.MES.LineAndon;
using SIE.MetaModel.View;
using SIE.Web.MES.LineAndon.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.LineAndon
{
    /// <summary>
    /// 产线与安灯区域
    /// </summary>
    public class AndonLineViewConfig : WebViewConfig<AndonLine>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(/*WebCommandNames.Add,*/ WebCommandNames.Edit, WebCommandNames.Save);
            //View.UseCommands(typeof(AndonLineImportCommand).FullName, typeof(AndonLineDLTemplateCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(AdonlineLabelPrintCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Seq).ShowInList();
                View.Property(p => p.MachineName).ShowInList(width: 150).Readonly();
                View.Property(p => p.MachineCode).ShowInList(width: 150).Readonly();
                //View.Property(p => p.Equipment).UseDataSource((o, e, r) =>
                //{
                //    return RT.Service.Resolve<AndonLineController>().GetEquipAccounts(r, e);
                //}).UsePagingLookUpEditor().ShowInList(width: 150);
                View.Property(p => p.Equipment).UsePagingLookUpEditor((c, p) => {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(p.EquipmentName), nameof(p.Equipment.Name));
                    dic.Add(nameof(p.EquipmentDate), nameof(p.Equipment.PurchaseDate));
                    c.DicLinkField = dic;
                }).Readonly();
                View.Property(p => p.EquipmentDate).Readonly();
                View.Property(p => p.WorkCenter).ShowInList(width: 150).Readonly();
                View.Property(p => p.Factory).ShowInList().UseFactoryEditor().Readonly();
                View.Property(p => p.WorkShopCode).ShowInList().Readonly();
                View.Property(p => p.WorkShop).ShowInList().UseResourceWorkShopEditor().Readonly();
                View.Property(p => p.AndonUphold).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AndonCode), nameof(e.AndonUphold.AndonCode));
                    m.DicLinkField = keyValues;
                    m.ReloadDataOnPopping = true;
                }).ShowInList(width: 200).Readonly();
                View.Property(p => p.AndonCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.IsLocalPrint).ShowInList();
                View.Property(p => p.PrinterIp).ShowInList(width: 150);
                View.Property(p => p.AndonEntity).ShowInList(width: 150);
                View.Property(p => p.AndonOrder).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.MachineName).ShowInList(width: 150);
            View.Property(p => p.MachineCode).ShowInList(width: 150);
            View.Property(p => p.EquipmentNo).ShowInList(width: 150);
            View.Property(p => p.WorkCenterCode).HasLabel("工作中心");
            View.Property(p => p.FactoryCode).HasLabel("工厂编码");
            View.Property(p => p.WorkShopCode).HasLabel("车间编码");
            View.Property(p => p.AndonDesc).HasLabel("区域描述");
            View.Property(p => p.AndonCode).ShowInList(width: 150);
            View.Property(p => p.PrinterIp).ShowInList(width: 150);
            View.Property(p => p.AndonEntity).ShowInList(width: 150);
            View.Property(p => p.AndonOrder).ShowInList(width: 150);
        }

        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Seq).ShowInList().Readonly();
                View.Property(p => p.MachineName).ShowInList(width: 150).Readonly();
                View.Property(p => p.MachineCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.Equipment).Show().Readonly();
                View.Property(p => p.EquipmentDate).Show().Readonly();
                View.Property(p => p.WorkCenter).ShowInList(width: 150).Readonly();
                View.Property(p => p.Factory).ShowInList().UseFactoryEditor().Readonly();
                View.Property(p => p.WorkShop).ShowInList().UseResourceWorkShopEditor().Readonly();
                View.Property(p => p.AndonUphold).Show().Readonly();
                View.Property(p => p.AndonCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.IsLocalPrint).ShowInList().Readonly();
                View.Property(p => p.PrinterIp).ShowInList(width: 150).Readonly();
                View.Property(p => p.AndonEntity).ShowInList(width: 150).Readonly();
                View.Property(p => p.AndonOrder).ShowInList(width: 150).Readonly();
            }
        }
    }
}
