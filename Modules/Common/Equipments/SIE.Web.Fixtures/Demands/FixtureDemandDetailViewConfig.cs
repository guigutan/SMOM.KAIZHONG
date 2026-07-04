using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using SIE.Resources.ProcessSegments;
using System;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands
{
    /// <summary>
	/// 需求明细视图配置
	/// </summary>
    public class FixtureDemandDetailViewConfig : WebViewConfig<FixtureDemandDetail>
    {
        /// <summary>
        /// 自定义工治具治具需求明细编辑视图
        /// </summary>
        public const string EditDemandView = "EditDemandView";

        /// <summary>
        /// 自定义工治具治具需求明细只读视图
        /// </summary>
        public const string ReadonlyDemandView = "ReadonlyDemandView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditDemandView, ReadonlyDemandView);
            if (ViewGroup == EditDemandView)
                ConfigEditDemandView();
            else if (ViewGroup == ReadonlyDemandView)
                ConfigReadonlyDemandView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.ProcessSurface).Readonly();
            View.Property(p => p.ProcessSegmentId).HasLabel("工段");
            View.Property(p => p.DemandQty).Readonly();
            View.Property(p => p.UnloadQty).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置编辑视图
        /// </summary>
        void ConfigEditDemandView()
        {
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseCommands("SIE.Web.Fixtures.Demands.Commands.AddDetailCommand", "SIE.Web.Fixtures.Demands.Commands.EditDetailCommand", WebCommandNames.Delete);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.All).Cascade(p => p.FixtureModelId, null)
                    .Cascade(p => p.ModelName,null).Show(ShowInWhere.All);
                
                
                View.Property(p => p.FixtureModelId).UseDataSource((e, c, r) =>
                {
                    var detail = e as FixtureDemandDetail;
                    if (detail == null)
                        return new EntityList<FixtureModel>();
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureModels(detail.FixtureType, c, r);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureModel.Code));
                    keyValues.Add(nameof(e.ModelName), nameof(e.FixtureModel.Name));
                    m.DicLinkField = keyValues;
                }).Cascade(p => p.FixtureEncodeId, null).HasLabel("型号编码").Show(ShowInWhere.All);
                
                View.Property(p => p.ModelName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureEncode).UsePagingLookUpEditor((m, e) =>
                  {
                      m.MethodClassName = "SIE.Web.Fixtures.Demands.Editors.EncodeEditor";
                      m.QueryMode = "remote";
                      m.DataSourceProperty = "EncodeEditor";
                      m.ReloadDataOnPopping = true;
                  }).Cascade(p => p.ProcessSegmentId, null).HasLabel("工治具编码").Show(ShowInWhere.All);
                View.Property(p => p.ProcessSurface).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProcessSegmentId).UseDataSource((e, c, r) =>
                {
                    var detail = e as FixtureDemandDetail;
                    if (detail == null||detail.FixtureEncode==null)
                        return new EntityList<ProcessSegment>();
                    return RT.Service.Resolve<CoreFixtureController>().GetProcessSegment(detail.FixtureEncodeId, detail.ParentProcessSegmentId,detail.ProcessSurface, c, r);
                }).HasLabel("工段").Show(ShowInWhere.All);
                View.Property(p => p.DemandQty).HasLabel("需求数量".L10N()+"*").Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置只读视图
        /// </summary>
        void ConfigReadonlyDemandView()
        {
            using (View.OrderProperties())
            {
                View.UseChildrenAsHorizontal();
                View.ClearCommands();
                View.Property(p => p.EncodeCode).Readonly().ShowInList();
                View.Property(p => p.ModelCode).Readonly().ShowInList();
                View.Property(p => p.ModelName).Readonly().ShowInList();
                View.Property(p => p.FixtureType).Readonly().ShowInList();
                View.Property(p => p.ProcessSurface).Readonly().ShowInList();
                View.Property(p => p.ProcessSegment).Readonly().ShowInList();
                View.Property(p => p.DemandQty).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
