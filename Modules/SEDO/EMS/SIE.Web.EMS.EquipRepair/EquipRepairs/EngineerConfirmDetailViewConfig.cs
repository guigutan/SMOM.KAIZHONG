using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 工程确认视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EngineerConfirmDetailViewConfig : WebViewConfig<EngineerConfirmDetail>
    {
        #region 图片名称 FileName
        /// <summary>
        /// 图片名称
        /// </summary>
        [Label("图片名称")]
        public static readonly Property<string> FileNameProperty = P<EngineerConfirmDetail>.RegisterExtensionReadOnly("FileName", typeof(EngineerConfirmDetailViewConfig),
            GetFileName, EngineerConfirmDetail.EngineerAttachmentProperty);

        /// <summary>
        /// 图片名称
        /// </summary>
        public static string GetFileName(EngineerConfirmDetail me)
        {
            string FileName = System.IO.Path.GetFileName(me.EngineerAttachment);
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
            View.AssignAuthorize(typeof(EngineerConfirmDetail));
            //View.DeclareExtendViewGroup(PhotoViewGroup);
            View.DeclareExtendViewGroup(new string[]
            {
                EquipRepairViewConfig.EngineerConfirmViewGroup,PhotoViewGroup
            });
            if (ViewGroup == EquipRepairViewConfig.EngineerConfirmViewGroup)
                EngineerConfirmView();
            if (ViewGroup == PhotoViewGroup)
                PhotoView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EngineerConfirmDetailBehavior");
            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EngineerFileDownloadCommand");
            View.Property(p => p.ProjectNameView).Readonly();
            View.Property(p => p.EquipRepairScore).Readonly();
            View.Property(EngineerConfirmDetailViewConfig.FileNameProperty).Readonly();
            View.Property(p => p.EngineerAttachment)
                .UseConfigValueEditor(p => 
            {
                p.XType = "uploadfileeditor_engineerConfirm"; 
                p.AllowBlank = true;
                p.TriggerCls = "iconfont icon-ArrowUpBold1";
            })
                .Show().HasLabel("图片路径(上传)");
            View.Property(p => p.Remark).Readonly().ShowInList(width: 150);

            View.AttachDetailChildrenProperty(typeof(EngineerConfirmDetail), (c) =>
            {
                var item = c.Parent as EngineerConfirmDetail;
                item = RF.GetById<EngineerConfirmDetail>(item.Id);
                return item;
            }, PhotoViewGroup).HasLabel("查看");

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 工程确认明细视图
        /// </summary>
        protected void EngineerConfirmView()
        {
            View.AssignAuthorize(typeof(EngineerConfirmDetail));
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).Readonly().Show();
                View.Property(p => p.EquipRepairScore).HasLabel("评分项*").Show();
                //View.Property(EngineerConfirmDetailViewConfig.FileNameProperty).Readonly().Show();工程确认明细视图的扩展只读属性为何出不来？
                //View.Property(p => p.EngineerAttachment).UseConfigValueEditor(p => { p.XType = "uploadfileeditor_engineerConfirm"; p.AllowBlank = true; }).Show();EngineerAttachment无法赋值
                View.Property(p => p.ProjectNameView).UseConfigValueEditor(p => 
                { 
                    p.XType = "uploadfileeditor_engineerConfirm"; 
                    p.AllowBlank = true;
                    p.TriggerCls = "iconfont icon-ArrowUpBold1";
                }).ShowInList(width: 150).HasLabel("图片路径(上传)");
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
                View.Property(p => p.Content).UseCustomEditor(p => p.XType = "engineerPictureEditor").HasLabel("").ShowInDetail();
            }
        }
    }
}
