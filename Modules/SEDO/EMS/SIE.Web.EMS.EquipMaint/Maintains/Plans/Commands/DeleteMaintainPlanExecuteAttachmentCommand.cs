using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Records;
using SIE.MetaModel;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Core.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 删除图片
    /// </summary>
    public class DeleteMaintainPlanExecuteAttachmentCommand : ImmediateDeleteCommand
    {

    }
}
