using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipTypes;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.EMS.SpareParts.Commands;
using SIE.Web.EMS.SpareParts.SparePartTypes;
using System.Linq;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 备件基础数据视图配置
    /// </summary>
    internal class SparePartViewConfig : WebViewConfig<SparePart>
    {
        public const string SelectWinView = "SelectWinView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(SparePart.SparePartCodeProperty);
            View.DeclareExtendViewGroup(SelectionView);
            if (ViewGroup == SelectWinView)
            {
                SelectWin();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseClientOrder();
            View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.SparePartBehavior");
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.EMS.SpareParts.Commands.AddSparePartCommand");
            View.RemoveCommands(WebCommandNames.Copy);
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.ImportSparePartCommand");
            View.UseCommand("SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.UseCommand(typeof(ImportSparePartItemCommand).FullName);
            View.RemoveCommands(EnableCommand.CommandName);
            View.RemoveCommands(DisableCommand.CommandName);
            View.UseCommand(typeof(EnableSparePartCommand).FullName);
            View.UseCommand(typeof(DisableSparePartCommand).FullName);

            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
            View.Property(p => p.Specification);
            View.Property(p => p.ItemCategory)
                .UsePagingLookUpEditor((m, e) =>
                {
                    //m.DisplayField = ItemCategory.NameProperty.Name;
                    m.BindDisplayField = SparePart.ItemCategoryNameProperty.Name;
                });
            View.Property(p => p.SpartType);
            View.Property(p => p.SpartEquipType).UsePagingLookUpEditor((m, e) =>
            {
                //m.DisplayField = EquipType.TypeNameProperty.Name;
                m.BindDisplayField = SparePart.SparePartTypeNameProperty.Name;
            }); 
            View.Property(p => p.SpartEquipModel);
            View.Property(p => p.State);
            View.Property(p => p.ControlMethod);
            View.Property(p => p.ExemptionInspect).Readonly();
            View.Property(p => p.OriginalItemCode);
            View.Property(p => p.IsReplacement).Readonly();
            View.Property(p => p.Unit);
            View.Property(p => p.Manufacturer);
            View.Property(p => p.Supplier);
            View.Property(p => p.GoodNumber);
            View.Property(p => p.RotNumber);
            View.Property(p => p.SafeStock);
            View.Property(p => p.LifeTime);
            View.Property(p => p.UseTime).ShowInList(width: 130);
            View.Property(p => p.UnitPrice).UseSpinEditor(m => m.DecimalPrecision = 2);
            View.ChildrenProperty(p => p.PictureAttachmentList).HasLabel("图片");
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件");
        }

        private void SelectWin()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.Specification).Show();
                View.Property(p => p.ItemCategory).Show();
                View.Property(p => p.SpartType).Show();
                View.Property(p => p.SpartEquipType).Show();
                View.Property(p => p.SpartEquipModel).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.ControlMethod).Show();
                View.Property(p => p.ExemptionInspect).Show();
                View.Property(p => p.OriginalItemCode).Show();
                View.Property(p => p.IsReplacement).Show();
                View.Property(p => p.Unit).Show();
                View.Property(p => p.Manufacturer).Show();
                View.Property(p => p.Supplier).Show();
                View.Property(p => p.SafeStock).Show();
                View.Property(p => p.LifeTime).Show();
                View.Property(p => p.UseTime).Show();
                View.Property(p => p.UnitPrice).Show();
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommand(WebCommandNames.FormSave);
            View.HasDetailColumnsCount(4);
            View.Property(p => p.SparePartCode).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.SparePartName).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Specification);
            View.Property(p => p.ItemCategory).HasLabel("分类层级*").UseDataSource((e, c, r) =>
            {
                var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(SIE.Items.Items.CategoryType.Item, ItemType.SparePart, r, c);
                list.ForEach(p => p.TreePId = null);
                return list;
            }).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = ItemCategory.NameProperty.Name;
                    m.BindDisplayField = SparePart.ItemCategoryNameProperty.Name;
                });
            View.Property(p => p.SpartType).Cascade(p => p.SpartEquipType, null).Cascade(p => p.SpartEquipModel, null);
            View.Property(p => p.SpartEquipType)
                .Cascade(p => p.SpartEquipModel, null)
                .Readonly(p => p.SpartType != SIE.EMS.SpareParts.Enums.SparePartType.Special)
                .UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = EquipType.TypeNameProperty.Name;
                    m.BindDisplayField = SparePart.SparePartTypeNameProperty.Name;
                });
            View.Property(p => p.SpartEquipModel).UseDataSource((e, c, r) =>
            {
                var entity = e as SparePart;
                return RT.Service.Resolve<SparePartTypeController>().GetEquipModels(entity.SpartEquipTypeId, r, c);
            }).Readonly(p => p.SpartType != SIE.EMS.SpareParts.Enums.SparePartType.Special);
            View.Property(p => p.State).DefaultValue(1);
            View.Property(p => p.ControlMethod).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.ExemptionInspect);
            View.Property(p => p.Unit);
            View.Property(p => p.SafeStock).UseSpinEditor(m => m.MinValue = 0).DefaultValue(0);
            View.Property(p => p.Manufacturer);
            View.Property(p => p.Supplier);
            View.Property(p => p.OriginalItemCode);
            View.Property(p => p.LifeTime);
            View.Property(p => p.UseTime);
            View.Property(p => p.UnitPrice).UseSpinEditor(m => m.DecimalPrecision = 2);
            View.Property(p => p.IsReplacement);

            View.ChildrenProperty(p => p.PictureAttachmentList).HasLabel("图片").Show(ChildShowInWhere.All).ViewGroup = "FormEditView";
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件");
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
            View.Property(p => p.Specification);
            View.Property(p => p.ItemCategory);
            View.Property(p => p.SpartType);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.SparePartCode).ImportIndexer().HasLabel("备件编码");
            View.Property(p => p.SparePartName);
            View.Property(p => p.Specification);
            View.PropertyRef(p => p.ItemCategory.Code).HasLabel("分类层级");
            View.Property(p => p.SpartType);
            View.PropertyRef(p => p.SpartEquipType.TypeCode).HasLabel("设备类型编码");
            View.PropertyRef(p => p.SpartEquipModel.Code).HasLabel("设备型号编码");
            View.Property(p => p.State);
            View.Property(p => p.ControlMethod);
            View.Property(p => p.OriginalItemCode);
            View.Property(p => p.IsReplacement);
            View.PropertyRef(p => p.Unit.Name).HasLabel("单位");
            View.Property(p => p.Manufacturer);
            View.Property(p => p.Supplier);
            View.Property(p => p.SafeStock).UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = false;
                p.AllowBlank = true;
            });
            View.Property(p => p.LifeTime).UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = false;
                p.AllowBlank = true;
            });
            View.Property(p => p.UseTime).UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = false;
                p.AllowBlank = true;
            });
            View.Property(p => p.UnitPrice).UseSpinEditor(p =>
            {
                p.AllowNegative = true;
                p.AllowDecimals = false;
                p.AllowBlank = true;
            });
        }
    }
}