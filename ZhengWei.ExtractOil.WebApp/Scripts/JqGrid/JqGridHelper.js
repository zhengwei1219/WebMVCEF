/*该js文件只是适用于jqgrid表格控件（部分方法只对单选有效）——dzc */
var tableObj; //加载时设置 $J("#..") 表格对象
var NullId = '@Guid.Empty';
//取得表格某列的值，数组
function getArray(iCol, grid) {
    if (grid) tableObj = grid;
    var ids = tableObj.getDataIDs();
    var rtn = [];
    for (var i = 0; i < ids.length; i++) {
        rtn.push(tableObj.getCell(ids[i], iCol));
    }
    return rtn;
}
//取得被选中的行id
function getSelRow(grid) {
    if (grid) tableObj = grid;
    var selIndex = tableObj.getGridParam("selrow");
    if (selIndex == null) {
        alert("请单击鼠标选择一行以后再执行操作");
        return null;
    }
    return selIndex;
}
//取得被选中的多行的行索引集合
function getSelRows(grid) {
    if (grid) tableObj = grid;
    var selIndexs = tableObj.getGridParam("selarrrow");
    if (selIndexs == null) {
        alert("请单击鼠标选择一行以后再执行操作");
        return null;
    }
    return selIndexs;
}
//通过行id获取该行的数据对象
function getOneRow(id, grid) {
    if (grid) tableObj = grid;
    if (id == null) {
        alert("没有指定行序号");
        return null;
    }
    return tableObj.getRowData(id);
}
// 获取选中行指定列的值
function getSelRowCol(colname, grid) {
    if (grid) tableObj = grid;
    var selIndex = tableObj.getGridParam("selrow");
    if (selIndex == null) {
        alert("请单击鼠标选择一行以后再执行操作");
        return null;
    }
    return tableObj.getCell(selIndex, colname);
}
// 获取指定行指定列的值
function getOneRowCol(rowindex, colname, grid) {
    if (grid) tableObj = grid;
    if (rowindex == null) {
        alert("行号为空!");
        return null;
    }
    return tableObj.getCell(rowindex, colname);
}
//获取选中行的整行数据对象
function GetSelRowData(grid) {
    if (grid) tableObj = grid;
    var id = getSelRow(grid);
    if (id != null) {
        tableObj.saveRow(id);
        return tableObj.getRowData(id);
    }
}
//移动一行
function moveRow(moveMethod, grid) {
    if (grid) tableObj = grid;
    var id;
    id = getSelRow(grid);
    tableObj.restoreRow(id);
    if (id == null) return;
    var targetId = getTargetId(id, moveMethod)
    if (targetId == -1) return;

    var temp1 = tableObj.getRowData(id);
    var temp2 = tableObj.getRowData(targetId);
    //对调行号
    var tempRn = temp1.rn;
    temp1.rn = temp2.rn;
    temp2.rn = tempRn;
    //对调数据
    tableObj.setRowData(id, temp2);
    tableObj.setRowData(targetId, temp1);
    tableObj.setSelection(targetId);
}

