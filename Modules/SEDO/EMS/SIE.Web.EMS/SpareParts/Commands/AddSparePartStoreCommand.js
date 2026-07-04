SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.AddSparePartStoreCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" }, 
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: [],
                success: function (res) {
                    entity.setStoreCode(res.Result);
                }
            }, me.view);
        }
    },
});