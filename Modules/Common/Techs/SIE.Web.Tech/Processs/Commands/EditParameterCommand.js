SIE.defineCommand('SIE.Web.Tech.Processs.Commands.EditParameterCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onTypeChanged, this);
        }
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