//取得上移时的上一行的id，或下移时的下一行的id
function getTargetId(selId, method, grid) {
    if (grid) tableObj = grid;
    var ids = tableObj.getDataIDs();
    for (var i = 0; i < ids.length; i++) {
        if (selId == ids[i] && method == "up") {
            if (i == 0) return -1;
            else return ids[i - 1];
        }
        if (selId == ids[i] && method == "down") {
            if (i == ids.length - 1) return -1;
            else return ids[i + 1];
        }
    }
}
//删除当前选中行
function deleteRow(grid) {
    if (grid) tableObj = grid;
    var id = getSelRow(grid);
    tableObj.delRowData(id);
}
//删除指定的行
function deleteOneRow(seq, grid) {
    if (grid) tableObj = grid;
    tableObj.delRowData(seq);
}
//最后一行插入一行数据 para为插入的数据数组{name:value}
function addNewRow(para, grid) {
    if (grid) tableObj = grid;
    if (para) {
        var date = new Date();
        para["MetaData.Input_Time"] = date;
        para["MetaData.Verify_Date"] = date;
        para["MetaData.Update_Date"] = date;
        para["MetaData.Input_ORG"] = NullId;
        para["MetaData.Verify_ORG"] = NullId;
        para["MetaData.Update_Org"] = NullId;
        para["MetaData.QualityTagText"] = "初始录入";
        para["MetaData.Quality_Tag_Str"]=2;
    }
    var cnt = GetRowCount();
    tableObj.addRowData(cnt + 1, para);
    tableObj.setRowData(cnt + 1, null, "edited");  // dirty
}
//获取grid的所有的行数
function GetRowCount(grid) {
    if (grid) tableObj = grid;
    var ids = tableObj.getDataIDs();
    return ids.length;
}
//对于本地local数据方式刷新，可用此种方法刷新表格
function refreshGrid(grid, data) {
    if (grid) tableObj = grid;
    tableObj.jqGrid('clearGridData');
    tableObj.jqGrid('setGridParam',
			{
			    datatype: 'local',
			    data: data
			}).trigger("reloadGrid");
}
//提取GRID中列的显示或隐藏,返回bool数组
function getCelldisplay(grid) {
    var visibleFlags = new Array();
    var tbobj = document.getElementById(grid).childNodes[0];
    if (!tbobj.hasChildNodes())
        return null;
    var tdarray = document.getElementById(grid).childNodes[0].childNodes[0].childNodes;
    for (var k = 0; k < tdarray.length; k++) {
        if (tdarray[k].style.display == "none")
            visibleFlags[k] = 'false';
        else
            visibleFlags[k] = 'true';
    }
    return visibleFlags;
}
// 回存编辑状态cell的数据 （编辑状态时取值前注意回存）
function restorEditCell(cellname, iRow, iCol, grid) {
    if(grid) tableObj = grid;
    var selector = iRow + '_' + cellname;
    $("#" + selector).blur(function () {
        grid.saveCell(iRow, iCol); //回存编辑的值
    });
}
//单独针对靶心数据表(TargetSurvey.cshtml)里面的经、纬度转化而创建的方法
function restorEditCell1(cellname, value, iRow, iCol, grid) {
    alert(value);
    alert(cellname);
    if (cellname == 'TargetLatitude') {
        if (value != null || value != "") {
            if (value.indexOf("°", 0) >= 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) >= 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr = arr[1].split("'");
                var fenar2 = arrr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(fenar2 * 60) + parseFloat(miaoar3)).toFixed(4);
            } else if (value.indexOf("°", 0) >= 0 & value.indexOf("\"", 0) >= 0 & value.indexOf("'", 0) < 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(miaoar3)).toFixed(4);
            }
            else if (value.indexOf("°", 0) >= 0 & value.indexOf("\'", 0) >= 0 & value.indexOf("\"", 0) < 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr2 = arrr[1].split("'");
                var fenar2 = arrr[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(fenar2 * 60)).toFixed(4);
            } else if (value.indexOf("°", 0) < 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) >= 0) {
                var arrr = value.split("'");
                var fenar2 = arrr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(fenar2 * 60) + parseFloat(miaoar3)).toFixed(4);
            }
            else if (value.indexOf("°", 0) < 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) < 0) {
                var arrr = value.split("'");
                var fenar2 = arrr[0];
                value = parseFloat(parseFloat(fenar2 * 60)).toFixed(4);
            }
            else {
                var arrr = value.split("\"");
                var miaoar3 = arrr[0];
                value = parseFloat(miaoar3).toFixed(4);
            }
        }
        
    }
    if (cellname == 'TargetLongitude') {
        alert(value);
        alert(cellname);
        if (value != null || value != "") {
            if (value.indexOf("°", 0) >= 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) >= 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr = arr[1].split("'");
                var fenar2 = arrr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(fenar2 * 60) + parseFloat(miaoar3)).toFixed(4);
            } else if (value.indexOf("°", 0) >= 0 & value.indexOf("\"", 0) >= 0 & value.indexOf("'", 0) < 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(miaoar3)).toFixed(4);
            }
            else if (value.indexOf("°", 0) >= 0 & value.indexOf("\'", 0) >= 0 & value.indexOf("\"", 0) < 0) {
                var arr = value.split("°");
                var duar1 = arr[0];
                var arrr2 = arrr[1].split("'");
                var fenar2 = arrr[0];
                value = parseFloat(parseFloat(duar1 * 3600) + parseFloat(fenar2 * 60)).toFixed(4);
            } else if (value.indexOf("°", 0) < 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) >= 0) {
                var arrr = value.split("'");
                var fenar2 = arrr[0];
                var arrr2 = arrr[1].split("\"");
                var miaoar3 = arrr2[0];
                value = parseFloat(parseFloat(fenar2 * 60) + parseFloat(miaoar3)).toFixed(4);
            }
            else if (value.indexOf("°", 0) < 0 & value.indexOf("'", 0) >= 0 & value.indexOf("\"", 0) < 0) {
                var arrr = value.split("'");
                var fenar2 = arrr[0];
                value = parseFloat(parseFloat(fenar2 * 60)).toFixed(4);
            }
            else {
                var arrr = value.split("\"");
                var miaoar3 = arrr[0];
                value = parseFloat(miaoar3).toFixed(4);
            }
        }
    }

    if (grid) tableObj = grid;
    var selector = iRow + '_' + cellname;
    $("#" + selector).blur(function () {
        grid.saveCell(iRow, iCol); //回存编辑的值
    });
}


