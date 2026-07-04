Ext.define("SIE.Web.Inventory.Strategy.OnShelvesRuleDetailAction", {
    statics: {
        onEntityPropertyChanged: function (e) {          
            var entity = e.entity;
            var value = e.value;
            if (e.property.length > 0) {
                if (e.property == 'Strategy') {
                    if (value != null) {
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy01.value && value != SIE.Inventory.Strategy.StrategyType.Strategy02.value) {
                            entity.setFromLocationId(null);
                            entity.setFromLocationId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy02.value && value != SIE.Inventory.Strategy.StrategyType.Strategy03.value) {
                            entity.setToAreaId(null);
                            entity.setToAreaId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy01.value && value != SIE.Inventory.Strategy.StrategyType.Strategy04.value) {
                            entity.setToLocationId(null);
                            entity.setToLocationId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy07.value && value != SIE.Inventory.Strategy.StrategyType.Strategy08.value) {
                            entity.setToLogicAreaId(null);
                            entity.setToLogicAreaId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy08.value) {
                            entity.setFromLogicAreaId(null);
                            entity.setFromLogicAreaId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy09.value) {
                            entity.setToStationId(null);
                            entity.setToStationId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy10.value) {
                            entity.setToStationGroupId(null);
                            entity.setToStationGroupId_Display(null);
                        }
                        if (value != SIE.Inventory.Strategy.StrategyType.Strategy03.value && value != SIE.Inventory.Strategy.StrategyType.Strategy07.value &&
                            value != SIE.Inventory.Strategy.StrategyType.Strategy09.value && value != SIE.Inventory.Strategy.StrategyType.Strategy10.value) {
                            entity.setFromStationId(null);
                            entity.setFromStationId_Display(null);
                        }
                    }
                    else {
                        entity.setFromLogicAreaId(null);
                        entity.setFromLogicAreaId_Display(null);
                        entity.setFromLocationId(null);
                        entity.setFromLocationId_Display(null);
                        entity.setToAreaId(null);
                        entity.setToAreaId_Display(null);
                        entity.setToLocationId(null);
                        entity.setToLocationId_Display(null);
                        entity.setToLogicAreaId(null);
                        entity.setToLogicAreaId_Display(null);
                        entity.setToStationId(null);
                        entity.setToStationId_Display(null);
                        entity.setToStationGroupId(null);
                        entity.setToStationGroupId_Display(null);
                    }
                }
                else if (e.property == 'SceneType') {
                    if (value != null) {
                        entity.setStrategy(null);
                        if (value == SIE.Inventory.Strategy.SceneType.ASRS.value) {
                            entity.setMaxItemNum(null);
                            entity.setMaxLotNum(null);
                            entity.setLocationState(null);
                            entity.setMaxLocNum(null);
                            entity.setItemCategory(null);
                            entity.setExistsItem(null);
                            entity.setIsSameItemOnhand(null);
                            entity.setToLotAtt01(null);
                            entity.setToLotAtt01Value(null);
                            entity.setToLotAtt02(null);
                            entity.setToLotAtt02Value(null);
                            entity.setToLotAtt03(null);
                            entity.setToLotAtt03Value(null);
                            entity.setToLotAtt04(null);
                            entity.setToLotAtt04Value(null);
                            entity.setSpaceLimit1(null);
                            entity.setSpaceLimit2(null);
                            entity.setSpaceLimit3(null);
                            entity.setSpaceLimit4(null);
                            entity.setUpProcessType(null);
                            entity.setPickProcessType(null);
                            entity.setLocationType(null);
                            entity.setStorageLimit1(null);
                            entity.setStorageLimit2(null);
                            entity.setStorageLimit3(null);
                            entity.setStorageLimit4(null);
                        }
                        else {
                            entity.setFromStationId(null);
                            entity.setFromStationId_Display(null);
                        }
                    }
                    else {
                        entity.setStrategy(null);
                    }
                }
                else if (e.property == 'FromLogicAreaId') {
                    if (value != null) {
                        entity.setFromStationId(null);
                        entity.setFromStationId_Display(null);
                    }
                }
                else if (e.property == 'FromStationId') {
                    if (value != null) {
                        entity.setFromLogicAreaId(null);
                        entity.setFromLogicAreaId_Display(null);
                    }
                }
            }
        }
    }
});