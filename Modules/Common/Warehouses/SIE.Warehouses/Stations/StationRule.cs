using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// LED风格样式被引用，不可删除
    /// </summary>
    [DisplayName("LED风格样式被引用，不可删除")]
    [Description("LED风格样式被引用，不可删除")]
    public class LedStypeReferencedRule : NoReferencedRule<LEDShowStyle>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LedStypeReferencedRule()
        {
            Properties.Add(LED.ShowStypeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var entity = o as LEDShowStyle;
                return "LED风格样式[{0}]已经被[{1}]引用，不能删除".L10nFormat(entity.Code, "LED屏幕基础数据".L10N());
            };
        }
    }

    /// <summary>
    /// LED屏幕基础数据被引用，不可删除
    /// </summary>
    [DisplayName("LED屏幕基础数据被引用，不可删除")]
    [Description("LED屏幕基础数据被引用，不可删除")]
    public class LedReferencedRule : NoReferencedRule<LED>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LedReferencedRule()
        {
            Properties.Add(Station.LedIdProperty);
            MessageBuilder = (o, e) =>
            {
                var entity = o as LED;
                return "LED屏幕基础数据[{0}]已经被[{1}]引用，不能删除".L10nFormat(entity.Code, "站台".L10N());
            };
        }
    }

    /// <summary>
    /// 巷道被引用，不可删除
    /// </summary>
    [DisplayName("巷道被引用，不可删除")]
    [Description("巷道被引用，不可删除")]
    public class RoutewayReferencedRule : NoReferencedRule<Routeway>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutewayReferencedRule()
        {
            Properties.Add(Station.RoutewayIdProperty);
            MessageBuilder = (o, e) =>
            {
                var entity = o as Routeway;
                return "巷道[{0}]已经被[{1}]引用，不能删除".L10nFormat(entity.Code, "站台".L10N());
            };
        }
    }

    /// <summary>
    /// 站台被引用，不可删除
    /// </summary>
    [DisplayName("站台被引用，不可删除")]
    [Description("站台被引用，不可删除")]
    public class StationReferencedRule : NoReferencedRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationReferencedRule()
        {
            Properties.Add(StationGroupLine.StationIdProperty);
            MessageBuilder = (o, e) =>
            {
                var entity = o as Station;
                return "站台[{0}]已经被[{1}]引用，不能删除".L10nFormat(entity.Code, "站台组".L10N());
            };
        }
    }

    /// <summary>
    /// 站台仓库验证规则
    /// </summary>
    [DisplayName("站台仓库验证规则")]
    [Description("站台仓库验证规则")]
    public class StationWarehouseRule : EntityRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationWarehouseRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var station = entity as Station;
            if (station.WarehouseId <= 0)
                return;
            var count = RT.Service.Resolve<WarehouseController>().CheckEmployeeWarehouse(station.WarehouseId);
            if (count <= 0)
            {
                e.BrokenDescription = "[{0}]当前用户没有仓库[{0}]权限".L10nFormat(station.Code, station.Warehouse?.Code);
            }
        }
    }

    /// <summary>
    /// 站台巷道验证规则
    /// </summary>
    [DisplayName("站台巷道验证规则")]
    [Description("站台巷道验证规则")]
    public class StaionRoutewayRule : EntityRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StaionRoutewayRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var station = entity as Station;
            if (station.WarehouseId <= 0)
                return;
            if (station.RoutewayId.HasValue && station.RoutewayId.Value > 0)
            {
                var routeway = RT.Service.Resolve<WarehouseController>().GetRouteway(station.WarehouseId, station.RoutewayId.Value);
                if (routeway == null)
                {
                    e.BrokenDescription = "[{0}]站台当前巷道不属于仓库[{0}]".L10nFormat(station.Code, station.Warehouse.Code);
                }
            }
        }
    }

    /// <summary>
    /// 站台配置数据验证规则
    /// </summary>
    [DisplayName("站台配置数据验证规则")]
    [Description("站台配置数据验证规则")]
    public class StaionRule : EntityRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StaionRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var ctl = RT.Service.Resolve<StationController>();
            var station = entity as Station;
            if (station.WarehouseId <= 0)
                return;

            if (!station.Code.StartsWith(station.StationType.ToString()))
            {
                e.BrokenDescription += "[{0}]类型为[{1}]，前缀需为[{2}]\r\n".L10nFormat(station.Code, station.StationType.ToLabel(), station.StationType.ToString());
            }

            if (station.Code.IndexOf(":") < 0)
            {
                e.BrokenDescription += "[{0}]站台格式不正确：前缀  +  :  + 后续内容\r\n".L10nFormat(station.Code);
            }

            if (station.OpcSerialNo.HasValue)
            {
                var count = ctl.GetStationCoutByPropery(station.Id, null, station.OpcSerialNo.Value, null);
                if (count > 0)
                    e.BrokenDescription += "[{0}]站台OPC编号[{1}]不能重复;\r\n".L10nFormat(station.Code, station.OpcSerialNo.Value);
            }
            if (station.OpcScannerNo.HasValue)
            {
                var count = ctl.GetStationCoutByPropery(station.Id, station.OpcScannerNo, null, null);
                if (count > 0)
                    e.BrokenDescription += "[{0}]站台母托盘OPC编号[{1}]不能重复;\r\n".L10nFormat(station.Code, station.OpcScannerNo);
            }
            if (station.OpcSubTrayScannerNo.HasValue)
            {
                var count = ctl.GetStationCoutByPropery(station.Id, null, null, station.OpcSubTrayScannerNo.Value);
                if (count > 0)
                    e.BrokenDescription += "[{0}]站台子托盘OPC编号[{1}]不能重复;\r\n".L10nFormat(station.Code, station.OpcSubTrayScannerNo.Value);
            }

            if (station.BackupExitAddress.IsNotEmpty())
            {
                var stationCodes = station.BackupExitAddress.Split('|');
                if (stationCodes.Length <= 0)
                {
                    e.BrokenDescription += "[{0}]站台备选出库地址格式不正确，多个值用符号“|”隔开;\r\n".L10nFormat(station.Code);
                }
                else
                {
                    if (stationCodes.Contains(station.Code))
                    {
                        e.BrokenDescription += "[{0}]站台备选出库地址不能存在当前站台\r\n".L10nFormat(station.Code);
                    }
                    var list = ctl.GetStationCountByCodes(station.WarehouseId, stationCodes.ToList());
                    if (!list.Any())
                    {
                        e.BrokenDescription += "[{0}]站台备选出库地址不是仓库[{1}]下站台;\r\n".L10nFormat(station.Code, station.Warehouse.Code);
                    }
                    else if (list.Count != stationCodes.Length)
                    {
                        var tempCodes = stationCodes.Except(list.Select(p => p.Code)).ToList();
                        e.BrokenDescription += "[{0}]站台备选出库地址[{1}]不是仓库[{2}]下站台;\r\n".L10nFormat(station.Code, string.Join(",", tempCodes), station.Warehouse.Code);
                    }
                }
            }

            if (station.RelatedStation.IsNotEmpty())
            {
                if (station.Code == station.RelatedStation)
                {
                    e.BrokenDescription += "[{0}]站台关联站台不能存在当前站台\r\n".L10nFormat(station.Code);
                }
                var releateStation = ctl.GetStationByCode(station.WarehouseId, station.RelatedStation);
                if (releateStation == null)
                {
                    e.BrokenDescription += "[{0}]站台关联站台不是仓库[{1}]下站台;\r\n".L10nFormat(station.Code, station.Warehouse.Code);
                }
            }

            if (station.InOutRelatedStation.IsNotEmpty())
            {
                var stationCodes = station.InOutRelatedStation.Split('|');
                if (stationCodes.Length <= 0)
                {
                    e.BrokenDescription += "[{0}]站台出入库模式的关联站台格式不正确，多个值用符号“|”隔开;\r\n".L10nFormat(station.Code);
                }
                else
                {
                    if (stationCodes.Contains(station.Code))
                    {
                        e.BrokenDescription += "[{0}]站台出入库模式的关联站台不能存在当前站台\r\n".L10nFormat(station.Code);
                    }
                    var list = ctl.GetStationCountByCodes(station.WarehouseId, stationCodes.ToList());
                    if (!list.Any())
                    {
                        e.BrokenDescription += "[{0}]站台出入库模式的关联站台不是仓库[{1}]下站台;\r\n".L10nFormat(station.Code, station.Warehouse.Code);
                    }
                    else if (list.Count != stationCodes.Length)
                    {
                        var tempCodes = stationCodes.Except(list.Select(p => p.Code)).ToList();
                        e.BrokenDescription += "[{0}]站台出入库模式的关联站台[{1}]不是仓库[{2}]下站台;\r\n".L10nFormat(station.Code, string.Join(",", tempCodes), station.Warehouse.Code);
                    }
                }
            }
        }
    }
}
