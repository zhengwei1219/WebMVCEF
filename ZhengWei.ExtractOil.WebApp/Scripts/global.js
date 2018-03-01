
String.prototype.trim = function () {
    // Strip leading and trailing white-space
    return this.replace(/^\s*|\s*$/g, "");
}

String.prototype.Truncate = function (length) {
    if (this.length <= length) return this;
    return this.substr(0, length) + "...";
}

//replaceAll 扩展替换方法（替换所有） 
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}
Date.prototype.NextMonth = function () {
    var year = this.getFullYear();
    var month = this.getMonth();
    if (month == 11) {
        year = year + 1;
        month = 0;
    }
    else
        month = month + 1;
    return new Date(year, month, this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds());
}

Date.prototype.toGBShortString = function () {
    if (this == null) return '';
    var year = this.getFullYear();
    var month = this.getMonth() + 1;
    var day = this.getDate();
    return year + '-' + month + '-' + day;
}
Date.prototype.toGBString = function () {
    if (this == null) return '';
    var year = this.getFullYear();
    var month = this.getMonth() + 1;
    var day = this.getDate();
    return year + '-' + month + '-' + day;
}

Date.prototype.toGBLongString = function () {
    if (this == null) return '';
    var year = this.getFullYear();
    var month = this.getMonth() + 1;
    var day = this.getDate();
    return year + '-' + month + '-' + day + ' ' + this.getHours() + ':' + this.getMinutes() + ':' + this.getSeconds();
}

Date.prototype.addDays = function (days) {
    return new Date(this.getTime() + days * 24 * 60 * 60 * 1000);
}

// parse date ,if success,return the date ,or return null	
function parseDate(dateString) {
    var reg = /^(\d{1,4})-(\d{1,2})-(\d{1,2})/;
    var r = dateString.match(reg);
    if (r == null) return null;
    r[2] = r[2] - 1;
    var d = new Date(r[1], r[2], r[3]);
    if (d.getFullYear() != r[1]) return null;
    if (d.getMonth() != r[2]) return null;
    if (d.getDate() != r[3]) return null;

    return d;

    var pos1 = dateString.indexOf('-', 0);
    if (pos1 <= 0) return null;
    var pos2 = dateString.indexOf('-', pos1 + 1);
    if (pos2 <= 0) return null;

    var year = parseInt(dateString.substr(0, pos1), 10);
    var month = parseInt(dateString.substr(pos1 + 1, pos2 - pos1 - 1), 10) - 1;
    var day = parseInt(dateString.substr(pos2 + 1, dateString.length - pos2 - 1), 10);
    //alert(year+":"+month+":"+day);
    return new Date(year, month, day);
}
///fill localGrid with the response from ajax method 
//argument: 
//grid :the grid will been filled ;rep:the ajax method's return
function FillGrid(grid, rep) {
    hideMsg();
    try {
        if (rep.error != null) {
            alert(rep.error.Message);
            hideMsg();
            return;
        }
        localData = rep.value;
        grid.setRowProperty("count", localData.length);
        grid.refresh();
        //name.focus();
    }
    catch (e) {
        return;
    }
}

var ajaxLoadingDiv = null;
function createMsgDiv() {
    ajaxLoadingDiv = document.createElement("DIV");
    ajaxLoadingDiv.innerHTML = '<img src="' + imagesUrl +'/loading.gif" border=0 /><span id="loadingText">装载数据....</span>';
    ajaxLoadingDiv.style.position = "absolute";
    ajaxLoadingDiv.style.display = "none";
    ajaxLoadingDiv.style.color = "red";
    ajaxLoadingDiv.style.fontWeight = "bolder";
    ajaxLoadingDiv.style.pixelLeft = 400;
    ajaxLoadingDiv.style.pixelTop = 300;

    document.body.appendChild(ajaxLoadingDiv);
}

function showMsg(msg) {
    if (ajaxLoadingDiv == null) createMsgDiv();
    var textObj = ajaxLoadingDiv.children.item(1);

    if (msg != null && msg.length > 0) {
        textObj.innerText = msg;
    }
    else
        textObj.innerText = "装载数据....";
    ajaxLoadingDiv.style.pixelLeft = (document.body.offsetWidth - 200 - document.body.scrollLeft) / 2;
    ajaxLoadingDiv.style.pixelTop = (document.body.offsetHeight - 20 - document.body.scrollTop) / 2;
    ajaxLoadingDiv.style.display = "";
}