//通过编辑状态的单元格元素的id回存当前单元格
function restoreOneCell(grid, cellname) {
    if (grid) tableObj = grid;
    var iRow = cellname.split('_')[0];
    var iColstr = cellname.split('_')[1];
    var colModels = tableObj.jqGrid('getGridParam', 'colModel');
    var iCol = getSNFromModels(colModels, iColstr);
    tableObj.saveCell(iRow, iCol); //回存编辑的值
}
//jqgrid单元格时间控件
function celldatepicker(el) {
    $(el).datepicker({
        changeMonth: true,
        changeYear: true,
        showWeek: true,
        dateFormat: "yy-mm-dd",
        onClose: function (dateText, inst) {
            restoreOneCell(tableObj, el.id);
            $(this).blur();
        }
    });
}
//通过select下拉框选择失去焦点后回存数据
function AutoSave(selectel) {
    $(selectel).blur(function () {
        var cellname = $(selectel).parent()[0].firstChild.id;
        restoreOneCell(tableObj, cellname);
    });
}
//通过列名称获取列在models里的序号
function getSNFromModels(colModels, colStr) {
    for (var i = 0; i < colModels.length; i++) {
        if (colModels[i].name == colStr) return i;
    }
}

/*基于jqgrid 表格直接编辑， 提取的公共方法（删除、保存） 
需要指定 保存数据的服务 SaveGridUrl  和 删除数据服务 DeleteGridRowUrl ，通过标识Id删除。
以及表格刷新数据的方法  ;成功返回保存成功的记录数，否则返回错误信息 */

//保存grid数据，仅仅保存被修改过（"dirty"）的行
function GridSave(SaveGridUrl, grid) {
    $("#collapseobj_ActResult").html("");
    $("#DivErrmsgTcat").show();
    if (grid) tableObj = grid;
    var changed = tableObj.getChangedCells("all");
    var saved = 0;
    var failed = 0;
    var errormsg = "";
    if (changed.length == 0) {
        $("#collapseobj_ActResult").append("<span class='prompt'>无数据更新</span>");
        return;
    }
    var hasNewData = false;
    //判断是否有新增的数据，有则需要添加批次，没有则不需要

    for (var i = 0; i < changed.length; i++) {
        var newrow = getOneRow(changed[i].id, grid);
        var IsNull = GridDataIsNull(newrow);
        if (!IsNull.isNull) {
            $("#collapseobj_ActResult").append("<span class='prompt'>第" + changed[i].id + "条" + IsNull.errmsg + "<br/></span>");
            continue; 
         }
        if (newrow.Id == '00000000-0000-0000-0000-000000000000') hasNewData = true;
    }
    if (hasNewData) {
        //先添加一个新批次
        $.ajax({
            type: "POST",
            url: saveBatchUrl, //服务
            data: { wellBoreId: WellboreId, dataCode: code, dataType: dataType },
            async: false,
            success: function (data) {
                if (data.IsSuccess) {
                    SaveGridUrl = SaveGridUrl + "&batchId=" + data.Id;
                    for (var i = 0; i < changed.length; i++) {
                        var newrow = getOneRow(changed[i].id, grid);
                        var IsNull = GridDataIsNull(newrow);
                        if (!IsNull.isNull) { continue; }
                        newrow = DeleteNullJason(newrow);
                        $.ajax({
                            type: "POST",
                            url: SaveGridUrl, //服务
                            data: newrow,
                            async: false,
                            success: function (data) {
                                if (data.IsSuccess) {
                                    saved++;
                                } else {
                                    failed++;
                                    errormsg += "第" + changed[i].id + "条记录保存失败:<br/>" + data.ErrorMessage + "<br/>";
                                }
                            }, error: function (req, status, error) {
                                failed++;
                                errormsg += "第" + changed[i].id + "条记录保存失败:" + error + "<br/>";
                            }
                        })
                    }
                } else {
                    $("#collapseobj_ActResult").append("<span class='error'>保存失败:" + data.ErrorMessage + "<br/></span>");
                }
            }, error: function (req, status, error) {
                $("#collapseobj_ActResult").append("<span class='error'>保存失败:" + error + "<br/></span>");
            }
        })
    }
    else {
        for (var i = 0; i < changed.length; i++) {
            var newrow = getOneRow(changed[i].id, grid);
            var IsNull = GridDataIsNull(newrow);
            if (!IsNull.isNull) { continue; }
            newrow = DeleteNullJason(newrow);
            $.ajax({
                type: "POST",
                url: SaveGridUrl, //服务
                data: newrow,
                async: false,
                success: function (data) {
                    if (data.IsSuccess) {
                        saved++;
                    } else {
                        failed++;
                        errormsg += "第" + changed[i].id + "条记录保存失败:<br/>" + data.ErrorMessage + "<br/>";
                    }
                }, error: function (req, status, error) {
                    failed++;
                    errormsg += "第" + changed[i].id + "条记录保存失败:" + error + "<br/>";
                }
            })
        }
    }
    var alertmsg = "";
    if (saved > 0) {
        alertmsg = "保存成功" + saved + "条记录；";
    }
    if (failed > 0) {
        alertmsg += "<span class='error'>失败" + failed + "条记录;原因：<br/>" + errormsg + "</span>";
    }
    $("#collapseobj_ActResult").append(alertmsg);
    return failed == 0;
}

