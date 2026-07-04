using SIE.Fixtures;
using SIE.Fixtures.Enums;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using SIE.Web.Fixtures.Models.Commands;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具编码视图配置
    /// </summary>
    internal class FixtureEncodeViewConfig : WebViewConfig<FixtureEncode>
    {
        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int displayCoulmnWidth = 20;
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Fixtures.Models.Commands.AddFixtureEncodeCommand");
            View.ReplaceCommands(WebCommandNames.Save, typeof(FixtureEncodeSaveCommand).FullName);
            View.UseCommands(typeof(SyncFixtureEncodeCommand).FullName, "SIE.Web.Fixtures.Models.Commands.FixtureEncodeImportCommand", 
                "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.Code).ShowInList(displayCoulmnWidth*8);
           
            View.Property(p => p.FixtureModelId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ModelName), nameof(e.FixtureModel.Name));
                keyValues.Add(nameof(e.FixtureType), nameof(e.FixtureModel.FixtureTypeName));
                keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureModel.ManageMode));
                keyValues.Add(nameof(e.SlotType), nameof(e.FixtureModel.SlotType));
                keyValues.Add(nameof(e.FixedStorage), nameof(e.FixtureModel.FixedStorage));
                keyValues.Add(nameof(e.LoadingManage), nameof(e.FixtureModel.LoadingManage));
                keyValues.Add(nameof(e.BindProduct), nameof(e.FixtureModel.BindProduct));
                keyValues.Add(nameof(e.BindEquip), nameof(e.FixtureModel.BindEquip));
                keyValues.Add(nameof(e.UnitName), nameof(e.FixtureModel.UnitName));
                keyValues.Add(nameof(e.MaxUseNum), nameof(e.FixtureModel.MaxUseNum));
                keyValues.Add(nameof(e.MaxUseHour), nameof(e.FixtureModel.MaxUseHour));
                keyValues.Add(nameof(e.MaintainNum), nameof(e.FixtureModel.MaintainNum));
                keyValues.Add(nameof(e.MaintainHour), nameof(e.FixtureModel.MaintainHour));
                keyValues.Add(nameof(e.WarnNum), nameof(e.FixtureModel.WarnNum));
                keyValues.Add(nameof(e.WarnHour), nameof(e.FixtureModel.WarnHour));
                keyValues.Add(nameof(e.OnlineHour), nameof(e.FixtureModel.OnlineHour));
                keyValues.Add(nameof(e.MaintainEnforce), nameof(e.FixtureModel.MaintainEnforce));
                m.DicLinkField = keyValues;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {   
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureModels(null,pagingInfo, keyword);//null是工治具类型，此处设置为空
            }).HasLabel("型号编码").ShowInList(displayCoulmnWidth * 8);
            View.Property(p => p.ModelName).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.FixtureType).Readonly().ShowInList(displayCoulmnWidth * 5);
            View.Property(p => p.IndustryProperties).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.ManageMode).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.Exemption).ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.FixedStorage).Readonly().ShowInList(displayCoulmnWidth * 4);
           
            View.Property(p => p.LoadingManage).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.BindProduct).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.BindEquip).Readonly().ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.UnitName).Readonly().ShowInList(displayCoulmnWidth * 4);

            View.Property(p => p.TotalNum).Readonly().ShowInList(displayCoulmnWidth *3);
            View.Property(p => p.CanUseNum).Readonly().ShowInList(displayCoulmnWidth * 3);
            View.Property(p => p.InWarehouseNum).Readonly().ShowInList(displayCoulmnWidth * 3);
            View.Property(p => p.AcceptedInWHNum).Readonly().ShowInList(displayCoulmnWidth * 5);
            View.Property(p => p.ScrapNum).Readonly().ShowInList(displayCoulmnWidth * 3);

            View.Property(p => p.MaxUseNum).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.MaxUseHour).Readonly().ShowInList(displayCoulmnWidth * 7);
            View.Property(p => p.MaintainNum).Readonly().ShowInList(displayCoulmnWidth * 7);
            View.Property(p => p.MaintainHour).Readonly().ShowInList(displayCoulmnWidth * 7);
            View.Property(p => p.WarnNum).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.WarnHour).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.OnlineHour).Readonly().ShowInList(displayCoulmnWidth * 9);
            View.Property(p => p.MaintainEnforce).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.ChildrenProperty(p => p.FixtureEncodeProductDetailList).HasLabel("产品清单");
            View.ChildrenProperty(p => p.FixtureEncodeMaintainProjectList).HasLabel("保养项目");
            View.ChildrenProperty(p => p.FixtureEncodeStorageLocationList).HasLabel("存储位置");
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.ManageMode).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.SlotType).Readonly();
            View.Property(p => p.FixedStorage).Readonly();
            View.Property(p => p.LoadingManage).Readonly();
            View.Property(p=>p.Exemption).Readonly();
            View.Property(p => p.BindProduct).Readonly();
            View.Property(p => p.BindEquip).Readonly();
            View.Property(p => p.UnitName).Readonly();
            View.Property(p => p.MaxUseNum).Readonly();
            View.Property(p => p.MaxUseHour).Readonly();
            View.Property(p => p.MaintainNum).Readonly();
            View.Property(p => p.MaintainHour).Readonly();
            View.Property(p => p.WarnNum).Readonly();
            View.Property(p => p.WarnHour).Readonly();
            View.Property(p => p.OnlineHour).Readonly();
            View.Property(p => p.MaintainEnforce).Readonly();
        }


       /// <summary>
       /// 配置导入
       /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).ImportIndexer();
            View.PropertyRef(p => p.FixtureModel.Code).HasLabel("型号编码");
        }

    }
}