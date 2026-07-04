using NPOI.SS.Formula.Functions;
using SIE.Api;
using SIE.Domain;
using SIE.Security;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.MES.ListAtts
{
    public partial class ListAttController : DomainController
    {
        /// <summary>
        /// 保存考勤原始数据
        /// </summary>
        /// <param name="listListAttInfo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("保存考勤原始数据")]
        public virtual PostResData SaveListAtt(List<ListAttInfo> listListAttInfo)
        {
            PostResData postRes = new PostResData();
            postRes.DataRows = listListAttInfo.Count;
            postRes.AddRows = 0;
            postRes.FailRows = 0;
            postRes.IgnoreRow = 0;
            foreach (ListAttInfo listAttInfo in listListAttInfo)
            {
                try
                {
                    if (listAttInfo.DataId.IsNullOrEmpty()) { continue; }
                    var entityData = Query<ListAtt>().Where(p => p.DataId == listAttInfo.DataId).FirstOrDefault();
                    if (entityData != null) { postRes.IgnoreRow += 1; continue; }
                    entityData = new ListAtt();
                    entityData.DataId = listAttInfo.DataId;
                    entityData.EventTime = listAttInfo.EventTime;
                    entityData.Pin = listAttInfo.Pin.IsNullOrEmpty() ? "" : listAttInfo.Pin.PadLeft(8, '0');
                    entityData.Name = listAttInfo.Name;
                    entityData.LastName = listAttInfo.LastName;
                    entityData.DeptName = listAttInfo.DeptName;
                    entityData.AreaName = listAttInfo.AreaName;
                    entityData.CardNo = listAttInfo.CardNo;
                    entityData.DevSn = listAttInfo.DevSn;
                    entityData.VerifyModeName = listAttInfo.VerifyModeName;
                    entityData.EventName = listAttInfo.EventName;
                    entityData.EventPointName = listAttInfo.EventPointName;
                    entityData.ReaderName = listAttInfo.ReaderName;
                    entityData.AccZone = listAttInfo.AccZone;
                    entityData.DevName = listAttInfo.DevName;
                    entityData.LogId = listAttInfo.LogId;
                    entityData.AttPlace = listAttInfo.AttPlace;
                    entityData.Mark = listAttInfo.Mark;

                    RF.Save(entityData);
                    postRes.AddRows += 1;
                }
                catch
                {
                    postRes.FailRows += 1;
                }
            }
            return postRes;
        }
    }
}