function showAutoHideMessageInSeconds(obj, message, seconds) {
    obj.innerHTML = message;
    obj.style.display = "";
    window.setTimeout("var obj=document.getElementById('" + obj.id + "');obj.style.display='none';obj.innerHTML='';", seconds * 1000);
}

function hideMsg() {
    ajaxLoadingDiv.style.display = "none";
}

//将一个数组的数组string[][]导出为excel文件,需要在其父目录中有ExportExcel文件配合
var exportForm = null;

function ExportToExcel(filename, title, columns, data, isShowRowNumber, visibleFlags) {
    ExportToExcel(filename, title, columns, data, isShowRowNumber, visibleFlags, "");
}

function ExportToExcel(filename, title, columns, data, isShowRowNumber, visibleFlags, wellboreid) {
    //目前导出不支持空数据，如果只导出列头，需添加一行空行
    if (data.length == 0) {
        var onerow = new Array(columns.length);
        data.push(onerow);
    }
    //动态生成一个form
    if (exportForm == null) {
        //不兼容IE9
        // exportForm = document.createElement("<form id='exportForm' method='post' action='../ExportExcel.aspx' />");
            exportForm = document.createElement("form");
            exportForm.setAttribute("id", "exportForm");
            exportForm.setAttribute("method", "post");
            exportForm.setAttribute("action", "../Export/ExportExcel"); 
    }
    var html = "";
    html = html + "<input type='text' name='filename' value='" + filename + "'>";
    html = html + "<input type='text' name='title' value='" + title + "'>";
    html = html + "<input type='text' name='columns' value='" + columns.join(',') + "'>";
    html = html + "<input type='text' name='isShowRowNumber' value='" + isShowRowNumber + "'>";
    html = html + "<input type='text' name='wellboreid' value='" + wellboreid + "'>";
    var str = '';
    for (var i = 0; i < data.length; i++) {
        var temp = data[i].join("|").replaceAll('^', '~');
        str = str + temp + "^";
    }
    html = html + "<input type='text' name='data' value='" + str + "'>";
    html = html + "<input type='text' name='visibleFlags' value='" + visibleFlags.join(',') + "'>";
    html = html + "<input type='submit' name='sub' value='submit'>";
    exportForm.innerHTML = html;
    exportForm.style.display = "none";
    document.body.appendChild(exportForm);
    //提交form,交由页面来处理
    exportForm.submit();
}

//get the selected value from radio group
function getSelectedValueFromRadio(radioId) {
    var rbl = document.getElementsByName(radioId);
    if (rbl != null) {
        for (var i = 0; i < rbl.length; i++) {
            if (rbl[i].checked) {
                return rbl[i].value;
            }
        }
    }
    else
        return "";
}
// #############################################################################
// function to toggle the collapse state of an object, and save to a cookie
function toggle_collapse(objid) { 
    obj = document.getElementById("collapseobj_" + objid);
    img = document.getElementById("collapseimg_" + objid);
    cel = document.getElementById("collapsecel_" + objid); 
    if (!obj) {
        // nothing to collapse!
        if (img) {
            // hide the clicky image if there is one
            img.style.display = "none";
        }
        return false;
    }

    if (obj.style.display == "none") {
        obj.style.display = "";
        //save_collapsed(objid, false);
        if (img) {
            img_re = new RegExp("_collapsed\\.gif$");
            img.src = img.src.replace(img_re, '.gif');
        }
        if (cel) {
            cel_re = new RegExp("^(thead|tcat)(_collapsed)$");
            cel.className = cel.className.replace(cel_re, '$1');
        }
    }
    else {
        obj.style.display = "none";
        //save_collapsed(objid, true);
        if (img) {
            img_re = new RegExp("\\.gif$");
            img.src = img.src.replace(img_re, '_collapsed.gif');
        }
        if (cel) {
            cel_re = new RegExp("^(thead|tcat)$");
            cel.className = cel.className.replace(cel_re, '$1_collapsed');
        }
    }
    return false;
}

