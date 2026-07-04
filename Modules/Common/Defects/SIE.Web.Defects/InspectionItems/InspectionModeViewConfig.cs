using SIE.Defects.InspectionItems;
using SIE.Domain;

namespace SIE.Web.Defects.InspectionItems
{
    /// <summary>
    /// 检验方式视图配置
    /// </summary>
    public class InspectionModeViewConfig : WebViewConfig<InspectionMode>
    {
        #region 编码只读 CodeReadOnly
        /// <summary>
        /// 编码只读
        /// </summary>
        public static readonly Property<bool> CodeReadOnlyProperty = P<InspectionMode>.RegisterExtensionReadOnly("CodeReadOnly", typeof(InspectionModeViewConfig),
            GetCodeReadOnly, InspectionMode.IdProperty);

        /// <summary>
        /// 编码只读
        /// </summary>
        /// <param name="me">当前实体</param>
        /// <returns>更改状态</returns>
        public static bool GetCodeReadOnly(InspectionMode me)
        {
            return me.PersistenceStatus != PersistenceStatus.New;
        }
        #endregion

        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
        }
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
