SIE.defineCommand('SIE.Web.Fixtures.Models.Commands.AddFixtureEncodeCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        entity.mon(entity, 'propertyChanged', me.fixtureModelChanged, me);
        SIE.invokeDataQuery({
            method: 'GetFixtureEncodeNO',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.Fixtures.Models.DataQueryers.FixtureEncodeDataQueryer',
            token: me.view.token,
            success: function (res) {
                entity.setCode(res.Result);
                if (res.Result != "")
                    entity.belongsView.getControl().actionables[0].grid.columns[1].focus();
            }
        });



    },
    fixtureModelChanged: function (e) {
        var entity = e.entity;
        var me = this;
        if (e.property == 'FixtureModelId' && e.value != null) {
            SIE.invokeDataQuery({
                method: 'GetFixtureEncodeProjects',
                params: [e.value],
                action: 'queryer',
                type: 'SIE.Web.Fixtures.Models.DataQueryers.FixtureEncodeDataQueryer',
                token: me.view.token,
                success: function (res) {
                    var newEntitys = [];
                    Ext.each(res.Result.data.items, function (item) {
                        var newEntity = Ext.create("SIE.Fixtures.Models.FixtureEncodeMaintainProject");
                        newEntity.generateId();
                        newEntity.data.MaintainProjectId = item.data.MaintainProjectId;
                        newEntity.data.MaintainProjectId_Display = item.data.MaintainProjectId_Display;
                        newEntity.data.InStorageMaintain = item.data.InStorageMaintain;
                        newEntity.data.CommonMaintain = item.data.CommonMaintain;
                        newEntity.data.OnlineMaintain = item.data.OnlineMaintain;
                        newEntity.data.ToStorageMaintain = item.data.ToStorageMaintain;
                        newEntity.data.ProjectConsumable = item.data.Consumable;
                        newEntity.data.ProjectMethod = item.data.Method;
                        newEntity.data.ProjectTool = item.data.Tool;
                        newEntity.data.ProjectMinValue = item.data.MinValue;
                        newEntity.data.ProjectMaxValue = item.data.MaxValue;
                        newEntitys.push(newEntity);
                    });
                    entity.FixtureEncodeMaintainProjectList().setData(newEntitys);
                }
            });
        }
    }
});