using SIE.Domain;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 工位货区保存后触发扩展类
    /// </summary>
    [System.ComponentModel.DisplayName("产线货区提交后")]
    [System.ComponentModel.Description("产线货区提交后,默认生成通用物料货位")]
    public class StorageAreaSubmitted : OnSubmitted<StorageArea>
    {
        /// <summary>
        /// 表示将处理事件的方法（工位货区保存后需要增加通用的货位）
        /// </summary>
        /// <param name="entity">泛型实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(StorageArea entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                var location = new StorageLocation
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsCommon = true,
                    StorageArea = entity
                };
                RF.Save(location);
            }
        }
    }
}