//删除指定数据行号 默认删除当前选中的数据(单行),需要NullId（Guid.Empty）支持，以判断为空时直接删除，不请求服务器
function GridDelete(Seq, grid, DeleteGridRowUrl) {
    $("#DivErrmsgTcat").show();
    $("#collapseobj_ActResult").html("");
    if (grid) tableObj = grid;
    var seq;
    if (Seq == null) {
        var Id = getSelRowCol("Id", grid);
        if (Id == null) { return; }
        seq = getSelRow(grid);
    }
    else {
        Id = getOneRowCol(Seq, "Id",grid);
    }
    if (Id == null) { return; }
    if (Id == NullId) {
        deleteRow(grid);
        return;
    }
    if (confirm("确定删除第" + seq + "条记录吗？")) {
        $.ajax({
            type: "POST",
            url: DeleteGridRowUrl, //服务
            data: { Id: Id },
            success: function (data) {
                if (data.IsSuccess) {
                    deleteRow(grid);
                    $("#collapseobj_ActResult").append("删除成功！");          
                } else {
                    $("#collapseobj_ActResult").append("<span class='error'>" + "“" + data.EventName + "”操作失败,原因如下:</br>" + data.ErrorMessage + "</span>");
                }
            }, error: function (req, status, error) {
                $("#collapseobj_ActResult").append("<span class='error'>" + "删除失败,原因:" + error + "</span>");
            }
        });
    }
}

//删除选中(多)行;并返回成功的记录数和失败的记录数和错误原因
function GridDeleteAll(grid, DeleteGridRowUrl) {
    $("#DivErrmsgTcat").show();
    $("#collapseobj_ActResult").html("");
    if (grid) tableObj = grid;
    var errormsg = '';
    var deleted = 0;
    var failed = 0;
    var ids = getSelRows(grid);
    if (ids.length <= 0) {
        alert("请选择要删除的行");
        hideMsg();
        return;
    }
    //if (ids == null) return;
    if (confirm("确定删除共" + ids.length + "条记录吗？")) {
        for (var i = 0; i < ids.length; i++) {
            var Id = getOneRowCol(ids[i], "Id",grid);
            if (Id == null) continue;
            if (Id == NullId) {
                ///deleteOneRow(ids[i], grid);
                deleted++;
                continue;
            }
            $.ajax({
                type: "POST",
                url: DeleteGridRowUrl, //服务
                async: false,
                data: { Id: Id },
                success: function (data) {
                    if (data.IsSuccess) {
                        deleted++;
                    }
                    else {
                        errormsg += "第" + ids[i] + "条：" + "“" + data.EventName + "”操作失败,原因如下:</br>" + data.ErrorMessage + "</br>";
                        failed++;
                    }
                }, error: function (req, status, error) {
                    failed++;
                    errormsg += "第" + ids[i] + "条：" + "删除失败,原因:" + error + "</br>";
                }
            });
        }
        var alertmsg = "删除成功" + deleted + "条记录；";
        if (failed > 0) {
            alertmsg += "<span class='error'>" + "失败" + failed + "条记录;原因：</br>" + errormsg + "</span>";
        }
        $("#collapseobj_ActResult").append(alertmsg);
    }
}

