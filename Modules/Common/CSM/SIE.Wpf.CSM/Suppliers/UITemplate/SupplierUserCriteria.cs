using SIE.Common.Security;
using SIE.Domain;

namespace SIE.WPF.CSM.Suppliers.UITemplate
{
    /// <summary>
    /// 供应商的账户查询实体
    /// </summary>
    [QueryEntity]
    public class SupplierUserCriteria : Criteria
    {
        #region 构造函数
        public SupplierUserCriteria() { }
        #endregion

        #region 代码 Code
        /// <summary>
        /// 代码
        /// </summary>
        public static readonly Property<string> CodeProperty = P<SupplierUserCriteria>.Register(e => e.Code);
        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly Property<string> NameProperty = P<SupplierUserCriteria>.Register(e => e.Name);
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }

        #endregion

        #region 查询
        protected override EntityList Fetch()
        {
            return DCF.Create<UserController>().FetchBy(Code, Name);
        }
        #endregion

    }

    #region 查询视图
    internal class SupplierUserCriteriaViewConfig : WPFViewConfig<SupplierUserCriteria>
    {
        protected override void ConfigView()
        {
            View.DomainName("选择账号");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("账号").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("姓名").Show(ShowInWhere.All);
            }
        }
    }
    #endregion
}
