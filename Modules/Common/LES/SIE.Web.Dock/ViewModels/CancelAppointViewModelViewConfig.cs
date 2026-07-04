using SIE.Dock.ViewModels;
using SIE.Domain;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.Dock.ViewModels
{
    /// <summary>
    /// 取消预约ViewModel视图配置
    /// </summary>
    public class CancelAppointViewModelViewConfig : WebViewConfig<CancelAppointViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.CancelReasonId).UseDataSource((o, c, r) =>
                {
                    var viewModel = o as CancelAppointViewModel;
                    if (viewModel == null)
                    {
                        return new EntityList<Reason>();
                    }

                    var reasonTypeValueList = new List<int>();
                    if (viewModel.Type == 0)
                    {
                        reasonTypeValueList.Add((int)ReasonType.CANCEL_APPOINTMENT);
                    }
                    if (viewModel.Type == 1)
                    {
                        reasonTypeValueList.Add((int)ReasonType.CANCELQUEUE_REASON);
                    }

                    return RT.Service.Resolve<ReasonController>().GetReasonTypeData(reasonTypeValueList, State.Enable, r, c);
                }).UsePagingLookUpEditor((p, m) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(m.ReasonName), nameof(m.CancelReason.Name));
                    p.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.ReasonDesc).UseMemoEditor().Show();
            }
        }
    }
}