function set_cookie(name, value, expires) {
    if (!expires) {
        expires = new Date();
    }
    document.cookie = name + "=" + escape(value) + "; expires=" + expires.toGMTString() + "; path=/";
}

// #############################################################################
// function to retrieve a cookie
function fetch_cookie(name) {
    cookie_name = name + "=";
    cookie_length = document.cookie.length;
    cookie_begin = 0;
    while (cookie_begin < cookie_length) {
        value_begin = cookie_begin + cookie_name.length;
        if (document.cookie.substring(cookie_begin, value_begin) == cookie_name) {
            var value_end = document.cookie.indexOf(";", value_begin);
            if (value_end == -1) {
                value_end = cookie_length;
            }
            return unescape(document.cookie.substring(value_begin, value_end));
        }
        cookie_begin = document.cookie.indexOf(" ", cookie_begin) + 1;
        if (cookie_begin == 0) {
            break;
        }
    }
    return null;
}

function save_collapsed(objid, addcollapsed) {
    var collapsed = fetch_cookie('witsml_collapse');
    var tmp = new Array();

    if (collapsed != null) {
        collapsed = collapsed.split('\n');

        for (var i in collapsed) {
            if (collapsed[i] != objid && collapsed[i] != '') {
                tmp[tmp.length] = collapsed[i];
            }
        }
    }

    if (addcollapsed) {
        tmp[tmp.length] = objid;
    }

    expires = new Date();
    expires.setTime(expires.getTime() + (1000 * 86400 * 365));
    set_cookie('witsml_collapse', tmp.join('\n'), expires);
}


//is select item selected
function isItemSelected(source, arguments) {
    if (arguments.Value == "" || arguments.Value < 0) {
        arguments.IsValid = false;
    }
    else {
        arguments.IsValid = true;
    }
}

//屏蔽输入框中的星号，例如有时扫描护照时，如果扫描的有错误，返回的是星号，这时应屏蔽掉这些星号
function maskAsterisk(e) {
    var ee = (e) ? e : window.event;
    if (ee.keyCode == 42) {
        return false;
    }
}

//定义护照号码的匹配正则表达式
var passportNumberPatt = /^[DPSAKT][E\d]?[\.\d]\d{6,8}\s*/;

//给指定字符串用指定字符补齐
//itemstr为目标字符，fillwith需要填充的单个字符,length 长度，如果目标字符长度>length 直接截断返回
//direction 0在前补齐，1在后补齐
function FillUp(itemstr, fillwith, length, direction) {
    if (itemstr.length > length) {
        return itemstr.substr(0, length);
    }
    else {
        var temp = '';
        for (var i = 0; i < (length - itemstr.length); i++) {
            temp = temp + fillwith.toString();
        }
        if (direction == 0) {
            return itemstr + temp.toString();
        }
        else {
            return temp.toString() + itemstr;
        } 
    }
}

function CloseDialog() {
    window.close();
}

//增加日期控件
$(document).ready(function () {
    $(".chzn-select").chosen();

    $("input.datetimepicker").bind({
        click: function () {
            WdatePicker({
                dateFmt: 'yyyy-MM-dd HH:mm',autoUpdateOnChanged:true });
        }
    });
    $("input.datepicker").bind({
        click: function () {
            WdatePicker({ dateFmt: 'yyyy-MM-dd' });
        }
    });
});
function formatDate(fdate,formatStr) {
    var fTime, fStr = 'ymdhis';
    if (!formatStr)
        formatStr = "y-m-d";
    if (fdate)
        fTime = new Date(fdate);
    else
        fTime = new Date();
    var formatArr = [
        fTime.getFullYear().toString(),
        (fTime.getMonth() + 1).toString(),
        fTime.getDate().toString(),
        fTime.getHours().toString(),
        fTime.getMinutes().toString(),
        fTime.getSeconds().toString()
    ];
    for (var i = 0; i < formatArr.length; i++) {
        formatStr = formatStr.replace(fStr.charAt(i), formatArr[i]);
    }
    return formatStr;
}
Date.prototype.format = function(format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    };
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    };
    return format;
}

