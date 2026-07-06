using SIE.MES.SpcFromMesDetails;
using SIE.MetaModel.View;

namespace SIE.Web.MES.SpcFromMesDetails
{
    /// <summary>
    /// 统计过程控制明细表-视图配置  
    /// </summary>
    public class SpcFromMesDetailViewConfig : WebViewConfig<SpcFromMesDetail>
    {


        /// <summary>
        /// ViewGroup视图---记录测量数据
        /// </summary>
        public const string EditView = nameof(EditView);

        /// <summary>
        /// ViewGroup视图---查看预览
        /// </summary>
        public const string ReadonlyView = nameof(ReadonlyView);

        /// <summary>
        ///
        /// </summary>
        protected override void ConfigView()
        {

            View.AssignAuthorize(typeof(SpcFromMesDetail));
            View.DeclareExtendViewGroup(new string[] { EditView, ReadonlyView });
            switch (ViewGroup)
            {
                case EditView:
                    EditConfigView();
                    break;
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
                default:
                    break;
            }

        }

        private void ConfigReadonlyView()
        {
            View.ClearCommands();

            //根据 GroupCount 动态显示/隐藏列的实现：
            View.AddBehavior("SIE.Web.MES.SpcFromMesDetails.Behaviors.SpcFromMesDetailBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.ObservedValue1).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue2).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue3).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue4).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue5).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue6).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue7).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue8).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue9).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue10).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue11).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue12).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue13).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue14).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue15).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue16).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue17).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue18).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue19).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue20).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue21).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue22).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue23).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue24).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue25).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue26).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue27).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue28).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue29).ShowInList(width: 60).Readonly();
                View.Property(p => p.ObservedValue30).ShowInList(width: 60).Readonly();


                View.Property(p => p.SumX).ShowInList(width: 60).Readonly();
                View.Property(p => p.AvgX).ShowInList(width: 60).Readonly();
                View.Property(p => p.RangeX).ShowInList(width: 60).Readonly();
            }


        }

        private void EditConfigView()
        {

            View.ReplaceCommands(WebCommandNames.Save, typeof(Commands.SpcFromMesDetailSaveCommand).FullName);
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);

            View.AddBehavior("SIE.Web.MES.SpcFromMesDetails.Behaviors.SpcFromMesDetailBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.ObservedValue1).ShowInList(width: 70);
                View.Property(p => p.ObservedValue2).ShowInList(width: 70);
                View.Property(p => p.ObservedValue3).ShowInList(width: 70);
                View.Property(p => p.ObservedValue4).ShowInList(width: 70);
                View.Property(p => p.ObservedValue5).ShowInList(width: 70);
                View.Property(p => p.ObservedValue6).ShowInList(width: 70);
                View.Property(p => p.ObservedValue7).ShowInList(width: 70);
                View.Property(p => p.ObservedValue8).ShowInList(width: 70);
                View.Property(p => p.ObservedValue9).ShowInList(width: 70);
                View.Property(p => p.ObservedValue10).ShowInList(width: 70);
                View.Property(p => p.ObservedValue11).ShowInList(width: 70);
                View.Property(p => p.ObservedValue12).ShowInList(width: 70);
                View.Property(p => p.ObservedValue13).ShowInList(width: 70);
                View.Property(p => p.ObservedValue14).ShowInList(width: 70);
                View.Property(p => p.ObservedValue15).ShowInList(width: 70);
                View.Property(p => p.ObservedValue16).ShowInList(width: 70);
                View.Property(p => p.ObservedValue17).ShowInList(width: 70);
                View.Property(p => p.ObservedValue18).ShowInList(width: 70);
                View.Property(p => p.ObservedValue19).ShowInList(width: 70);
                View.Property(p => p.ObservedValue20).ShowInList(width: 70);
                View.Property(p => p.ObservedValue21).ShowInList(width: 70);
                View.Property(p => p.ObservedValue22).ShowInList(width: 70);
                View.Property(p => p.ObservedValue23).ShowInList(width: 70);
                View.Property(p => p.ObservedValue24).ShowInList(width: 70);
                View.Property(p => p.ObservedValue25).ShowInList(width: 70);
                View.Property(p => p.ObservedValue26).ShowInList(width: 70);
                View.Property(p => p.ObservedValue27).ShowInList(width: 70);
                View.Property(p => p.ObservedValue28).ShowInList(width: 70);
                View.Property(p => p.ObservedValue29).ShowInList(width: 70);
                View.Property(p => p.ObservedValue30).ShowInList(width: 70);


                View.Property(p => p.SumX).ShowInList(width: 70).Readonly();
                View.Property(p => p.AvgX).ShowInList(width: 70).Readonly();
                View.Property(p => p.RangeX).ShowInList(width: 70).Readonly();
            }


        }



        /// <summary>
        /// 视图配置---默认
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);

            View.AddBehavior("SIE.Web.MES.SpcFromMesDetails.Behaviors.SpcFromMesDetailBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.ObservedValue1).ShowInList(width: 100);
                View.Property(p => p.ObservedValue2).ShowInList(width: 100);
                View.Property(p => p.ObservedValue3).ShowInList(width: 100);
                View.Property(p => p.ObservedValue4).ShowInList(width: 100);
                View.Property(p => p.ObservedValue5).ShowInList(width: 100);
                View.Property(p => p.ObservedValue6).ShowInList(width: 100);
                View.Property(p => p.ObservedValue7).ShowInList(width: 100);
                View.Property(p => p.ObservedValue8).ShowInList(width: 100);
                View.Property(p => p.ObservedValue9).ShowInList(width: 100);
                View.Property(p => p.ObservedValue10).ShowInList(width: 100);
                View.Property(p => p.ObservedValue11).ShowInList(width: 100);
                View.Property(p => p.ObservedValue12).ShowInList(width: 100);
                View.Property(p => p.ObservedValue13).ShowInList(width: 100);
                View.Property(p => p.ObservedValue14).ShowInList(width: 100);
                View.Property(p => p.ObservedValue15).ShowInList(width: 100);
                View.Property(p => p.ObservedValue16).ShowInList(width: 100);
                View.Property(p => p.ObservedValue17).ShowInList(width: 100);
                View.Property(p => p.ObservedValue18).ShowInList(width: 100);
                View.Property(p => p.ObservedValue19).ShowInList(width: 100);
                View.Property(p => p.ObservedValue20).ShowInList(width: 100);
                View.Property(p => p.ObservedValue21).ShowInList(width: 100);
                View.Property(p => p.ObservedValue22).ShowInList(width: 100);
                View.Property(p => p.ObservedValue23).ShowInList(width: 100);
                View.Property(p => p.ObservedValue24).ShowInList(width: 100);
                View.Property(p => p.ObservedValue25).ShowInList(width: 100);
                View.Property(p => p.ObservedValue26).ShowInList(width: 100);
                View.Property(p => p.ObservedValue27).ShowInList(width: 100);
                View.Property(p => p.ObservedValue28).ShowInList(width: 100);
                View.Property(p => p.ObservedValue29).ShowInList(width: 100);
                View.Property(p => p.ObservedValue30).ShowInList(width: 100);


                View.Property(p => p.SumX).ShowInList(width: 100).Readonly();
                View.Property(p => p.AvgX).ShowInList(width: 100).Readonly();
                View.Property(p => p.RangeX).ShowInList(width: 100).Readonly();
            }


        }


    }
}
