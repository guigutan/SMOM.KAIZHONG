SIE.defineCommand('SIE.Web.EMS.Faults.Commands.AddEquipLargeFaultCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },

    execute: function (view, source) {
        var entity = this.getEditEntity();
        view.execute({
            data: {},
            success: function (res) {
                entity.setCode(res.Result);
            }
        });
        this.edit(entity);
    }
});