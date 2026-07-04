Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.HandoverConfirmDetailBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function (config) {
        var me = this;
        if (this.getCurrent() && this.getCurrent() != null) {
            var picture = document.getElementById("handoverPicture");
            if (this.getCurrent().data.HandoverAttachment) {
                var filePath = this.getCurrent().getHandoverAttachment();
                var fileName = this.getCurrent().getFileName();
                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                    method: "DownLoadPictureAttachment",
                    params: [filePath, fileName],
                    async: false,
                    token: me.token,
                    callback: function (res) {
                        var file = res.Result;
                        var photo = file.FileContent;
                        if (photo && photo !== null) {
                            picture.src = photo;
                        } else {
                            picture.src = "";
                        }
                    }
                });
            }
        }
    }
});