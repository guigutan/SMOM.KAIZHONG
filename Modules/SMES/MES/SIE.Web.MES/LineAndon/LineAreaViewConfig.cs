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
    public class LineAreaViewConfig : WebViewConfig<LineArea>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LineArea));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(LineAreaImportCommand).FullName, typeof(LineAreaDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.MachineCode).ShowInList(width: 150).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);
                View.Property(p => p.MachineName).ShowInList(width: 150);
                View.Property(p => p.Equipment).UsePagingLookUpEditor((c, p) => {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(p.EquipmentName), nameof(p.Equipment.Name));
                    dic.Add(nameof(p.EquipmentDate), nameof(p.Equipment.PurchaseDate));
                    c.DicLinkField = dic;
                });
                View.Property(p => p.EquipmentDate).Readonly();
                View.Property(p => p.WorkCenter).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList().UseFactoryEditor();
                View.Property(p => p.WorkShop).ShowInList().UseResourceWorkShopEditor();
                View.Property(p => p.WorkShopCode).ShowInList().Readonly();
                View.Property(p => p.AndonUphold).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AndonCode), nameof(e.AndonUphold.AndonCode));
                    m.DicLinkField = keyValues;
                    m.ReloadDataOnPopping = true;
                }).ShowInList(width: 200);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.MachineCode).Show();
                View.Property(p => p.MachineName).Show();
                View.Property(p => p.EquipmentNo).ShowInList(width: 150).HasLabel("设备编码");
                View.Property(p => p.WorkCenterCode).HasLabel("工作中心");
                View.Property(p => p.FactoryCode).HasLabel("工厂编码");
                View.Property(p => p.WorkShopCode).HasLabel("车间编码");
                View.Property(p => p.AndonDesc).HasLabel("区域描述");
                View.Property(p => p.AndonCode).Show();
            }
        }

    }
}
