using DocumentFormat.OpenXml.Office2010.CustomUI;
using SIE.MES.SpcFromMesDetails;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);

            using (View.OrderProperties())
            {
                //View.AddBehavior(typeof(SpcFromMesDetailBehavior).FullName);
               
                View.Property(p => p.ObservedValue1).ShowInList(width: 150);
                View.Property(p => p.ObservedValue2).ShowInList(width: 150);
                View.Property(p => p.ObservedValue3).ShowInList(width: 150);
                View.Property(p => p.ObservedValue4).ShowInList(width: 150);


                // 这里不配置 ObservedValue 列，ObservedValue1、ObservedValue2...由行为动态添加
                //View.AddBehavior(typeof(SpcFromMesDetailBehavior).FullName);
            }


        }

        private void EditConfigView()
        {

            View.ReplaceCommands(WebCommandNames.Save, typeof(Commands.SpcFromMesDetailSaveCommand).FullName);
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);

            //View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.Delete);


            using (View.OrderProperties())
            {
                //View.Property(p => p.RowNumber).Show(ShowInWhere.All).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);

                View.Property(p => p.ObservedValue1).ShowInList(width: 150);
                View.Property(p => p.ObservedValue2).ShowInList(width: 150);
                View.Property(p => p.ObservedValue3).ShowInList(width: 150);
                View.Property(p => p.ObservedValue4).ShowInList(width: 150);


                // 这里不配置 ObservedValue 列，ObservedValue1、ObservedValue2...由行为动态添加
                //View.AddBehavior(typeof(SpcFromMesDetailBehavior).FullName);
            }


        }



        /// <summary>
        /// 视图配置---默认
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);

            using (View.OrderProperties())
            {
                //View.AddBehavior(typeof(SpcFromMesDetailBehavior).FullName);

                View.Property(p => p.ObservedValue1).ShowInList(width: 150);
                View.Property(p => p.ObservedValue2).ShowInList(width: 150);
                View.Property(p => p.ObservedValue3).ShowInList(width: 150);
                View.Property(p => p.ObservedValue4).ShowInList(width: 150);


                // 这里不配置 ObservedValue 列，ObservedValue1、ObservedValue2...由行为动态添加
                //View.AddBehavior(typeof(SpcFromMesDetailBehavior).FullName);
            }




        }


    }
}
