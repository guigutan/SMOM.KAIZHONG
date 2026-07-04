using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Maintains.Controller;
using SIE.MetaModel;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.AssetRequisitions.Commands
{
    public class AssetRequisitionAttachmentDeleteCommand : DeleteAttachmentCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var parentId = (double)JsonConvert.DeserializeObject<JObject>(args.Data)["ParentEntity"]["Id"];
            var canExecute = RT.Service.Resolve<AssetRequisitionController>().CheckCanExecute(parentId);
            if (!canExecute)
            {
                throw new ValidationException("领用单为已审批状态时不能删除附件图片！".L10N());
            }
            EntityMeta entityMeta = ClientEntities.Find(args.Type);
            if (scope != entityMeta.EntityType.GetQualifiedName())
            {
                throw new SecurityException("参数type[{0}]与令牌不一致".L10nFormat(args.Type));
            }

            DeleteAttachmentViewArgs deleteAttachmentViewArgs = args.Data.ToJsonObject<DeleteAttachmentViewArgs>();
            string attachmentId = deleteAttachmentViewArgs.AttachmentId;
            if (!string.IsNullOrEmpty(attachmentId))
            {
                double num = Convert.ToDouble(attachmentId);
                EntityRepository entityRepository = RepositoryFactory.Find(entityMeta.EntityType);
                Entity byId = entityRepository.GetById(num);
                if (byId != null)
                {
                    RT.Service.Resolve<AttachmentController>().DeleteFile(((SIE.Common.Attachments.Attachment)byId).FileName, ((SIE.Common.Attachments.Attachment)byId).FilePath);
                    byId.PersistenceStatus = PersistenceStatus.Deleted;
                    entityRepository.Save(byId);
                }
            }
            return true;
        }
    }
}
