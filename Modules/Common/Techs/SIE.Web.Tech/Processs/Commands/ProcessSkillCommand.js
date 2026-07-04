SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessSkillCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'SkillId', targetClassName: 'SIE.Resources.Skills.Skill' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {     
        var p = view.getParent();
        if (!p) { return true; }
        var entity = p.getCurrent();
        if (entity === null) { return false };
        return entity.dirty == false;//entity.data.CreateBy > 0;
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var skillId = item.getId();
                if (me._sourceViewSelectItems.indexOf(skillId) === -1) {
                    var skill = { ProcessId: me._sourceId, SkillId: skillId };
                    operationDatas.push(skill);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();
                    me._ownerView.loadChildData(true);
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
});