SIE.defineCommand('SIE.Web.MES.ProductRoutings.EditBomCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
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
    }
});