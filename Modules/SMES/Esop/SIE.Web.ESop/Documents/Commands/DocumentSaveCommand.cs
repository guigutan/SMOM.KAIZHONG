using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Documents;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ESop.Documents.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.ESop.Documents.Commands.DocumentSaveCommand")]
    public class DocumentSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="entity">发放单实体</param>
        protected override void DoSave(Entity entity)
        {
            var documentCollection = entity as DocumentCollection;

            if (documentCollection == null)
            {
                throw new ValidationException("保存数据异常，请重新尝试！".L10N());
            }
            RF.Save(documentCollection);
        }
    }
}