//删除选中(多)行;并返回成功的记录数和失败的记录数和错误原因
function GridDeleteAllByIds(grid, DeleteGridRowUrl) {
    $("#collapseobj_ActResult").html("");
    $("#DivErrmsgTcat").show();
    if (grid) tableObj = grid;
    var errormsg = '';
    var deleted = 0;
    var failed = 0;
    var ids = getSelRows(tableObj);
    if (ids == null) return;
    if (ids.length <= 0) { alert("请选择要删除的行"); hideMsg(); return; }
    if (confirm("确定删除共" + ids.length + "条记录吗？")) {
        showMsg("正在删除...");
        var strIds = "";
        for (var i = ids.length - 1; i >= 0; i--) {
            var Id = getOneRowCol(ids[i], "Id");
            if (Id == null || Id == "") continue;
            if (Id == NullId) {//删除未保存的行
                deleteOneRow(ids[i]);
                deleted++;
                continue;
            }
            strIds += Id + ",";
        }
        if (strIds == "") {
            hideMsg();
            return;
        }
        $.ajax({
            type: "POST",
            url: DeleteGridRowUrl, //服务
            async: true,
            data: { Ids: strIds },
            success: function (data) {
                if (data.IsSuccess) {
                    hideMsg();
                    $("#collapseobj_ActResult").append("删除成功！");
                    refFun();
                }
                else {
                    hideMsg();
                    refFun();
                    $("#collapseobj_ActResult").append("<span class='error'>" + "“" + data.EventName + "”操作失败,原因如下:\r\n" + data.ErrorMessage + "<br/></span>");
                }
            }, error: function (req, status, error) {
                hideMsg();
                $("#collapseobj_ActResult").append("<span class='error'>" + error + "</span>");
            }
        });
    }
}
/**************************************************************/
/**************************************************************
通过ajax 提取 用于jqgrid的下拉框编辑的附录代码类集合  附录必须实现 Code和Name 属性*/
/*jqgrid的下拉框接收类型值为 key:value;默认为Code：Name，type：0为Code：Name形式；type：1为Id：Name形式*/


//获取jqgrid能格式化的下拉框列表值。为 {Code:Name}形式，必须指定获取的ajax服务路径
function getSelectStr(url, type) {
    var returnStr = "";
    $.ajax({
        type: "post",
        url: url, //action接受地址
        data: {}, //传入前台的值
        async: false,
        success: function (data) {
            if (type == null) { returnStr = JasonApp2SelectStr(data); }
            else if (type = 1) { returnStr = JasonApp2SelectStr(data, "Id", "Name"); } //可以考虑扩展其他...
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown)
        }
    })
    return returnStr;
    alert(returnStr);
}

