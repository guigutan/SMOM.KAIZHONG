using SIE.Domain;
using SIE.EMS.Maintains.Records;
using SIE.Resources.Enterprises;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Records
{
    /// <summary>
    /// 保养记录查询实体视图配置
    /// </summary>
    internal class MaintainRecordCriteriaViewConfig : WebViewConfig<MaintainRecordCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.MaintainNo);
            View.Property(p => p.EquipAccount);
            View.Property(p => p.MachineName);
            View.Property(p => p.Workshop).UseDataSource((source, pagingInfo, keyword) =>
            {
                var criteria = source as MaintainRecordCriteria;

                if (criteria == null)
                {
                    return new EntityList<Enterprise>();
                }

                var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);

                workShopList.ForEach(enterprise =>
                {
                    enterprise.TreePId = null;
                });

                return workShopList;
            });

            View.Property(p => p.ResourceName);
            View.Property(p => p.Process).Show(ShowInWhere.Hide);
            View.Property(p => p.ExeState);
            View.Property(p => p.ExeResult);
            View.Property(p => p.ConfirmResult);
            View.Property(p => p.ExeState);
            View.Property(p => p.PlanMaintainDate).UseDateRangeEditor(p =>
            {
                p.DateFormat = "Y/m/d";
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            });
        }
    }
}