//日期比较大小
function DateDuibi(a, b) {
    var arr = a.split("-");
    var starttime = new Date(arr[0], arr[1], arr[2]);
    var starttimes = starttime.getTime();

    var arrs = b.split("-");
    var lktime = new Date(arrs[0], arrs[1], arrs[2]);
    var lktimes = lktime.getTime();
    if (starttimes < lktimes) {
        return true;
    }
    else
        return false;
}
//计算时间差天数 精确到小时 yyyy-MM-dd hh:mm:ss
function daysBetween(DateOne, DateTwo) {
    if (DateOne == "" || DateTwo == "") return 0;
    var dateonearr = DateOne.split(' ');
    var datetwoarr = DateTwo.split(' ');
    if (dateonearr.length < 1 || datetwoarr.length < 1) return 0;
    try {
        var OneMonth = dateonearr[0].split('-')[1];
        var OneDay = dateonearr[0].split('-')[2];
        var OneYear = DateOne.substring(0, DateOne.indexOf('-'));
        var TwoMonth = datetwoarr[0].split('-')[1];
        var TwoDay = datetwoarr[0].split('-')[2];
        var TwoYear = DateTwo.substring(0, DateTwo.indexOf('-'));
        var onehour = dateonearr[1].split(':')[0];
        var oneminute = dateonearr[1].split(':')[1];
        var twohour = datetwoarr[1].split(':')[0];
        var twominute = datetwoarr[1].split(':')[1];
        //计算两个时间相差几个小时 
        var cha = (Date.parse(TwoMonth + '/' + TwoDay + '/' + TwoYear) - Date.parse(OneMonth + '/' + OneDay + '/' + OneYear)) / 86400000 * 24 + (parseInt(twohour, '10') - parseInt(onehour, '10')) + (parseInt(twominute, '10') - parseInt(oneminute, '10')) / 60;
        //计算两个时间相差几天
        //var cha = Date.parse(OneMonth + '/' + OneDay + '/' + OneYear) - Date.parse(TwoMonth + '/' + TwoDay + '/' + TwoYear)/ 86400000;             
        return cha.toFixed(2);
    } catch (e) {
        return 0;
    }
}

//获取参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

//注释内容隐藏显示Fun
function picShowHide() {
    $j("[src='../images/icon_memo.gif']").click(function () {
        $j(this).parent().hide(500);
    });
}

//设定量纲
function SetDimensions(dataClass) { 
    if (dataClass != "") {
        //var dataClass = '@Model.GetType().Name';  
        //打开设置量纲的页面,返回设置结果 
        var rtn = window.showModalDialog(SetDimensionsUrl + "?a=" + Math.random() + "&dataClass=" + dataClass, window, 'dialogWidth=510px;dialogHeight=400px;resizable:yes;help:no;status:no,scroll:yes');
        if (rtn == undefined) return;
        //保存到配置文件  
        $.post(encodeURI(saveDimensionsUrl + "?propertyUnits=" + rtn + "&dataClass=" + dataClass), function (data) {
            location.reload(true); //刷新页面
        })
    }
}

function RefPage() { 
    if (window.location.href.indexOf("&aa=") > 0) window.location.href = window.location.href.substring(0, window.location.href.indexOf("&aa="));
    window.location.href += "&aa=" + Math.random();
}
function RefQRPage(EventType) {
    if (window.location.href.indexOf("&aa=") > 0) window.location.href = window.location.href.substring(0, window.location.href.indexOf("&aa="));
    window.location.href += "&event=" + EventType + "&aa=" + Math.random();
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}


