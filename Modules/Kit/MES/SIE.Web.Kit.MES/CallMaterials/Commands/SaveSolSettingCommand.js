SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.SaveSolSettingCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit" },
    /**
        * @protected virtual void
       * 保存后的提示信息
        * @param {type} view
        * @param {type} res
        */
    onSavedMsg: function (view, res) {
        var me = this;
        //me.fireEvent('selectionchange'); 
        //me.fireEvent('load');

        SIE.Msg.showInstantMessage('保存成功'.t());
    },
});