//将传回的jason对象（附录集合）转换成jqgrid能格式化的下拉框列表值, 实现为键值对形式例如：默认为Code 和Name
function JasonApp2SelectStr(Jdate, Key, Value) {
    var key = "Code";
    var value = "Name";
    if (Key != null) { key = Key; }
    if (Value != null) { value = Value; }
    var tempstr = ":空;"; //空置 不选择
    for (i = 0; i < Jdate.length; i++) {
        if (i < Jdate.length - 1) {
            tempstr += Jdate[i][key] + ":" + Jdate[i][value] + ";";
        } else {
            tempstr += Jdate[i][key] + ":" + Jdate[i][value];
        }
    }
    return tempstr;
}
//通过value值获取下拉框的文本  value:name
function getSelectText(options, value) {
    var selectarr = options.split(";");
    for (var i = 0; i < selectarr.length; i++) {
        var option = selectarr[i].split(":");
        if (option[0] == value) {
            return option[1] == "空" ? "" : option[1].split('[')[0];
        }
    }
}
//去除jason的空对象
function DeleteNullJason(jasonstr) {
    var newjson = '';
    $.each(jasonstr, function (k, v) {
        if (v != null && v != "") {
            newjson += k + "=" + v + "&";
        }
    }); return newjson;
}
/*****************************************************************/
//添加必填项判断
function GridDataIsNull(newrow) {
    var colModels = tableObj.jqGrid('getGridParam', 'colModel');
    var colNames = tableObj.jqGrid('getGridParam', 'colNames');
    var isNull = true;
    var errmsg = "";
    for (var i = 0; i < colModels.length; i++) {
        if (colModels[i].editrules) {
            if (colModels[i].editrules.required) {
                var colName = colModels[i].name;
                $.each(newrow, function (k, v) {
                    if (k == colName) {
                        if (v == null || v == '') {
                            errmsg += colNames[i] + "不能为空;";
                            isNull = false;
                        }
                    }
                });
            }
        }
    }
    return { isNull: isNull, errmsg: errmsg }; 
}
/*********************表格中添加审批意见**********************************/
//在应用的页面必须有id=dialog的div,id=txt_sul的输入框，且要定义变量sulInfo和数组变量changes
function gridCompleteSetMsg(grid) {
    if (grid) tableObj = grid;
    var ids = tableObj.jqGrid('getDataIDs');
    for (var i = 0; i < ids.length; i++) {
        var cl = ids[i];
        var id = tableObj.jqGrid('getRowData', ids[i]);
        if (getOneRowCol(cl, "MetaData.QualityTagText") == "已提交") {
            tableObj.find("#" + cl).find(":checkbox").attr('disabled', 'disabled');
            for (var j = 0; j < changes.length; j++) {
                if (changes[j].objId == id.Id) {
                    tableObj.jqGrid('setCell', cl, changes[j].cellName, '', { background: 'red' });
                    tableObj.find("#" + cl).find('td').eq(changes[j].icol).attr('title', changes[j].sul);
                }
            }
            var cols = tableObj.jqGrid('getGridParam', 'colModel');
            for (var k = 0; k < cols.length; k++) {
                tableObj.jqGrid('setCell', cl, cols[k].name, '', 'not-editable-cell');
            }
        }
    }
}
function GridDialogHide(grid) {
    if (grid) tableObj = grid;
    sulInfo = $("#txt_sul").val();
    var cl = tableObj.jqGrid('getRowData', vRowid);
    var cols = tableObj.jqGrid('getGridParam', 'colModel');
    //如果changes中已存在，先删掉，然后再处理
    for (var i = 0; i < changes.length; i++) {
        if (changes[i].objId == cl.Id && changes[i].icol == vICol)
            changes.splice(i, 1);
    }
    if (sulInfo != "") {
        tableObj.jqGrid('setCell', vRowid, cols[vICol].name, '', { background: 'red' });
        var changein = { objId: cl.Id, cellName: cols[vICol].name, icol: vICol, sul: sulInfo };
        changes.push(changein);
        tableObj.find("#" + vRowid).find('td').eq(vICol).attr('title', "审核失败：" + sulInfo);
    } else {
        tableObj.find("#" + vRowid).find('td').eq(vICol).attr('style', "text-align:center;");
        tableObj.find("#" + vRowid).find('td').eq(vICol).attr('title', sulInfo);
    }
    $("#txt_sul").val('');
}
function GridonCellSelect(grid, iCol, rowid, MetaDataStatus) {
    if (grid) tableObj = grid;
    var colModels = tableObj.jqGrid('getGridParam', 'colModel')
    if (iCol > 1 && iCol != colModels.length) {
        //在changes中查找如果有匹配的数据，赋值给txt_sul
        var cl = tableObj.jqGrid('getRowData', rowid);
        for (var i = 0; i < changes.length; i++) {
            if (changes[i].objId == cl.Id && changes[i].icol == iCol)
                $("#txt_sul").val(changes[i].sul);
        }
        if (MetaDataStatus != "初始录入" && MetaDataStatus != "一级审核未通过" && MetaDataStatus != "二级审核未通过" && MetaDataStatus != "三级审核未通过") {
            $("#dialog").modal();
        }
    }
}
/*************************End*******************************************/
/*grid中提交被选中的行*/
//提交被选中的行：1、先保存所有有更改的数据；2、取出所有未提交的数据提交
 