function initPageComFun(Id, code, dataType, BatchId) {
    $("#dvToolsbar").hide();
    $("#dvToolsbarImport").hide();
    $("#dvToolsbarapp").hide();
    $("#SpanSetDimensions").hide();
    $("#SpanQR").hide();
    $("#GriddvToolsbarDoc").hide();
    $.ajax({
        type: "POST",
        url: initPageCom + "&showtype=" + GetQueryString("showtype"),
        data: { modelId: Id, code: code, dataType: dataType, BatchId: BatchId },
        cache: false,
        dataType: "json",
        success: function (data) {

            //显示审核意见  
            if (data.OperButton == "none") {
                $("#Table_project").find("input,select,textarea").attr("ReadOnly", "true");
                $(".chzn-container").attr("disabled", "disabled");
                //$("#Table_project").find("input,select,textarea").attr("ReadOnly", "true");
            } else {
                $("#dvToolsbar").show();
                $("#GriddvToolsbarDoc").show();
                $("#dvToolsbarImport").show();
                $("#SpanQR").show();
            }
            if (data.AppButton != "none")
                $("#dvToolsbarapp").show();
            if (data.AppSolButton == "") {
                $("#Table_project").find(".textboxtd").each(function () {
                    $(this).append("<a href='javascript:void(0)' onclick=AppSul('" + $(this).find("*:first").attr("id") + "',this)><img alt='审核意见' src='" + imagesUrl + "/未标题-3.png'/></a>");
                });
            } 
            
            if (data.AppSolShow == "") {
                for (var i = 0; i < data.AppSolList.length; i++) {
                    if (data.IsCenterQC == "Y") {
                        $("#Table_project").find("input,select,textarea").each(function () {
                            if ($(this).attr("name") != undefined) {
                                if ($(this).attr("name").replace(new RegExp("_", "gm"), '').toLowerCase() == data.AppSolList[i].ApprovalProp.replace(new RegExp("_", "gm"), '').toLowerCase()) {

                                    $(this).parent().append("<font style='color:red;'>" + "中心库质检错误：" + data.AppSolList[i].Solution + "</font>");
                                }
                            }
                        });

                        //                    $("#Table_project").find("#" + data.AppSolList[i].ApprovalProp).parent().append("<font style='color:red;'>"  + "中心库质检错误：" + data.AppSolList[i].Solution + "</font>");
                    }
                    else
                        $("#Table_project").find("#" + data.AppSolList[i].ApprovalProp).parent().append("<font style='color:red;'>审核失败：" + data.AppSolList[i].Solution + "</font>");
                }
            }
            if (data.DimensionsButton == "") {
                $("#SpanSetDimensions").show();
            }
        }
    });
}
function initGridPageComFun(code, dataType) {
    $("#GriddvToolsbar").hide();
    $("#GriddvToolsbarImport").hide();
    $("#GriddvToolsbarImport1").hide();
    $("#GriddvToolsbarapp").hide();
    $("#btnExportDatas").hide();
    $("#btnSetDimensions").hide();
    $("#GriddvToolsbarCuverSet").hide();
    $("#GriddvToolsbarDoc").hide();
    $("#btQR").hide();
    $.ajax({
        type: "POST",
        url: initAprovalRecords + "&showtype=" + GetQueryString("showtype"),
        data: { modelId: "00000000-0000-0000-0000-000000000000", code: code, dataType: dataType, isGrid: false },
        async: false,
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data.OperButton == "") {
                $("#GriddvToolsbar").show();
                $("#GriddvToolsbarImport").show();
                $("#GriddvToolsbarImport1").show();
                $("#GriddvToolsbarCuverSet").show();
                $("#GriddvToolsbarDoc").show();
                $("#btQR").show();
            }
            if (data.ExportButton == "") {
                $("#btnExportDatas").show();
            }
            if (data.DimensionsButton == "") {
                $("#btnSetDimensions").show();
            }
        }
    });
}


function AppSul(thisa, thisb) {
    var inputtext = "<input maxlength='20' type='text'id='sul_" + thisa + "' name='sul_" + thisa + "'  class='appsul-text' style='width:120px'>";
    if ($(thisb.parentNode).find("#sul_" + thisa).length == 0)
        $(inputtext).appendTo(thisb.parentNode);
    else
        $(thisb.parentNode).find("#sul_" + thisa).remove();

}
//textarea中截取最大长度字符
function checkMaxLen(obj, maxlength) {
    if (obj.value.length > maxlength) {
        obj.value = obj.value.substr(0, maxlength);
    }
}

