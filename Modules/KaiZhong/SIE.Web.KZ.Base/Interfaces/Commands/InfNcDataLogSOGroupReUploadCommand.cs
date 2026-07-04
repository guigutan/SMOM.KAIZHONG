/*using SIE.Domain.Validation;
using SIE.EventMessages.WebApis;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.KZ.Group.SmomControl.Jobs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    public class InfNcDataLogSOGroupReUploadCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            RT.Service.Resolve<LogGroupController>().InfNcDataLogGroupReUpload(args);
            String BatchNo = "";
            var list = RT.Service.Resolve<LogGroupController>().GetInfNcDataLogGroupByIds(args);
            var groupGridValues = new List<string>();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.GroupGuid))
                {
                    groupGridValues.Add(item.GroupGuid.Trim());
                }
            }
            BatchNo = string.Join(",", groupGridValues.Distinct());

            try
            {
                //AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactorySOJobExecute(InfType.WorkOrder, BatchNo);
                //AddLog(string.Join(",", curMsg));

            }
            catch (Exception exMsg)
            {
                //AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                SendGroupWorkOrderDataToFactoryJob.IsRun = false;
            }
            return "重新完成!".L10N();
        }
    }
}
*//*

using SIE.Domain.Validation;
using SIE.EventMessages.WebApis;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.KZ.Group.SmomControl.Jobs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    public class InfNcDataLogSOGroupReUploadCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            try
            {
                RT.Service.Resolve<LogGroupController>().InfNcDataLogGroupReUpload(args);

                var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactoryJobExecute(InfType.WorkOrder);

                return curMsg;
            }
            catch (Exception exMsg)
            {
                // 直接返回字符串，不要调用 .L10N()
                return $"执行失败: {exMsg.Message}";
            }
        }
    }
}*/

using Org.BouncyCastle.Crypto;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WebApis;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.KZ.Group.SmomControl.Jobs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    public class InfNcDataLogSOGroupReUploadCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            var list = RT.Service.Resolve<LogGroupController>().GetInfNcDataLogGroupByIds(args);
            var Ids = new List<double>();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Id.ToString()))
                {
                    Ids.Add(item.Id);
                }
            }
            RT.Service.Resolve<LogGroupController>().InfNcDataLogSOR(Ids);
            //RT.Service.Resolve<LogGroupController>().InfNcDataLogGroupReUpload(args);
            /*  String BatchNo = "";
              var list = RT.Service.Resolve<LogGroupController>().GetInfNcDataLogGroupByIds(args);
              var groupGridValues = new List<string>();
              foreach (var item in list)
              {
                  if (!string.IsNullOrEmpty(item.GroupGuid))
                  {
                      groupGridValues.Add(item.GroupGuid.Trim());
                  }
              }
              BatchNo = string.Join(",", groupGridValues.Distinct());

              try
              {
                  //AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

                  var curMsg = RT.Service.Resolve<GroupFactoryJobController>().CorpFactorySOJobExecute(InfType.WorkOrder, BatchNo);
                  //AddLog(string.Join(",", curMsg));

              }
              catch (Exception exMsg)
              {
                  //AddLog($"执行失败，错误信息: {exMsg.Message}");
              }
              finally
              {
                  SendGroupWorkOrderDataToFactoryJob.IsRun = false;
              }
  */
            return "重新完成!".L10N();
        }
    }
}