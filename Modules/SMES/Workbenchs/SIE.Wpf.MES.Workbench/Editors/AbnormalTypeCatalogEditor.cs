using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.MES.Workbench.Editors
{
    /// <summary>
    /// 异常停线快码编辑器
    /// </summary>
    public class AbnormalTypeCatalogEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public static string EditorName { get; } = "AbnormalTypeCatalogEditor";

        /// <summary>
        /// 异常停线快码数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="titleProperty">IManagedProperty</param>
        /// <returns>异常停线快码集合</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var catalogs = RT.Service.Resolve<CatalogController>().GetCatalogList(AbnormalCause.AbnormalTypeCatalog);
            return catalogs;
        }
    }
}