function ExecuteQRBasis(tableName, Id, Code,ShowById) {
    $.ajax({
        type: "POST",
        url: ExcuteQRUrl,
        data: { TableName: tableName, Ids: Id, Code: Code },
        cache: false,
        dataType: "json",
        success: function (data) {
            var inplist1 = document.getElementById("Table_project").getElementsByTagName("INPUT");
            var inplist2 = document.getElementById("Table_project").getElementsByTagName("TEXTAREA");
            var inplist3 = document.getElementById("Table_project").getElementsByTagName("SELECT");
            var ids = [];
            for (var j = 0; j < inplist1.length; j++) {
                if (ShowById) ids.push(inplist1[j].id);
                else ids.push(inplist1[j].name);
            }
            for (var j = 0; j < inplist2.length; j++) {
                if (ShowById) ids.push(inplist2[j].id);
                else ids.push(inplist2[j].name);
            }
            for (var j = 0; j < inplist3.length; j++) {
                //ids.push(inplist3[j].id);
                if (ShowById) ids.push(inplist3[j].id);
                else ids.push(inplist3[j].name);
            }
            //debugger;
            //            for (var k = 0; k < ids.length; k++) {
            //                if ($("#Table_project").find("#sperror_" + ids[k]).val() != undefined) $("#Table_project").find("#sperror_" + ids[k]).remove();
            //            }
            $("span[id^=sperror_]").remove();
            if (data.length > 0) {

                for (var i = 0; i < data.length; i++) {
                    for (var j = 0; j < ids.length; j++) {
                        if (ids[j].toLowerCase() == data[i].PropertyName.toLowerCase()) {
                            if (ShowById) $("#Table_project").find("#" + ids[j]).parent().append("<span id='sperror_" + ids[j] + "' style='color:red;'>" + "质检错误:" + data[i].OriginalValue + "；</span>");
                            else $("#Table_project").find("input[name='" + ids[j] + "']").parent().append("<span id='sperror_" + ids[j] + "' style='color:red;'>" + "质检错误:" + data[i].OriginalValue + "；</span>");
                            break;
                        }
                    }
                }
            }
            else {
                alert("质检通过");
            }
        }, error: function (req, status, error) {
            alert(error);
        }
    });
}

function ExecuteQRBasisSubmit(tableName, Id, Code,ShowById) {
    $.ajax({
        type: "POST",
        url: ExcuteQRUrl,
        data: { TableName: tableName, Ids: Id, Code: Code },
        cache: false,
        dataType: "json",
        success: function (data) {
            var inplist1 = document.getElementById("Table_project").getElementsByTagName("INPUT");
            var inplist2 = document.getElementById("Table_project").getElementsByTagName("TEXTAREA");
            var inplist3 = document.getElementById("Table_project").getElementsByTagName("SELECT");
            var ids = [];
            for (var j = 0; j < inplist1.length; j++) {
                if(ShowById) ids.push(inplist1[j].id);
                else ids.push(inplist1[j].name);
            }
            for (var j = 0; j < inplist2.length; j++) {
                if (ShowById) ids.push(inplist2[j].id);
                else ids.push(inplist2[j].name);
            }
            for (var j = 0; j < inplist3.length; j++) {
                if (ShowById) ids.push(inplist3[j].id);
                else ids.push(inplist3[j].name);
            }
            for (var k = 0; k < ids.length; k++) {
                if ($("#Table_project").find("#sperror_" + ids[k]).val() != undefined) $("#Table_project").find("#sperror_" + ids[k]).remove();
            }
            if (data.length >= 0) {
                for (var i = 0; i < data.length; i++) {
                    for (var j = 0; j < ids.length; j++) {
                        if (ids[j].toLowerCase() == data[i].PropertyName.toLowerCase()) {
                            if (ShowById) $("#Table_project").find("#" + ids[j]).parent().append("<span id='sperror_" + ids[j] + "' style='color:red;'>" + "质检错误:" + data[i].OriginalValue + "</span>");
                            else $("#Table_project").find("input[name='" + ids[j] + "']").parent().append("<span id='sperror_" + ids[j] + "' style='color:red;'>" + "质检错误:" + data[i].OriginalValue + "</span>");
                            break;
                        }
                    }
                }
            }
            //                else {
            //                    alert("质检通过");
            //                 }  
        }, error: function (req, status, error) {
            alert(error);
        }
    });
}

//检查文件名中非法字符
function ContainsSpecial(origin) {
    if (origin.indexOf("+") > -1  || origin.indexOf("?") > -1 || origin.indexOf("%") > -1) {
        {
            return "文件名称不能含有特殊字符‘+’、‘？’、‘%’等";
        }
    }
    return "";
}