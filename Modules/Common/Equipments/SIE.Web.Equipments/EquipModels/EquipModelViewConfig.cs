using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Equipments.EquipModels.Commands;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号视图
    /// </summary>
    public class EquipModelViewConfig : WebViewConfig<EquipModel>
    {
        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        public const string EISBaseDataViewGroup = "EISBaseDataViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(EISBaseDataViewGroup);
            if (ViewGroup == EISBaseDataViewGroup) { EISBaseDataView(); }

        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipModel));
            View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.Copy);
            View.UseCommands(typeof(EquipModelSaveCommand).FullName, typeof(EquipModelImportCommand).FullName);
            View.UseCommands("SIE.Web.Equipments.EquipModels.Commands.OpenEquipAccountCommand",
                "SIE.Web.Equipments.EquipModels.Commands.OpenEquipTypeCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Specifications).ShowInList(width: 80);
                View.Property(p => p.Manufacturers).ShowInList(width: 80);
                View.Property(p => p.EquipTypeId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.TypeName), nameof(e.EquipType.TypeName));
                    m.DicLinkField = keyValues;
                }).HasLabel("设备类型编码").ShowInList(width: 120);
                View.Property(p => p.TypeName).Readonly().HasLabel("设备类型").ShowInList(width: 80);

                View.Property(p => p.TypeCategory).UseCatalogEditor(c => {c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).ShowInList(width: 80);

                View.Property(p => p.IndustryCategory);
            }


            #region 电子行业页签

            View.AttachDetailChildrenProperty(typeof(EquipModel), (c) =>
            {
                var model = c.Parent as EquipModel;
                model = RF.GetById<EquipModel>(model.Id, new EagerLoadOptions().LoadWithViewProperty());
                return model;
            }, EISBaseDataViewGroup).HasLabel("电子行业").Show(ChildShowInWhere.All).HasOrderNo(210);
            View.ChildrenProperty(p => p.LocationList).HasLabel("位置列表").Show(ChildShowInWhere.All).HasOrderNo(220);
            #endregion

        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(EquipModelSaveCommand).FullName);
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.Equipments.EquipModels.Scripts.EquipModelBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Specifications);
                View.Property(p => p.Manufacturers);
                View.Property(p => p.EquipTypeId).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipModelController>().GetEquipTypes(p, k, true);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.TypeName), nameof(e.EquipType.TypeName));
                    m.DicLinkField = keyValues;
                }).HasLabel("设备类型编码");
                View.Property(p => p.TypeName).Readonly().HasLabel("设备类型");
                View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; });
                View.Property(p => p.IndustryCategory);
            }

            #region 电子行业页签

            View.AttachDetailChildrenProperty(typeof(EquipModel), (c) =>
            {
                var model = c.Parent as EquipModel;
                model = RF.GetById<EquipModel>(model.Id, new EagerLoadOptions().LoadWithViewProperty());
                return model;
            }, EISBaseDataViewGroup).HasLabel("电子行业").Show(ChildShowInWhere.All).HasOrderNo(210);

            View.ChildrenProperty(p => p.LocationList).HasLabel("位置列表").Show(ChildShowInWhere.All).HasOrderNo(220);
            #endregion
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.DisableEditing();
            View.Property(p => p.Code).HasLabel("型号编码");
            View.Property(p => p.Name).HasLabel("型号名称");
            View.Property(p => p.TypeCode);
            View.Property(p => p.TypeName);
            View.Property(p => p.TypeCategory);
            View.Property(p => p.Specifications);
            View.Property(p => p.Manufacturers);
        }

        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        protected void EISBaseDataView()
        {
            View.ClearCommands();
            View.DefineFormChildSaveMode(FormChildSaveMode.Save);
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.RailType).Show();
                View.Property(p => p.VirtualDevice).DefaultValue(YesNo.No).Show();
                View.Property(p => p.FeederBinding).DefaultValue(YesNo.No).Show();
                View.Property(p => p.FeederLocFailSafe).DefaultValue(State.Disable).Show();
                View.Property(p => p.FeederBarcodeFailSafe).DefaultValue(State.Disable).Show();
                View.Property(p => p.IsDisabled).DefaultValue(YesNo.No).Show();
                View.Property(p => p.AgingType).Show();
                View.Property(p => p.ProductionType).Show();
            }
        }

        /// <summary>
        /// 配置数据导入的视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).ImportIndexer(true).HasLabel("设备型号编码*");
            View.Property(p => p.Name).HasLabel("设备型号名称*");
            View.Property(p => p.Specifications).HasLabel("技术规格");
            View.Property(p => p.Manufacturers).HasLabel("生产厂商");
            View.PropertyRef(p => p.EquipType.TypeCode).HasLabel("设备类型编码*");
            View.Property(p => p.IndustryCategory).HasLabel("行业属性*");
            View.Property(p => p.RailType).HasLabel("轨道类型");
            View.Property(p => p.FeederBinding).HasLabel("是否Feeder绑定");
            View.Property(p => p.FeederLocFailSafe).HasLabel("启用站位防错");
            View.Property(p => p.FeederBarcodeFailSafe).HasLabel("启用Feeder防错");
            View.Property(p => p.VirtualDevice).HasLabel("虚拟设备");
            View.Property(p => p.IsDisabled).HasLabel("禁用");
            View.Property(p => p.AgingType).HasLabel("老化方式");
            View.Property(p => p.ProductionType).HasLabel("产品生产模式");
        }
    }
}
