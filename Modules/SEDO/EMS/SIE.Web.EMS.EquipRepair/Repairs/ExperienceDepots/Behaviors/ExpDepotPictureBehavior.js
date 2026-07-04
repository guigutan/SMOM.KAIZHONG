Ext.define('SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Behaviors.ExpDepotPictureBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function (config) {
        var me = this;
        if (this.getCurrent() && this.getCurrent() != null) {
            var picture = document.getElementById("expDepotPicture");
            if (this.getCurrent().data.FilePath) {
                var filePath = this.getCurrent().getFilePath();
                var fileName = this.getCurrent().getFileName();
                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
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