SIE.defineCommand('SIE.Web.MES.ProductRoutings.AddBomCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        //装配工序且产品版本暂停才能使用
        if (view != undefined && view.parentView != undefined && view.parentView.layout != undefined && view.parentView.layout.version != undefined) {
            var version = view.parentView.layout.version;
            var selectNode = view.parentView.layout.getSelectNode();

            //单体的判断
            if (version.IsPause === 0 && selectNode && selectNode.designerData.ProcessType === 'Assembly')   //0 No 非暂停
            {
                return false;
            }
            if (selectNode && selectNode.designerData.ProcessType === 'BatchAssembly' && view.parentView.layout.batchRelation.IsPause == 0)   //0 No 非暂停
            {
                return false;
            }

            return selectNode && (selectNode.designerData.ProcessType === 'Assembly'
                || selectNode.designerData.ProcessType === 'BatchAssembly');
        }

        return false;
    },
    onItemCreated: function (entity) {
        if (entity) {
            entity.setQty(1);
        }
        var parentView = this.view.parentView;
        var layout = parentView.layout;
        var selectNode = parentView.layout.getSelectNode();
        if (layout && selectNode) {

            var activityId = selectNode.id;

            if (layout.processBomDic[activityId]) {
                var bomList = layout.processBomDic[activityId];
                bomList.push(entity.data);
            }
            else {
                layout.processBomDic[activityId] = [entity.data];
            }
        }
    }
});