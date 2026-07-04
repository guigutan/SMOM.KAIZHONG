Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.RepairAttachmentBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function (config) {
        var me = this;
        if (this.getCurrent() && this.getCurrent() != null) {
            var picture = document.getElementById("repairPicture");
            if (this.getCurrent().data.FilePath) {
                var filePath = this.getCurrent().getFilePath();
                var fileName = this.getCurrent().getFileName();
                var fileExtesion = this.getCurrent().getFileExtesion();
                var arr = [".png", ".jpg", ".bmp", ".gif", ".webp"
                    , ".avif", ".apng", ".jfif", ".jpeg", ".tif", ".pcx", ".tga", ".exif", ".fpx"
                    , ".svg", ".psd", ".cdr", ".pcd", ".dxf", ".ufo", ".eps", ".ai", ".raw", ".wmf"];

                picture.src = "";

                if (arr.indexOf(fileExtesion) >= 0) {
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                        method: "DownLoadPictureAttachment",
                        params: [filePath, fileName],
                        async: true,
                        token: me.token,
                        callback: function (res) {
                            var file = res.Result;
                            var photo = file.FileContent;
                            if (photo && photo !== null) {
                                picture.src = photo;
                            }
                        }
                    });

                    picture.style.display = "block";
                } else {
                    picture.style.display = "none";
                }
            }
        }
    }
});