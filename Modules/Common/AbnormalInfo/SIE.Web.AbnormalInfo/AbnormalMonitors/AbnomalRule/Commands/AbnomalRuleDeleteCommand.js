SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule.Commands.AbnomalRuleDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    execute: function (view) {
        var me = this;
        if (view.isListView) {
            var isImmediate = view.isImmediate();
            var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.L10N(), view.getSelection().length);
            if (isImmediate)
                msg += "确定后将直接删除！".L10N();
            else
                msg += "删除后，需要再次点击保存！".L10N();

            SIE.Msg.askQuestion(msg, function () {
                 me.removeChildData(view);
                view.removeSelection();
                var data = view.getData().data;
                if (data.length > 0) {
                    //不选中一行，列表的tbar（如果命令过多）会变更位置
                    view.getControl().setSelection(data.items[0]);
                    view.setCurrent(data.items[0], true);
                } else {
                    view.setCurrent(null, true);
                }
            });
        }
        else {
            //form view       
        }
    },
    /**
     * 移除关联数据节点
     * @param {any} view
     */
    removeChildData: function (view) {
        var selects = view.getSelection();
        selects.forEach(function (item) {
            item.setIsGroup(false);
            item.setIsCacul(false);
            item.setIsWhere(false);
        });
    }
})