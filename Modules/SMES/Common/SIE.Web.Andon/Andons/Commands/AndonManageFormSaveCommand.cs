using AngleSharp.Dom;
using SIE.Andon.Andons;
using SIE.Andon.MessageSendJob;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理保存命令
    /// </summary>
    public class AndonManageFormSaveCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<AndonManageSaveViewArgs>();
            var andonManage = data.AndonManage;
            var andonManageCallMaterials = data.AndonManageCallMaterials;
            RT.Service.Resolve<AndonManageController>().SaveAndonAndItemDetailAsync(andonManage, andonManageCallMaterials);
            andonManage.MarkSaved();
            andonManageCallMaterials.MarkSaved();
            return true;
        }

        /// <summary>
        /// 安灯管理主表实体和物料子表
        /// </summary>
        public class AndonManageSaveViewArgs
        {
            /// <summary>
            /// 安灯管理
            /// </summary>
            public AndonManage AndonManage { get; set; }

            /// <summary>
            /// 物料子表
            /// </summary>
            public EntityList<AndonManageCallMaterial> AndonManageCallMaterials { get; set; }
        }
    }
}
