using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯管理
    /// </summary>
    public class AndonManageCallMaterualViewConfig : WebViewConfig<AndonManageCallMaterial>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string LookupViewGroup = "LookupViewGroup";

        /// <summary>
        /// 是否叫料视图
        /// </summary>
        public const string ShowViewGroup = "ShowViewGroup";


        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AndonManage));
            View.DeclareExtendViewGroup(LookupViewGroup, ShowViewGroup);
            if (ViewGroup == LookupViewGroup)
            {
                LookUpView();
            }
            if (ViewGroup == ShowViewGroup)
            {
                ShowView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList().HasLabel("物料编码");
                View.Property(p => p.ItemName).ShowInList();
                View.Property(p => p.ConsumeType).ShowInList();
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.TimeNeed).ShowInList();
                View.Property(p => p.WareHouse).ShowInList();
                View.Property(p => p.StorageLocation).ShowInList();
                View.Property(p => p.No).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
        
        /// <summary>
        /// 表单视图
        /// </summary>
        protected void ShowView()
        {
            View.UseCommands(typeof(AndonManageCallMaterialAddCommand).FullName, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseDataSource((s, p, k) =>
                {
                    var source = s as AndonManageCallMaterial;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().ChoseItems(source, p, k);
                    }
                    else
                    {
                        return new EntityList<Item>();
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ConsumeType), nameof(e.Item.ConsumeMode));
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList().HasLabel("物料编码").UseListSetting(p=>p.HelpInfo= "与选中的工单维护的工序BOM和工单BOM有关".L10N());
                View.Property(p => p.ItemName).ShowInList().Readonly();
                View.Property(p => p.ConsumeType).ShowInList().Readonly();
                View.Property(p => p.Qty).UseSpinEditor(p => { p.MinValue = 0; }).ShowInList();
                View.Property(p => p.TimeNeed).ShowInList().DefaultValue(DateTime.Now);
                View.Property(p => p.WareHouse).ShowInList().Readonly(p => !p.Hand);
                View.Property(p => p.StorageLocation).UseDataSource((s, p, k) =>
                {
                    var source = s as AndonManageCallMaterial;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetStorageLocations(source, p, k);
                    }
                    else
                    {
                        return new EntityList<StorageLocation>();
                    }
                }).ShowInList().Readonly(p => !p.Hand);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void LookUpView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList().HasLabel("物料编码").Readonly();
                View.Property(p => p.ItemName).ShowInList().Readonly();
                View.Property(p => p.ConsumeType).ShowInList().Readonly();
                View.Property(p => p.Qty).ShowInList().Readonly();
                View.Property(p => p.TimeNeed).ShowInList().Readonly();
                View.Property(p => p.WareHouse).ShowInList().Readonly();
                View.Property(p => p.StorageLocation).ShowInList().Readonly();
                View.Property(p => p.No).ShowInList().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
