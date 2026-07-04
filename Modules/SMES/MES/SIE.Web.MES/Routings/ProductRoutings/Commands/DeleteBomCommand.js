SIE.defineCommand('SIE.Web.MES.ProductRoutings.DeleteBomCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        //装配工序且产品版本暂停才能使用
        if (view != undefined && view.parentView != undefined && view.parentView.layout != undefined && view.parentView.layout.version != undefined) {
            var selectNode = view.parentView.layout.getSelectNode();
            var version = view.parentView.layout.version;
            //单体的判断
            if (version.IsPause === 0 && selectNode && selectNode.designerData.ProcessType === 'Assembly')   //0 No 非暂停
            {
                return false;
            }

            if (selectNode && selectNode.designerData.ProcessType === 'BatchAssembly' && view.parentView.layout.batchRelation.IsPause == 0)   //0 No 非暂停
            {
                return false;
            }


            return selectNode && view.getCurrent() && (selectNode.designerData.ProcessType === 'Assembly'
                || selectNode.designerData.ProcessType === 'BatchAssembly');
        }

        return false;
    },
    execute: function (view) {
        var current = view.getCurrent();
        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.L10N(), view.getSelection().length);
        SIE.Msg.askQuestion(msg, function () {
            view.removeSelection();
            view.setCurrent(null, true);
            var parentView = view.parentView;
            var layout = parentView.layout;
            if (layout && current && parentView.layout.getSelectNode()) {
                var selectNode = parentView.layout.getSelectNode();

                //var processId = selectNode.designerData.ProcessId;
                var activityId = selectNode.id;

                if (layout.processBomDic[activityId]) {
                    var bomList = layout.processBomDic[activityId];
                    var bom = bomList.where(function (p) { return p.id === current.data.id; }).first();
                    if (bom) {
                        bomList.remove(bom);
                    }
                }
            }
        });
    }
});