Ext.define('SIE.Web.EMS.SpareParts.Behaviors.SparePartPictureBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);  
    },
    currentChanged: function (config) {
        var view = this;
        var entity = view.getCurrent();
        if (entity) {
            var picture = document.getElementById("sparePartPicture");
            if (entity.data.FilePath) {
                var filePath = entity.getFilePath();
                var fileName = entity.getFileName();
                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
                    method: "DownLoadPictureAttachment",
                    params: [filePath, fileName],
                    async: false,
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            var file = res.Result;
                            var photo = file.FileContent;
                            if (photo && photo !== null) {
                                picture.src = photo;
                            } else {
                                picture.src = "";
                            }
                        }
                    }
                });
            }
        }
    }
});