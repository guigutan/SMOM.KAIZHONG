SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeEnableCommand', {
    meta: { text: "启用", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (view) {
        //if (view.getSelection().length > 0 && view.getCurrent() !== null) {
        //    var flag = true;
        //    Ext.each(view.getSelection(), function (item) {
        //        if (item.getIsEnable() == 1 || item.isNew() || !item._SchemeWeeks || item._SchemeWeeks.data.length <= 0) {
        //            flag = false;
        //        }
        //    });
        //    return flag;
        //}
        //return false;

        if (view.getSelection().length > 0 && view.getCurrent() !== null) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getIsEnable() == 1) {
                    flag = false;
                }
            });
            return flag;
        }
        return false;
    },
    execute: function (view) {
        var me = view;
        var data = [];
        var entitys = view.getSelection();
        //判断当前选中的数据是否已保存，未保存给出提示信息，已保存则进行启用操作
        var isSaved = entitys.some(function (item) {
            return item.dirty == true
        });
        if (isSaved) {
            SIE.Msg.showWarning('数据未保存，请先保存后再操作'.t());
            return;
        }
        Ext.each(entitys, function (entity) { data.push(entity.data); });
        var msg = Ext.String.format('是否把选择的{0}条日历方案设置为启用状态?'.t(), entitys.length);
        SIE.Msg.askQuestion(msg, function () {
            view.execute({
                data: data,
                success: function (res) {
                    view.loadData();
                }
            }, view);
        });
    }
});