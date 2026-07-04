using SIE.Domain;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.MES.PrepareProducts.Commands;
using System.Collections.Generic;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产品产前准备子表视图
    /// </summary>
    public class PrepareProductDetailViewConfig : WebViewConfig<PrepareProductDetail>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        public const string AttachChildViewGroup = "AttachViewGroup"; 

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AttachChildViewGroup);
            View.AssignAuthorize(typeof(PrepareProduct));
            if (ViewGroup == AttachChildViewGroup)
            {
                AttachView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(PrepareProductDetailAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    var detail = e as PrepareProductDetail;
                    if (detail == null)
                    {
                        return new EntityList<Process>();
                    }
                    return RT.Service.Resolve<PrepareProductService>().GetProcessListByFamilyId(detail, p, k);
                }).ShowInList(width: 150);
                View.Property(p => p.PrepareProject).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.PrepareProjectName), nameof(e.PrepareProject.ProName));
                    keyValues.Add(nameof(e.PrepareProjectType), nameof(e.PrepareProject.ProType));
                    keyValues.Add(nameof(e.PrepareProjectDesc), nameof(e.PrepareProject.ProDesc));
                    m.DicLinkField = keyValues;
                }).HasLabel("项目编码").ShowInList(width: 150);
                View.Property(p => p.PrepareProjectName).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareProjectType).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareProjectDesc).Readonly().ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected void AttachView()
        {
            View.UseCommands(typeof(PrepareProductDetailAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    var detail = e as PrepareProductDetail;
                    if (detail == null)
                    {
                        return new EntityList<Process>();
                    }
                    return RT.Service.Resolve<PrepareProductService>().GetProcessListByFamilyId(detail, p, k);
                }).ShowInList(width: 150);
                View.Property(p => p.PrepareProject).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.PrepareProjectName), nameof(e.PrepareProject.ProName));
                    keyValues.Add(nameof(e.PrepareProjectType), nameof(e.PrepareProject.ProType));
                    keyValues.Add(nameof(e.PrepareProjectDesc), nameof(e.PrepareProject.ProDesc));
                    m.DicLinkField = keyValues;
                }).HasLabel("项目编码").ShowInList(width: 150);
                View.Property(p => p.PrepareProjectName).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareProjectType).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareProjectDesc).Readonly().ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
            View.PropertyRef(p => p.PrepareProject.ProCode).HasLabel("项目编码");
        }
    }
}
