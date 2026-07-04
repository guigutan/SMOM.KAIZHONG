using SIE.Common.DataSync;
using SIE.Common.Domain;
using SIE.Common.TimeStamp;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Threading;
using SIE.Utils;
using System;
using System.Linq;

namespace SIE.Equipments
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class EntityLogHelper
    {
        /// <summary>
        /// 生成变更日志
        /// </summary>
        /// <param name="entityType">类型</param>
        /// <param name="newEntity">新实体</param>
        /// <param name="oldEntity">旧实体</param>
        public static void CreateEntityLog(Type entityType, Entity newEntity, Entity oldEntity)
        {
            AsyncHelper.InvokeSafe(() =>
            {
                var copy = newEntity.GetRepository().New();

                copy.Clone(newEntity, CloneOptions.ReadDbRow());

                var meta = CommonModel.Entities.Find(entityType);

                foreach (var property in copy.PropertyContainer.GetNonReadOnlyProperties())
                {
                    //跳过
                    if (property is IRefEntityProperty || property is IListProperty || property is IViewProperty || property.Name == "IsEnableAsset")
                    {
                        continue;
                    }

                    //跳过
                    if (property.DeclareType == typeof(DataEntity) || property.DeclareType == typeof(DataSyncExtension) || property.DeclareType == typeof(TimeStampExtension))
                    {
                        continue;
                    }

                    var vp = property;
                    if (property is IRefIdProperty)
                    {
                        vp = (property as IRefIdProperty).RefEntityProperty;
                    }

                    var mp = meta.Property(vp);

                    //是否启用了不记日志的特性
                    if (mp.Attributes.Any(p => p is NonLoggingAttribute))
                    {
                        continue;
                    }

                    string oldValue;
                    string newValue;

                    if (property is IRefIdProperty)
                    {
                        //引用，取到引用的实体

                        var oldValueEntity = oldEntity.GetRefEntity((property as IRefIdProperty).RefEntityProperty);
                        var newValueEntity = copy.GetRefEntity((property as IRefIdProperty).RefEntityProperty);

                        if (oldValueEntity == null && newValueEntity == null)
                        {
                            continue;
                        }

                        if ((oldValueEntity != null && newValueEntity != null)
                            && object.Equals(oldValueEntity.GetId(), newValueEntity.GetId()))
                        {
                            continue;
                        }

                        //获取引用实体的显示属性
                        if (oldValueEntity != null)
                        {
                            string displayMember = oldValueEntity.GetType().CustomAttributes
                                .FirstOrDefault(p => p.AttributeType.Name == nameof(DisplayMemberAttribute))?.ConstructorArguments[0].Value.ToString();
                            oldValue = oldValueEntity.GetType().GetProperty(displayMember).GetValue(oldValueEntity, null).ToString();
                        }
                        else
                        {
                            oldValue = string.Empty;
                        }

                        //获取引用实体的显示属性
                        if (newValueEntity != null)
                        {
                            string displayMember = newValueEntity.GetType().CustomAttributes
                                .FirstOrDefault(p => p.AttributeType.Name == nameof(DisplayMemberAttribute))?.ConstructorArguments[0].Value.ToString();
                            newValue = newValueEntity.GetType().GetProperty(displayMember).GetValue(newValueEntity, null).ToString();
                        }
                        else
                        {
                            newValue = string.Empty;
                        }
                    }
                    else
                    {
                        var oldValueObject = oldEntity.GetProperty(property);
                        var newValueObject = copy.GetProperty(property);

                        if (object.Equals(oldValueObject, newValueObject))
                        {
                            continue;
                        }

                        if (property.PropertyType.IsEnum)
                        {
                            oldValue = EnumViewModel.EnumToLabel((Enum)oldValueObject).L10N();
                            newValue = EnumViewModel.EnumToLabel((Enum)newValueObject).L10N();
                        }
                        else
                        {
                            oldValue = oldValueObject?.ToString();
                            newValue = newValueObject?.ToString();
                        }
                    }

                    var log = new EntityLog();
                    log.TypeName = entityType.GetQualifiedName();
                    log.PropertyName = property.Name;
                    log.OldValue = oldValue;
                    log.NewValue = newValue;
                    log.EntityId = copy.GetId().ConvertTo<double>();
                    log.CreateBy = RT.IdentityId;
                    log.CreateDate = DateTime.Now;
                    log.PropertyLabel = mp.Label.L10N();
                    RF.Save(log);
                }

            });
        }
    }
}