function GridSubmit(grid, SaveGridUrl, SubmitGridUrl, paramData) {
    $("#msginfo").modal('show');
    if (grid) tableObj = grid;
    var changed = tableObj.getChangedCells("all");
    var saved = 0;
    var failed = 0;
    var errormsg = "";
    var hasNewData = false;
    //判断是否有新增的数据，有则需要添加批次，没有则不需要

    for (var i = 0; i < changed.length; i++) {
        var newrow = getOneRow(changed[i].id);
        var IsNull = GridDataIsNull(newrow);
        if (!IsNull.isNull) {
            $("#collapseobj_ActResult").html("<span class='prompt'>第" + changed[i].id + "条" + IsNull.errmsg + "<br/></span>");
            continue; 
         }
        if (newrow.Id == '00000000-0000-0000-0000-000000000000') hasNewData = true;
    }
    if (hasNewData) {
        //先添加一个新批次
        $.ajax({
            type: "POST",
            url: saveBatchUrl, //服务
            data: { wellBoreId: WellboreId, dataCode: code, dataType: dataType },
            async: false,
            success: function (data) {
                if (data.IsSuccess) {
                    SaveGridUrl = SaveGridUrl + "&batchId=" + data.Id;
                    for (var i = 0; i < changed.length; i++) {
                        var newrow = getOneRow(changed[i].id);
                        var IsNull = GridDataIsNull(newrow);
                        if (!IsNull.isNull) { continue; }
                        newrow = DeleteNullJason(newrow);
                        $.ajax({
                            type: "POST",
                            url: SaveGridUrl, //服务
                            data: newrow,
                            async: false,
                            success: function (data) {
                                if (data.IsSuccess) {
                                    saved++;
                                } else {
                                    failed++;
                                    errormsg += "第" + changed[i].id + "条记录保存失败:<br/>" + data.ErrorMessage + "<br/>";
                                }
                            }, error: function (req, status, error) {
                                failed++;
                                errormsg += "第" + changed[i].id + "条记录保存失败:" + error + "<br/>";
                            }
                        })
                    }
                } else {
                    $("#collapseobj_ActResult").append("<span class='error'>" + "保存失败:" + data.ErrorMessage + "</span>");
                }
            }, error: function (req, status, error) {
                $("#collapseobj_ActResult").append("<span class='error'>" + "保存失败:" + error + "</span>")
            }
        })
    }
    else {
        for (var i = 0; i < changed.length; i++) {
            var newrow = getOneRow(changed[i].id);
            var IsNull = GridDataIsNull(newrow);
            if (!IsNull.isNull) { continue; }
            newrow = DeleteNullJason(newrow);
            $.ajax({
                type: "POST",
                url: SaveGridUrl, //服务
                data: newrow,
                async: false,
                success: function (data) {
                    if (data.IsSuccess) {
                        saved++;
                    } else {
                        failed++;
                        errormsg += "第" + changed[i].id + "条记录保存失败:<br/>" + data.ErrorMessage + "<br/>";
                    }
                }, error: function (req, status, error) {
                    failed++;
                    errormsg += "第" + changed[i].id + "条记录保存失败:" + error + "<br/>";
                }
            })
        }
    }
    if (failed > 0) {
        errormsg = "保存失败" + failed + "条记录;原因：<br/>" + errormsg;
        $("#collapseobj_ActResult").append("<span class='error'>" + errormsg + "</span>");
        if ($("#msginfo").length > 0) $("#msginfo").modal('hide');
        return;
    }
    $.ajax({
        type: "POST",
        url: SubmitGridUrl, //服务
        data: paramData,
        async: false,
        success: function (data) {
            if (data.IsSuccess) {
                errormsg += "提交成功" + data.Count + "条记录";
                AddInfo2ActResult(errormsg);
            } else {
                errormsg += "提交失败:<br/>" + data.ErrorMessage + "\r\n";
                AddInfo2ActResult("<span class='error'>" + errormsg + "</span>");
                
            }
            if ($("#msginfo").length > 0) $("#msginfo").modal('hide');
        }, error: function (req, status, error) {
            errormsg += "提交失败:" + error + "\r\n";
            AddInfo2ActResult("<span class='error'>" + errormsg + "</span>");
            if ($("#msginfo").length > 0) $("#msginfo").modal('hide');
        }
    });
}//隐藏空列
function HideEmptyCol(grid) {
    if (grid) tableObj = grid;
    var visibleCols = [];
    var hideFlags = [];
    var ids = tableObj.jqGrid('getDataIDs');
    if (ids.length < 1) return;
    var hasRowNumber = tableObj.jqGrid('getGridParam', 'rownumbers');
    var multSelect = tableObj.jqGrid('getGridParam', 'multiselect');
    var colModels = tableObj.jqGrid('getGridParam', 'colModel');
    var start = (hasRowNumber ? 1 : 0) + (multSelect ? 1 : 0);
    var valid = [];
    var colNames = tableObj.jqGrid('getGridParam', 'colNames');
    if (Iscolhd) {
        ShowAllCol(grid, true);
        Iscolhd = false;
        return;
    }
    for (var i = start; i < colModels.length; i++) {
        if (!colModels[i].hidden) visibleCols.push(colModels[i].name);
    }
    var ishide = false;
    for (var i = 0; i < visibleCols.length; i++) {
        ishide = true;
        for (var j = 0; j < ids.length; j++) {
            var cellval = tableObj.getCell(ids[j], visibleCols[i]);
            if ($.trim(cellval) != "") {
                ishide = false;
                break;
            }
        }
        if (ishide) {
            tableObj.hideCol(visibleCols[i]);
        }
        Iscolhd = true;
    }
}
//显示行
function ShowAllCol(grid, inludingempty) {
    if (grid) tableObj = grid;
    var visibleCols = [];
    var hideFlags = [];
    var ids = tableObj.jqGrid('getDataIDs');
    if (ids.length < 1) return;
    var hasRowNumber = tableObj.jqGrid('getGridParam', 'rownumbers');
    var multSelect = tableObj.jqGrid('getGridParam', 'multiselect');
    var colModels = tableObj.jqGrid('getGridParam', 'colModel');
    var start = (hasRowNumber ? 1 : 0) + (multSelect ? 1 : 0);
    var valid = [];
    var colNames = tableObj.jqGrid('getGridParam', 'colNames');
    for (var i = start; i < colModels.length; i++) {
        if (!colModels[i].hidedlg) {
            if (inludingempty) { visibleCols.push(colModels[i].name); }
            else {
                if (!colModels[i].hidden) visibleCols.push(colModels[i].name);
            }
        }
        for (var j = 0; j < visibleCols.length; j++) {
            tableObj.showCol(visibleCols[j]);
        }
    }
}
/**************************End****************************************/
function GetUpdateTargetData(starIndex, datatype, tableObj) {
     var colModels = tableObj.jqGrid('getGridParam', 'colModel');
    var ids = tableObj.jqGrid('getDataIDs');
    var Ids = "";
    for (var i = 0; i < ids.length; i++) {
        Ids += tableObj.jqGrid('getRowData', ids[i]).Id;
        if (i != ids.length - 1) Ids += ",";
    }
    $.ajax({
        type: "POST",
        url: UpdateAllTargetUrl,
        data: { ClassName: datatype, Ids: Ids },
        cache: false,
        success: function (data) {
            for (var j = 0; j < ids.length; j++) {
                var modelId = tableObj.jqGrid('getRowData', ids[j]).Id;
                for (var i = 0; i < data.length; i++) {
                    if (modelId == data[i].EntityId) {
                        for (var m = starIndex; m < colModels.length - 15; m++) {
                            if (colModels[m].name == data[i].PropertyName) {                                
//                                tableObj.jqGrid('setCell', ids[j], colModels[m].name, "", { background: '#fd9f00' });
//                                var colName = tableObj.attr("id") + "_" + colModels[m].name;
//                                tableObj.find("#" + ids[j]).find('td').each(function () {
//                                    if ($(this).attr('aria-describedby') == colName)
//                                        $(this).attr('title', "原始值：" + data[i].OriginalValue);
//                                });
                                tableObj.jqGrid('setCell', ids[j], colModels[m].name, "", { background: '#fd9f00' });
                                var colName = tableObj.attr("id") + "_" + colModels[m].name;
                                if (colModels[m].edittype == "select") {
                                    var OriginalVal = getSelectText(colModels[m].editoptions.value, data[i].OriginalValue);
                                    tableObj.find("#" + ids[j]).find('td').each(function () {
                                        if ($(this).attr('aria-describedby') == colName) {
                                            $(this).attr('title', "原始值：" + OriginalVal);
                                        }
                                    });
                                }
                                else {
                                    tableObj.find("#" + ids[j]).find('td').each(function () {
                                        if ($(this).attr('aria-describedby') == colName)
                                            $(this).attr('title', "原始值：" + data[i].OriginalValue);
                                    });
                                }

                            }
                        }
                    }
                }
            }
        }, error: function (req, status, error) {
            alert(error);
        }
    });
}