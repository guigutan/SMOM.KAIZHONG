SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddWorkHoursRegisterCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    createNewItem: function () {
        var parentData = this.view.getParent().getData();//主表数据
        if (parentData.getActBeginDate() == null || parentData.getActEndDate() == null) {
            SIE.Msg.showInstantMessage('保养开始时间与保养结束时间不能为空!'.t());
            return false;
        }
        return this.view.createNewItem();
    },
    onItemCreated: function (entity) {
        if (entity) {
            //保养时间默认根据主表的保养时间显示
            var parentData = this.view.getParent().getData();//主表数据
            var model = entity.data;
            var me = this;
            var beginDay = new Date(parentData.getActBeginDate());
            var endDay = new Date(parentData.getActEndDate());
            var hours = Math.ceil((endDay - beginDay) / (1000 * 3600));
            entity.setBeginDay(beginDay);
            entity.setEndDay(endDay);
            entity.setWorkHours(hours);

        }
    }
});