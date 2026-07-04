using SIE.EMS.Tpms;

namespace SIE.Web.EMS.Tpms
{
    /// <summary>
    /// TMP评分明细视图配置
    /// </summary>
    internal class TpmRecordDetailViewConfig : WebViewConfig<TpmRecordDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.Tpms.Commands.EditRecordDetailCommand", "SIE.Web.EMS.Tpms.Commands.ShowScorePicture");
            using (View.OrderProperties())
            {
                View.UseClientOrder();
                View.Property(p => p.ProjectName).Readonly();
                View.Property(p => p.ProjectType).Readonly();
                //View.Property(p => p.CheckStandard).UseTextEditor(p => p.MaxLength = 1000).Readonly().ShowInList(width: 450);
                //View.Property(p => p.ScoreRate).Readonly();
                View.Property(p => p.DeductScore).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = false;
                    p.AllowBlank = false;
                    p.MinValue = 0;
                    p.MaxValue = 100;
                });
                View.Property(p => p.Remark);
                View.Property(p => p.Photo).Readonly();

                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);//隐藏
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);//隐藏
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);//隐藏
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);//隐藏
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Photo).UseImageComponentEditor().HasLabel("").ShowInDetail().Readonly();
        }
    }
}
