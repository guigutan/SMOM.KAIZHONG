Ext.define("SIE.Web.Dock.DockAppoints.DockAppointAction", {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0 && e.entity.getData()[e.property] != e.oldvalue) {
                if (e.property == 'AppointType' || e.property == 'YardZoneId' || e.property == 'AppointDate') {
                    clearAppointDock(entity);
                }
            }

            function clearAppointDock(entity) {
                entity.setAppointStartDate(null);
                entity.setAppointEndDate(null);
                entity.setUseHours(null);
                entity.setAppointDock(null);
            }
        },

        ////验证数据
        validateDockAppointData: function (indata) {
            if (indata.AppointType == null) {
                SIE.Msg.showError("预约类型不能为空！".t());
                return false;
            }

            if (indata.BillNo == null || indata.BillNo == "") {
                SIE.Msg.showError("单据号不能为空！".t());
                return false;
            }

            if (indata.YardZoneId == null || indata.YardZoneId <= 0) {
                SIE.Msg.showError("预约地点不能为空！".t());
                return false;
            }

            if (indata.CarNum == null || indata.CarNum == "") {
                SIE.Msg.showError("车牌号不能为空！".t());
                return false;
            }

            if (indata.Contacts == null || indata.Contacts == "") {
                SIE.Msg.showError("联系人不能为空！".t());
                return false;
            }

            if (indata.ContactNum == null || indata.ContactNum == "") {
                SIE.Msg.showError("联系电话不能为空！".t());
                return false;
            }

            if (indata.IDNumber == null || indata.IDNumber == "") {
                SIE.Msg.showError("身份证号不能为空！".t());
                return false;
            }

            if (indata.AppointDate == null) {
                SIE.Msg.showError("预约日期不能为空！".t());
                return false;
            }

            if (indata.AppointDock == null || indata.AppointDock == "") {
                SIE.Msg.showError("预约时段不能为空！".t());
                return false;
            }

            if (indata.UseHours == null || indata.UseHours <= 0) {
                SIE.Msg.showError("预计占用不能为空，并且必须大于0！".t());
                return false;
            }

            ////if (indata.AppointStartDate == null || indata.AppointEndDate == null) {
            ////    SIE.Msg.showError("预约时间不能为空！".t());
            ////    return false;
            ////}

            ////if (indata.AppointEndDate <= indata.AppointStartDate) {
            ////    SIE.Msg.showError("预约结束时间必须大于开始时间！".t());
            ////    return false;
            ////}

            return true;
        },


    }
});
