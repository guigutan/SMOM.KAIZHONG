SIE.defineCommand('SIE.Web.Resources.Enterprises.Commands.DisableResourceCommand', {
    meta: { text: "取消资源", group: "edit", iconCls: "icon-CalendarRemove icon-blue" },

    canExecute: function (view) {
        if (view.getSelection().length > 0) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                flag = flag & item.data.IsResource;
            });
            return flag;
        }
        return false;
    },

    execute: function (view, source) {
        var entitys = view.getSelection();
        if (entitys) {
            var data = [];
            Ext.each(entitys, function (entity) { data.push(entity.data); });

            var msg = Ext.String.format('取消当前选中{0}行 的资源？'.t(), entitys.length);
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