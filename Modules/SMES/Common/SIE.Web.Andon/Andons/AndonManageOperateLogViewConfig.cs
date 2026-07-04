using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯管理操作记录视图配置
    /// </summary>
    public class AndonManageOperateLogViewConfig : WebViewConfig<AndonManageOperateLog>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";
        
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(LookUpViewGroup);
            if (ViewGroup == LookUpViewGroup)
            {
                LookUpView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OperateTime).ShowInList(width:150).Readonly();
                View.Property(p => p.OperateType).Readonly();
                View.Property(p => p.Operater).Readonly();
                View.Property(p => p.Remark).ShowInList(width: 150).Readonly();
                //View.Property(p => p.LastOperateMinute).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OperateTime).Readonly();
                View.Property(p => p.OperateType).Readonly();
                View.Property(p => p.Operater).Readonly();
                View.Property(p => p.Remark).Readonly();
                //View.Property(p => p.LastOperate).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
        
        /// <summary>
        /// 查看视图
        /// </summary>
        protected void LookUpView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OperateTime).ShowInList(width:150).Readonly();
                View.Property(p => p.OperateType).Show().Readonly();
                View.Property(p => p.Operater).Show().Readonly();
                View.Property(p => p.Remark).ShowInList(width: 200).Readonly();
                //View.Property(p => p.LastOperate).Show().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
