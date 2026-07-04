SIE.defineCommand('SIE.Web.Resources.Enterprises.Commands.EnableResourceCommand', {
    meta: { text: "设为资源", group: "edit", iconCls: "icon-CalendarCheck icon-blue" },

    canExecute: function (view) {
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.isNew()) { return false; }
        if (view.getSelection().length > 0) {
            var flag = false;
            Ext.each(view.getSelection(), function (item) {
                flag = flag | item.data.IsResource;
            });
            return !flag;
        } 
        return true;
    },

    execute: function (view, source) {
        var entitys = view.getSelection();
        if (entitys) {
            var data = [];
            Ext.each(entitys, function (entity) { data.push(entity.data); });

            var msg = Ext.String.format('确认设定 当前选中的{0}行 为资源吗？'.t(), entitys.length);
            SIE.Msg.askQuestion(msg, function () {
                view.execute({
                    data: data,
                    success: function (res) {
                        view.loadData();
                    }
                }, view);
            });
        }
    }
});