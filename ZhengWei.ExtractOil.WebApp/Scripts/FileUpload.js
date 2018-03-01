
//初始化上传文档组件
function InitDocUploadify() {
    $('#file_upload').uploadify({
        'buttonText': '选择文件',
        'buttonClass': 'btn btn-success fileinput-button',
        'height': 18,
        'swf': swfurl,
        'uploader': uploaderurl,
        'successTimeout': 30,
        'auto': false,
        'multi': true,
        "fileSizeLimit": 1024 * 1024,
        "itemTemplate": true,
        "itemTemplate": '<div id="${fileID}" class="uploadify-queue-item">\
                    <div class="cancel">\
                    <a href="javascript:$(\'#${instanceID}\').uploadify(\'cancel\', \'${fileID}\');">X</a>\
                    </div>\
                    <span class="fileName">${fileName} (大小：${fileSize}；格式：${fileType})</span><span class="data"></span>\
                     <div class="uploadify-progress">\
					<div class="uploadify-progress-bar"><!--Progress Bar--></div>\
					</div><br>\
                   </div>',
        'onSelect': function (file) {
            if (file.name.length > 100) {
                alert("文件名称不能超过100个字符.请精简名称后再上传!");
                $('#file_upload').uploadify("cancel", file.id);
                return;
            }
            $("#btUpload").show();
            $("#btCancel").show();
        },
        'onCancel': function (file) {
//            $("#btUpload").hide();
//            $("#btCancel").hide();
        },
        'onUploadStart': function (file) {           
            $("#file_upload").uploadify("settings", "formData", { fileData: file});
        },
        "onUploadSuccess": Success,
        'onQueueComplete': Complete,
        'overrideEvents': ['onSelectError', 'onDialogClose'],  //注意添加'overrideEvents'选项，要不默认的错误此时还是会出现。 
        'onError': function (event, ID, fileObj, errorObj) {
            if (errorObj.status == 404)
                alert('找不到上传处理应用程序文件！');
            else if (errorObj.type === "HTTP")
                alert('HTTP 错误： ' + errorObj.type + ": " + errorObj.status);
            else if (errorObj.type === "File Size")
                alert('文件:' + fileObj.name + ' 超过最大限制: ' + 1024 * 1024 + 'KB');
            else
                alert('错误信息： ' + errorObj.type + ": " + errorObj.text);
        },
        //检测FLASH失败调用  
        'onFallback': function () {
            alert("您未安装FLASH控件，无法上传！请安装FLASH控件后再试。");
        }
    });
}
function Complete(queueData) {
    $("#btUpload").hide();
    $("#btCancel").hide();
    dialogClose();
    return true;
}
//成功返回处理
function Success(file, data, response) {
    if (data == "") return;
    var result = JSON.parse(data);
    $("#btUpload").hide();
    $("#btCancel").hide();
    if (result.Success == true) {
        //alert(file.name + ":上传成功");
        return true;
    }
    else {
        alert(file.name + ":上传出错:" + result.Message);
        $('#file_upload').uploadify("cancel", file.id);
        return false;
    }
}
