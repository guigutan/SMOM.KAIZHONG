SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.DelDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {

        //需要选中才可使用
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        //获取父视图
        var parent = view._parent;

        //判断父视图及父对象是否为空
        if (parent == null) {
            return false;
        } else if (parent.getCurrent() == null) {
            return false;
        }

        //获取父对象
        var parentCurrent = parent.getCurrent();

        //父对象出库状态为出库不可使用，父对象的相关单据为空时不可使用
        if (parentCurrent.getOutDepotState() != 0) {
            return false;
        }
        else if (parentCurrent.getReleDoc() == null || parentCurrent.getReleDoc() == "")
        {
            return false;
        }
        return true;
    },

    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    var count = res.Result;
                    SIE.Msg.showMessage(Ext.String.format("成功删除{0}条数据。".L10N(),count));
                    view.reloadData();
                },
            });
        });
    }
});