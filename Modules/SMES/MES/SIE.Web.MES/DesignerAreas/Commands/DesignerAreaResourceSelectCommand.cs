using SIE.Domain;
using SIE.MES.DesignerAreas;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DesignerAreas.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class DesignerAreaResourceSelectCommand : ViewCommand
    {
        /// <summary>
        /// 添加产线
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var datas = args.Data.ToJsonObject<List<DesignerAreaResources>>();
            Check.NotNullOrEmpty(datas, nameof(datas));

            if (null == datas || datas.Count == 0)
                throw new ArgumentNullException(nameof(datas));

            foreach (var item in datas)
            {
                var data = new DesignerAreaResources();
                data.DesignerAreaId = item.DesignerAreaId;
                data.ResourceId = item.ResourceId;
                savedData.Add(data);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
