using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Inventory.Transactions;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 周转规则明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class TurnOverRuleDetailViewConfig : WebViewConfig<TurnOverRuleDetail>
    {
        /// <summary>
        ///  扩展查看视图
        /// </summary>       
        public const string TurnOverRuleDetailGroup = "TurnOverRuleDetailGroup";

        public const string TurnOverRuleReadOnlyGroup = "TurnOverRuleReadOnlyGroup";

        
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(TurnOverRuleDetailGroup, TurnOverRuleReadOnlyGroup);
            switch (ViewGroup)
            {
                case TurnOverRuleDetailGroup:
                    TurnOverRuleDetailEditView();
                    break;
                case TurnOverRuleReadOnlyGroup:
                    TurnOverRuleReadOnlyView();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 编辑视图
        /// </summary>
        public void TurnOverRuleDetailEditView()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInDetail().Readonly();
                View.Property(p => p.OrderType).ShowInDetail().UseEnumEditor("SHIPMENT");
                View.Property(p => p.Transaction).ShowInDetail().UseDataSource((e, c, r) =>
                {
                    var turnOverRuleDetail = e as TurnOverRuleDetail;
                    if (turnOverRuleDetail == null)
                        return new EntityList<Transaction>();
                    if (!turnOverRuleDetail.OrderType.HasValue)
                        return new EntityList<Transaction>();
                    return RT.Service.Resolve<TransactionController>().GetTransactions(c, r, turnOverRuleDetail.OrderType.Value);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);
                View.Property(p => p.Customer).ShowInDetail().UseDataSource((e, c, r) =>
                {
                    var turnOverRuleDetail = e as TurnOverRuleDetail;
                    if (turnOverRuleDetail == null)
                        return new EntityList<Customer>();
                    return RT.Service.Resolve<CustomerController>().GetCustomer(CustomerType.CUSTOMER, r, c);
                }).UsePagingLookUpEditor().Readonly(p => p.DepartmentId != null)
                .UseListSetting(e => { e.HelpInfo = "显示类型为客户的客户列表,存在部门不可编辑"; });
                View.Property(p => p.DepartmentId).UseDataSource((e, c, r) =>
                {
                    // return RT.Service.Resolve<EnterpriseController>().GetEnterprises(null, c, r);
                    var source = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Department, c, r);
                    var result = new EntityList<Enterprise>();
                    foreach (var item in source)
                    {
                        if (item.Level.Type == EnterpriseType.Department)
                        {
                            item.TreePId = null;
                            result.Add(item);
                        }
                    }
                    return result;
                }).UsePagingLookUpEditor().Readonly(p => p.CustomerId != null)
                .UseListSetting(e => { e.HelpInfo = "存在客户不可编辑"; }).ShowInDetail();
                View.AttachChildrenProperty(typeof(TurnOverRuleDetailSortRuleViewModel), (w =>
                {
                    var Parent = w.Parent as TurnOverRuleDetail;
                    return RT.Service.Resolve<TurnOverRuleDetailSortRuleViewModelController>().GetList(Parent.Id);
                })).Show(ChildShowInWhere.Detail);
            }
        }
        /// <summary>
        /// 只读视图
        /// </summary>
        public void TurnOverRuleReadOnlyView()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInDetail().Readonly();
                View.Property(p => p.OrderType).ShowInDetail().UseEnumEditor("SHIPMENT").Readonly();
                View.Property(p => p.Transaction).ShowInDetail().UseDataSource((e, c, r) =>
                {
                    var turnOverRuleDetail = e as TurnOverRuleDetail;
                    if (turnOverRuleDetail == null)
                        return new EntityList<Transaction>();
                    if (!turnOverRuleDetail.OrderType.HasValue)
                        return new EntityList<Transaction>();
                    return RT.Service.Resolve<TransactionController>().GetTransactions(c, r, turnOverRuleDetail.OrderType.Value);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).Readonly();
                View.Property(p => p.Customer).ShowInDetail().UseDataSource((e, c, r) =>
                {
                    var turnOverRuleDetail = e as TurnOverRuleDetail;
                    if (turnOverRuleDetail == null)
                        return new EntityList<Customer>();
                    return RT.Service.Resolve<CustomerController>().GetCustomer(CustomerType.CUSTOMER, r, c);
                }).UsePagingLookUpEditor().Readonly(p => p.DepartmentId != null)
                .UseListSetting(e => { e.HelpInfo = "显示类型为客户的客户列表,存在部门不可编辑"; }).Readonly();
                View.Property(p => p.Department).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<EnterpriseController>().GetEnterprises(null, c, r);
                }).UsePagingLookUpEditor().Readonly(p => p.CustomerId != null)
                .UseListSetting(e => { e.HelpInfo = "存在客户不可编辑"; }).ShowInDetail().Readonly();
                View.AttachChildrenProperty(typeof(TurnOverRuleDetailSortRuleViewModel), (w =>
                {
                    var Parent = w.Parent as TurnOverRuleDetail;
                    return RT.Service.Resolve<TurnOverRuleDetailSortRuleViewModelController>().GetList(Parent.Id);
                })).Show(ChildShowInWhere.Detail).Readonly();
            }
        }
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(TurnOverRuleDetailAddCommand).FullName, typeof(TurnOverRuleDetailEditCommand).FullName);
            View.UseCommands(typeof(DeleteTurnOverRuleDetailCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                using (View.DeclareBand("基本信息"))
                {
                    View.Property(p => p.LineNo);
                    View.Property(p => p.OrderType);
                    View.Property(p => p.Transaction);
                    View.Property(p => p.CustomerName);
                    View.Property(p => p.Department);
                }
                using (View.DeclareBand("排序1"))
                {
                    View.Property(p => p.SortField1);
                    View.Property(p => p.FieldType1);
                    View.Property(p => p.SortType1);
                }
                using (View.DeclareBand("排序1的文本型字段"))
                {
                    View.Property(p => p.EqualValue1);
                }
                using (View.DeclareBand("排序1的数值型字段"))
                {
                    View.Property(p => p.LowerLimit1);
                    View.Property(p => p.UpperLimit1);
                }
                using (View.DeclareBand("排序1的日期型字段和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay1);
                    View.Property(p => p.UpperLimitDay1);
                }
               
                using (View.DeclareBand("排序2"))
                {
                    View.Property(p => p.SortField2);
                    View.Property(p => p.FieldType2);
                    View.Property(p => p.SortType2);
                }
                using (View.DeclareBand("排序2的文本型字段"))
                {
                    View.Property(p => p.EqualValue2);
                }
                using (View.DeclareBand("排序2的数值型字段"))
                {
                    View.Property(p => p.LowerLimit2);
                    View.Property(p => p.UpperLimit2);
                }
                using (View.DeclareBand("排序2的日期型字段和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay2);
                    View.Property(p => p.UpperLimitDay2);
                }
             
                using (View.DeclareBand("排序3"))
                {
                    View.Property(p => p.SortField3);
                    View.Property(p => p.FieldType3);
                    View.Property(p => p.SortType3);
                }
                using (View.DeclareBand("排序3的文本型字段"))
                {
                    View.Property(p => p.EqualValue3);
                }
                using (View.DeclareBand("排序3的数值型字段"))
                {
                    View.Property(p => p.LowerLimit3);
                    View.Property(p => p.UpperLimit3);
                }
                using (View.DeclareBand("排序3的日期型字段和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay3);
                    View.Property(p => p.UpperLimitDay3);
                }
                
                using (View.DeclareBand("排序4"))
                {
                    View.Property(p => p.SortField4);
                    View.Property(p => p.FieldType4);
                    View.Property(p => p.SortType4);
                }
                using (View.DeclareBand("排序4的文本型字段"))
                {
                    View.Property(p => p.EqualValue4);
                }
                using (View.DeclareBand("排序4的数值型字段"))
                {
                    View.Property(p => p.LowerLimit4);
                    View.Property(p => p.UpperLimit4);
                }
                using (View.DeclareBand("排序4的日期型字段和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay4);
                    View.Property(p => p.UpperLimitDay4);
                }
              
                using (View.DeclareBand("排序5"))
                {
                    View.Property(p => p.SortField5);
                    View.Property(p => p.FieldType5);
                    View.Property(p => p.SortType5);
                }
                using (View.DeclareBand("排序5的文本型字段"))
                {
                    View.Property(p => p.EqualValue5);
                }
                using (View.DeclareBand("排序5的数值型字段"))
                {
                    View.Property(p => p.LowerLimit5);
                    View.Property(p => p.UpperLimit5);
                }
                using (View.DeclareBand("排序5的日期型字段和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay5);
                    View.Property(p => p.UpperLimitDay5);
                }
               
                using (View.DeclareBand(" "))
                {
                    View.Property(p => p.CreateByName);
                    View.Property(p => p.CreateDate);
                    View.Property(p => p.UpdateByName);
                    View.Property(p => p.UpdateDate);
                }
            }
            View.ChildrenProperty(p => p.DefinitionList).IsVisible(false);
        }
    }
}
