SIE.defineCommand('SIE.Web.Resources.ProcessTechs.Commands.ProcessTechAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setCode(data.Code);
                    entity.setIsScheduling(data.IsScheduling);
                }
            }, me.view);
            entity.setIsScheduling(true)
        }
        this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
    },
    _onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        var processTechTypeId = entity.getProcessTechTypeId();
        if (processTechTypeId) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Resources.ProcessTechs.DataQuery.ProcessTechTypeQueryer",
                method: "GetProcessTechType",
                params: [processTechTypeId],
                async: false,
                token: me.view.token,
                callback: function (res) {
                    if (res.Success && res.Result != null) {
                        var algorithmMarking=res.Result.getData().items[0].getAlgorithmMarking();
                        entity.setAlgorithmMarking(algorithmMarking);
                    }
                },
            });
        }
        
        if (e.property.length > 0) {
             //勾选，偏移时间为空不可编辑
            //不勾选，偏移时间必填，转款时间为空不可编辑
            if (e.property.indexOf('IsScheduling') >= 0) {
                if (entity.getIsScheduling()) {
                    entity.setOffsetTime(null);
                }
                else {
                    entity.setTransferTime(null);
                }
            }
        }
    }
});