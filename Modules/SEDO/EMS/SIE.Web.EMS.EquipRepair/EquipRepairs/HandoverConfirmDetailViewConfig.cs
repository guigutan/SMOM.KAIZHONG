using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 交机确认视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class HandoverConfirmDetailViewConfig : WebViewConfig<HandoverConfirmDetail>
    {
        #region 图片名称 FileName
        /// <summary>
        /// 图片名称
        /// </summary>
        [Label("图片名称")]
        public static readonly Property<string> FileNameProperty = P<HandoverConfirmDetail>.RegisterExtensionReadOnly("FileName", typeof(HandoverConfirmDetailViewConfig),
            GetFileName, HandoverConfirmDetail.HandoverAttachmentProperty);

        /// <summary>
        /// 图片名称
        /// </summary>
        public static string GetFileName(HandoverConfirmDetail me)
        {
            string FileName = System.IO.Path.GetFileName(me.HandoverAttachment);
            return FileName;
        }
        #endregion

        /// <summary>
        /// 图片
        /// </summary>
        public const string PhotoViewGroup = "PhotoView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //View.DeclareExtendViewGroup(PhotoViewGroup);
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
            View.DeclareExtendViewGroup(new string[]
            {
                EquipRepairViewConfig.HandoverConfirmViewGroup,PhotoViewGroup
            });
            if (ViewGroup == EquipRepairViewConfig.HandoverConfirmViewGroup)
                HandoverConfirmView();
            if (ViewGroup == PhotoViewGroup)
                PhotoView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.HandoverConfirmDetailBehavior");
           View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.HandoverFileDownloadCommand");
            View.Property(p => p.ProjectNameView).Readonly();
            View.Property(p => p.EquipRepairScore).Readonly();
            View.Property(HandoverConfirmDetailViewConfig.FileNameProperty).Readonly();
            View.Property(p => p.HandoverAttachment)
                .UseConfigValueEditor(p => 
            { 
                p.XType = "uploadfileeditor_handoverConfirm"; 
                p.AllowBlank = true;
                p.TriggerCls = "iconfont icon-ArrowUpBold1";
            }).ShowInList(width: 150).HasLabel("图片路径(上传)");
            View.Property(p => p.Remark).Readonly().ShowInList(width:150);
            View.AttachDetailChildrenProperty(typeof(HandoverConfirmDetail), (c) =>
            {
                var item = c.Parent as HandoverConfirmDetail;
                item = RF.GetById<HandoverConfirmDetail>(item.Id);
                return item;
            }, PhotoViewGroup).HasLabel("查看");

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 交机确认明细视图
        /// </summary>
        protected void HandoverConfirmView()
        {
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).Readonly().Show();
                View.Property(p => p.EquipRepairScore).HasLabel("评分项*").Show();
                //View.Property(HandoverConfirmDetailViewConfig.FileNameProperty).Readonly().Show();交机确认明细视图的扩展只读属性为何出不来？
                View.Property(p => p.HandoverAttachment).UseConfigValueEditor(p => 
                { 
                    p.XType = "uploadfileeditor_handoverConfirm"; 
                    p.AllowBlank = true; 
                    p.TriggerCls = "iconfont icon-ArrowUpBold1"; 
                }).ShowInList(width:150).HasLabel("图片路径(上传)");
                View.Property(p => p.Remark).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        protected void PhotoView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Content).UseCustomEditor(p => p.XType = "handoverPictureEditor").HasLabel("").ShowInDetail();
            }
        }
    }
}
