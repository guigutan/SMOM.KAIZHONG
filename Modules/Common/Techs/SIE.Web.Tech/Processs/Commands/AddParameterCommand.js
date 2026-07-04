SIE.defineCommand('SIE.Web.Tech.Processs.Commands.AddParameterCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        this.callParent();
        entity.setType(1);
        entity.setDescription('通过');
        entity.mon(entity, 'propertyChanged', this.onTypeChanged, this.view)
    },
    onTypeChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == "Type") {
                if (e.entity.data.Type == 4) {
                    e.entity.setDescription('');
                }
                else {
                    var t = e.entity.data.Type;
                    if (t == 1) {
                        e.entity.setDescription('通过');                        
                    }
                    else if (t == 2)
                    {
                        e.entity.setDescription('失败');
                    }
                    else if (t == 3) {
                        e.entity.setDescription('任意');
                    }
                   
                    e.entity.setScript('');
                }
            }
        }
    }

});