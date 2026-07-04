using SIE.Domain;
using SIE.Warehouses.Stations;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Warehouses.Stations.Commands
{
    /// <summary>
    /// 选择站台
    /// </summary>
    public class SelectStationCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var groupLines = new EntityList<StationGroupLine>();
            var dtls = args.Data.ToJsonObject<List<StationGroupLine>>();
            Check.NotNullOrEmpty(dtls, nameof(dtls));
            if (null == dtls || dtls.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(dtls)));
            }
            var group = RF.GetById<StationGroup>(dtls.FirstOrDefault().StationGroupId);
            int index = 0;
            if (group.StationGroupLineList.Any())
                index = group.StationGroupLineList.Max(p => p.SequenceNo);
            dtls.ForEach(p =>
            {
                index++;
                var groupLine = new StationGroupLine();
                groupLine.SequenceNo = index;
                groupLine.StationGroupId = p.StationGroupId;
                groupLine.StationId = p.StationId;
                groupLines.Add(groupLine);
            });
            RF.Save(groupLines);
            return true;
        }
    }
}
