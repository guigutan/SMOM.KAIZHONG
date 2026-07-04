using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Fixtures._Extentions_;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
	/// 工治具型号视图配置
	/// </summary>
	public class FixtureModelViewConfig : WebViewConfig<FixtureModel>
    {

        private const int displayCoulmnWidth = 20;
        /// <summary>
        /// 电子基础数据
        /// </summary>
        public const string EISBaseDataViewGroup = "EIS_BaseData_ViewGroup";
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EISBaseDataViewGroup);

            if (ViewGroup == EISBaseDataViewGroup)
            {
                EISBaseDataView();
            }
            View.FormEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Copy);
            View.UseCommands(WebCommandNames.Save, typeof(SIE.Web.Common.Import.Commands.ImportExcelCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.AddBehavior("SIE.Web.Fixtures.Models.Scripts.FixtureModelBehavior");
            View.Property(p => p.Code).Readonly().ShowInList(displayCoulmnWidth * 8);
            View.Property(p => p.Name).Readonly().ShowInList(displayCoulmnWidth * 8);
            View.Property(p => p.FixtureType).Readonly().ShowInList(displayCoulmnWidth * 5);
            View.Property(p => p.ManageMode).UseEnumEditor().Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.FixedStorage).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.LoadingManage).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.BindProduct).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.BindEquip).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.UnitId).HasLabel("单位").Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.IndustryProperties).UseEnumEditor().Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.MaxUseNum).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.MaxUseHour).Readonly().ShowInList(displayCoulmnWidth * 7);
            View.Property(p => p.MaintainNum).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.WarnNum).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.MaintainHour).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.WarnHour).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.OnlineHour).Readonly().ShowInList(displayCoulmnWidth * 10);
            View.Property(p => p.MaintainEnforce).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipmentList).HasLabel("设备清单").HasOrderNo(10);
            View.ChildrenProperty(p => p.MaintainProjectList).HasLabel("工治具保养项目").HasOrderNo(20);
            View.AttachDetailChildrenProperty(typeof(FixtureModel), (c) =>
            {
                var account = c.Parent as FixtureModel;
                account = RF.GetById<FixtureModel>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataViewGroup).HasLabel("电子行业").HasOrderNo(100).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 配置电子行业属性
        /// </summary>
        protected void EISBaseDataView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.AddBehavior("SIE.Web.Fixtures.Models.Scripts.EISBaseDataViewGroupBehavior");
            
            using (View.OrderProperties())
            {
                View.Property(p => p.IsFeeder).Show().DefaultValue(false);
                View.Property(p => p.SlotType).Readonly(p => !p.IsFeeder).Show();
                View.Property(p => p.IsScraper).Show();
                View.Property(p => p.IsSteelNet).Show();

            }
        }


        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.AddBehavior("SIE.Web.Fixtures.Models.Scripts.FixtureModelBehavior");
            View.HasDetailColumnsCount(5);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.Name);
            View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
            });
            View.Property(p => p.ManageMode).Readonly(p=>p.PersistenceStatus== PersistenceStatus.Modified);
            View.Property(p => p.FixedStorage);
            View.Property(p => p.LoadingManage);
            View.Property(p => p.BindProduct);
            View.Property(p => p.BindEquip);
            View.Property(p => p.UnitId).HasLabel("单位");
            View.Property(p => p.IndustryProperties).UseEnumEditor().DefaultValue(10);
            View.Property(p => p.MaxUseNum).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.AllowDecimals = false;
            });
            View.Property(p => p.MaxUseHour).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.MaintainNum).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.AllowDecimals = false;
            });
            View.Property(p => p.WarnNum).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.AllowDecimals = false;
            });
            View.Property(p => p.MaintainHour).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.WarnHour).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.OnlineHour).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.DecimalPrecision = 2;
            }).Readonly(p => p.LoadingManage == YesNo.No);
            View.Property(p => p.MaintainEnforce).Readonly(p => p.LoadingManage == YesNo.No);
            View.ChildrenProperty(p => p.EquipmentList).HasLabel("设备清单").HasOrderNo(10);
            View.ChildrenProperty(p => p.MaintainProjectList).HasLabel("工治具保养项目").HasOrderNo(20);
            View.AttachDetailChildrenProperty(typeof(FixtureModel), (c) =>
            {
                var account = c.Parent as FixtureModel;
                account = RF.GetById<FixtureModel>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataViewGroup).HasLabel("电子行业").HasOrderNo(100).Show(ChildShowInWhere.All).HasOrderNo(30);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.ManageMode).UseEnumEditor().Readonly();
            View.Property(p => p.FixedStorage).Readonly();
            View.Property(p => p.LoadingManage).Readonly();
            View.Property(p => p.BindProduct).Readonly();
            View.Property(p => p.BindEquip);
            View.Property(p => p.UnitId).HasLabel("单位").Readonly();
            View.Property(p => p.IndustryProperties).UseEnumEditor().Readonly();
            View.Property(p => p.MaxUseNum).Readonly();
            View.Property(p => p.MaxUseHour).Readonly();
            View.Property(p => p.MaintainNum).Readonly();
            View.Property(p => p.WarnNum).Readonly();
            View.Property(p => p.MaintainHour).Readonly();
            View.Property(p => p.WarnHour).Readonly();
            View.Property(p => p.OnlineHour).Readonly();
            View.Property(p => p.MaintainEnforce).Readonly();
        }

        /// <summary>
        /// 导入
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).HasLabel("编码*");
            View.Property(p => p.Name).HasLabel("名称*");
            View.PropertyRef(p => p.FixtureType.Code).BeforeImportRequireFunc("工治具类型*").HasLabel("工治具类型*");
            View.Property(p => p.ManageMode).HasLabel("管控方式*");
            View.Property(p => p.FixedStorage).HasLabel("固定储位*");
            View.Property(p => p.LoadingManage).HasLabel("上料管理*");
            View.Property(p => p.BindProduct).HasLabel("绑定产品*");
            View.Property(p => p.BindEquip).HasLabel("绑定设备*");
            View.PropertyRef(p => p.Unit.Name).BeforeImportRequireFunc("单位*").HasLabel("单位*");
            View.Property(p => p.IndustryProperties);
            View.Property(p => p.SlotType);
            View.Property(p => p.MaxUseNum).BeforeImportNonnegativeFunc("最大使用次数");
            View.Property(p => p.MaxUseHour).BeforeImportNonnegativeFunc("最大使用小时数");
            View.Property(p => p.MaintainNum).BeforeImportNonnegativeFunc("保养标准(次数)");
            View.Property(p => p.WarnNum).BeforeImportNonnegativeFunc("预警值(次数)");
            View.Property(p => p.MaintainHour).BeforeImportNonnegativeFunc("保养标准(小时)");
            View.Property(p => p.WarnHour).BeforeImportNonnegativeFunc("预警值(小时)");
            View.Property(p => p.OnlineHour).BeforeImportNonnegativeFunc("上线定期保养标准(小时)");
            View.Property(p => p.MaintainEnforce).HasLabel("保养强制执行*");
            View.Property(p => p.IndustryProperties).HasLabel("行业属性*");
            View.Property(p => p.IsFeeder);
            View.Property(p => p.IsScraper);
            View.Property(p => p.SlotType);
        }
    }
}
