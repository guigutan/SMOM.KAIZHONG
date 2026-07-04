using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Logs
{
    public class UploadTransactionSubmitting : OnSubmitting<UploadTransaction>
    {
        protected override void Invoke(UploadTransaction entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //默认位0(方便后面计算重传次数)
                if (entity.UploadCount == null)
                    entity.UploadCount = 0;
            }
        }
    }
}
