using NPOI.POIFS.Properties;
using SIE.Common.Domain;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.RedCardManagment.RedCards
{
    /// <summary>
    /// 
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportRedCardItemSnHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportRedCardItemSnHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get ; set; } = new List<string>()
        {
            "物料SN".L10N(),
            "数量".L10N()
        };
       
        /// <summary>
       /// 列的标准验证
       /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set ; }

       /// <summary>
       /// 创建列验证
       /// </summary>
       /// <returns></returns>
       /// <exception cref="NotImplementedException"></exception>
        public IBusinessImport CreaetColumnValid()
        {
            ColumnValidList = new Dictionary<string, ValidColumn>()
            {
                {ColumnNameList[0],new ValidColumn(ImportDataType._String,true,true) },
                {ColumnNameList[1],new ValidColumn(ImportDataType._String,true,true) }
            };

            return this;
        }

        /// <summary>
        /// 关闭资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// 处理业务逻辑数据
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0)
                throw new ValidationException("导入数据不能为空！".L10N());
            var parentId = Convert.ToDouble(drs[0][ImportDataHandle.ParentId]);
            var redCardEntity = RF.GetById<RedCard>(parentId);
            if (redCardEntity == null)
                throw new EntityNotFoundException("红牌记录不存在。".L10N());
            var itemList =  RT.Service.Resolve<RedCardService>().GetItemSnRetroactives(redCardEntity.Id, null);
            foreach (var row in drs)
            {
                try
                {
                    var newSn = row["物料SN"].ToString();
                    foreach (var i in itemList)
                    {
                        if (newSn == i.SN)
                        {
                            if (row[ImportDataHandle.MessageColumnName] == "")
                            {
                                row[ImportDataHandle.MessageColumnName] = row[ImportDataHandle.MessageColumnName] + "SN已存在";
                            }
                            break;
                        }
                    }
                    if (row[ImportDataHandle.MessageColumnName] != "")
                    {
                        continue;
                    }
                    var sN = row["物料SN"].ToString();
                    var quantity = Convert.ToDecimal(row["数量"]);
                    using (var trans = DB.TransactionScope(RedCardManagmentDataProvider.ConnectionStringName))
                    {
                        //保存ReelID
                        var reel = new ItemSnRetroactive()
                        {
                            SN = sN,
                            Quannity = quantity,
                            Status = RedCardState.Disable,
                            ItemId = redCardEntity.ItemId,
                            SupplierId = redCardEntity.SupplierId,
                            RedCardId = redCardEntity.Id,
                        };
                        RF.Save(reel);
                        trans.Complete();
                    }
                }
                catch (Exception ex)
                {
                    //row[ImportDataHandle.MessageColumnName] = ex.Message;
                    row[ImportDataHandle.MessageColumnName] = row[ImportDataHandle.MessageColumnName] + ex.Message;
                }
            }
        }
    }
}
