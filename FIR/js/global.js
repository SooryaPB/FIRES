
var start = new Date(), maxTime = 50000, timeoutVal = Math.floor(maxTime / 100), mytimer;

function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel, ReportName) {
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';

    if (ShowLabel) {
        var row = "";
        for (var index in arrData[0]) {
            row += index.replace("_", " ").toUpperCase() + ',';
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }
        row.slice(0, row.length - 1);
        CSV += row + '\r\n';
    }
    if (CSV == '') {
        alert("Invalid data");
        return;
    }
    //Generate a file name
    var fileName = ReportName + "_";
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
    var link = document.createElement("a");
    link.href = uri;
    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";
    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
function pageHeading(heading_text)
{
    $('#ribbon_text').text(heading_text);
}
function loadContent(link) {
    
    //when applications are deployed in Virtual app mode the application name
    //prefixed to the links.
    if (g_app_name != undefined || g_app_name != "")
        link = g_app_name + link;

    $('#spinner').show();
    $.ajax({
        type: "POST",
        url: link,
        dataType: "html",
        cache: !0,
        success: function (page_content) { 
            //page_content = '<h1 class="ajax-loading-animation"><i class="fa fa-cog fa-spin"></i> Loading...</h1>' + page_content;
            $('#content').css({
                opacity: "0.0"
            }).html(page_content).delay(50).animate({
                opacity: "1.0"
            }, 300)
        },
        error: function (xhr) {
            
            var error = JSON.stringify(xhr);
            $('#content').html('<h4 class="ajax-loading-error"><i class="fa fa-warning txt-color-orangeDark"></i> Error 404! Page not found.</h4>')
        },
        async: !0
    })

}
function get_form_json(formname) {
    var json = {};
    $('#' + formname + ' *').not(':button,:hidden').filter(':input').each(function () {
        json[$(this).attr('name')] = $(this).val();
    });
    return json;
}

function ShowAlert(context,data)
{  
    if(data.hasOwnProperty("aResult"))
    {
        aResult = data.aResult;
        if(aResult.hasOwnProperty("retcode"))
        {
            if (data.aResult.retcode == 0)
                ShowOkMessage(context, data.aResult.result);
            else {
                ShowErrorMessage(context, data.aResult.result);
                return false;
            }
        }
    }
    else
    {
        if(data.hasOwnProperty("retcode"))
        {
            if (data.hasOwnProperty("aResult")) {
                if (data.aResult.retcode == 0)
                    ShowOkMessage(context, data.aResult.result);
                else {
                    ShowErrorMessage(context, data.aResult.result);
                    return false;
                }
            }else
            {
                if (data.retcode == 0)
                    ShowOkMessage(context, data.result);
                else {
                    ShowErrorMessage(context, data.result);
                    return false;
                }
            }
        }
    }
    return true;
}
 
function loadContentUsingGet(link) {
    //when applications are deployed in Virtual app mode the application name
    //prefixed to the links.
    if (g_app_name != undefined || g_app_name != "")
        link = g_app_name + '/' + link;

    $('#spinner').show();
    $.ajax({
        async: true,
        url: link,
        type: 'GET',
        dataType: 'html',
        contentType: 'application/html; charset=utf-8',
        success: function (data) {
            $('#main').html(data);
            $('#spinner').hide();
        },
        error: function (xhr) {

            $.SmartMessageBox({
                title: "<i class='fa fa-sign-out txt-color-orangeDark'></i> Logout <span class='txt-color-orangeDark'><strong>" + $('#show-shortcut').text() + "</strong></span> ?",
                content: "Oh Snap ! Your Session has timed out or the application encountered a non recoverable error. Please relogin....",
                buttons: '[Yes]'

            }, function (ButtonPressed) {
                if (ButtonPressed == "Yes") {
                    $.root_.addClass('animated fadeOutUp');
                    setTimeout(logout, 1000);
                    window.location = '/home/index';
                }
            });

        },
        processData: false
    });
}

function hide(control)
{ 
    $('#' + control).hide();
}

function import_data(dialog_name, title) {

    $(dialog_name).dialog({
        autoOpen: false,
        width: 700,
        height: 470,
        resizable: false,
        modal: false,
        title: title,
        buttons: [{
            html: "Close",
            "class": "btn btn-info",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
    $(dialog_name).show();
    $(dialog_name).dialog("open");
}
function setdatepicker(fldname)
{
    var d = new Date();
    var nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), ap;

    if (nhour == 0) { ap = " AM"; nhour = 12; }
    else if (nhour < 12) { ap = " AM"; }
    else if (nhour == 12) { ap = " PM"; }
    else if (nhour > 12) { ap = " PM"; nhour -= 12; }

    if (nyear < 1000) nyear += 1900;
    if (nmin <= 9) nmin = "0" + nmin;
    if (nsec <= 9) nsec = "0" + nsec;

    $("input[name='" + fldname + "']").datepicker({
        dateFormat: 'dd/mm/yy',
        minDate: 0,
        prevText: '<i class="fa fa-chevron-left"></i>',
        nextText: '<i class="fa fa-chevron-right"></i>',
        onSelect: function (selectedDate) { 
            $("input[name='" + fldname + "']").val(selectedDate + " " + nhour + ":" + nmin + ":" + nsec + ap);
        }
    }); 
}

function updateProgress(percentage) {
    $('#pbar_innerdiv').css("width", percentage + "%");
    $('#pbar_innertext').text(percentage + "%");
}
function start_progress_bar() {
    var now = new Date();
    var timeDiff = now.getTime() - start.getTime();
    var perc = Math.round((timeDiff / maxTime) * 100);
    if (perc <= 100) {
        updateProgress(perc);
        mytimer = setTimeout(start_progress_bar, timeoutVal); 
    }
    else {
        perc = 0;
    }
}
function stop_progress_bar() {
    $('#progress_bar_div').hide();
    clearTimeout(mytimer);
}
function init_progress_bar() {
    start = new Date();
    $('#progress_bar_div').show(),
       $("#progressbar").progressbar({
           value: false
       });
    start_progress_bar();
}

function stop_progress_bar() {
    $('#progress_bar_div').hide();
}

function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel, ReportName) {



    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';

    if (ShowLabel) {
        var row = "";
        for (var index in arrData[0]) {
            row += index.replace("_", " ").toUpperCase() + ',';
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }
        row.slice(0, row.length - 1);
        CSV += row + '\r\n';
    }
    if (CSV == '') {
        alert("Invalid data");
        return;
    }
    //Generate a file name
    var fileName = ReportName + "_";
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
    var link = document.createElement("a");
    link.href = uri;
    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";
    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
function clean_string(data) {
    return data.replace("\r\n", "").trim();
}
function postform(link) { 
    $.ajax({
        async: true,
        url: link,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/html; charset=utf-8',
        success: function (data) {
            $('#content').html(data);
        },
        error: function (xhr) { alert(JSON.stringify(xhr)); },
        processData: false
    });
}
function validate_form(formname, fields) {
    var flag = true;
    if (fields == undefined) {
        $('#' + formname + ' *').not(':button,:hidden').filter(':input').each(function () {
            if ($(this).val() == '') {
                $(this).css('border-color', '#A90329');
                $(this).css('background', '#fff0f0');
                flag = false;
            }
            else {
                $(this).css('border-color', '#7DC27D');
                $(this).css('background', '#f0fff0');

            }
        });
    }
    else {

        $('#' + formname + ' *').not(':button,:hidden').filter(':input').each(function () {

            if (($(this).val() == -1 || $(this).val() == '') && (fields.indexOf($(this).attr('name')) > -1)) {
                $(this).css('border-color', '#A90329');
                $(this).css('background', '#fff0f0');
                flag = false;
            }
            else {
                $(this).css('border-color', '#7DC27D');
                $(this).css('background', '#f0fff0');

            }
        });
    }
    return flag;
}
var prev_date_val = '';
function setdatepicker(fldname) {
    $("input[name='" + fldname + "']").datepicker({
        dateFormat: 'mm/dd/yy',
        prevText: '<i class="fa fa-chevron-left"></i>',
        nextText: '<i class="fa fa-chevron-right"></i>',
        onSelect: function (selectedDate) {
            if (isDateValid(fldname, selectedDate)) $("input[name='" + fldname + "']").val(selectedDate);
        },
        onClose: function (selectedDate, inst) {
            isDateValid(fldname, selectedDate);
        }
    }).on("change", function (e) {
        var curDate = $("input[name='" + fldname + "']").val();
        if (curDate) isDateValid(fldname, curDate);
    }).on("focusin", function (e) {
        prev_date_val = $("input[name='" + fldname + "']").val();
    });
}
function isDateValid(fldname, theDate) {

    var res = theDate.split("/");

    try {
        $.datepicker.parseDate('mm/dd/yy', theDate);
        if (res[2].length != 4) throw 'Invalid Year !';
    } catch (e) {
        $("input[name='" + fldname + "']").val(prev_date_val);
        var label = $("input[name='" + fldname + "']").parent().prev().text();
        if (!label) label = "Date";
        if (theDate) {
            $("input[name='" + fldname + "']").parent().addClass('state-error').datepicker("hide");
            ShowErrorMessage("Invalid Date", "Please enter a valid (mm/dd/yy) <i style='font-size: 1.1em;'>[" + label + "]</i>");
            return false;
        }
    }
    $("input[name='" + fldname + "']").parent().removeClass('state-error');
    return true;
}

function messageBox(mtitle, mcontent, mcolor) {
    if (getInternetExplorerVersion() == -1) {
        if (typeof (mcolor) === 'undefined') mcolor = "#FF0000";
        mcontent = (mcontent && mcontent.length > 0) ? "<i class='fa fa-clock-o'></i> <i><strong>" + mcontent + "</i></strong>" : '';
        $.smallBox({
            title: mtitle,
            content: mcontent,
            color: mcolor,
            iconSmall: "fa fa-check bounce animated",
            timeout: 3000,
            sound: 0
        });
    }
     
}

function ShowOkMessage(title, message) {
    $.smallBox({
        title: "<B>" + title + "</B>",
        content: "<B><i class='fa fa-clock-o'></i> <i>" + message + "</i></B>",
        color: "#5F895F",
        iconSmall: "fa fa-check bounce animated",
        timeout: 3000,
        sound: 0
    });
}

function ShowErrorMessage(title, message) {
    $.smallBox({
        title: "<B>" + title + "</B>",
        content: "<B><i class='fa fa-clock-o'></i> <i>" + message + "</i></B>",
        color: "#FF0000",
        iconSmall: "fa fa-check bounce animated",
        timeout: 3000,
        sound: 0
    });
}

function get_form_json(formname) {
    var json = {};
    $('#' + formname + ' *').filter(':input').each(function () {
        if ( $(this).attr('type') == 'checkbox') {
            if($(this).prop('checked') == true)
                json[$(this).attr('name')] = 1;
            else
                json[$(this).attr('name')] = 0;
        }
        else
            json[$(this).attr('name')] = $(this).val();
    });
    return json;
}

function get_form_json_for_accordian(formname) {
    var json = {};
    $('#' + formname + ' *').filter(':input').each(function () {
        if ($(this).attr('type') == 'checkbox') {
            
            if ($(this).prop('checked') == true){
                var str = $(this).attr('name');
                var split_str = str.split('_');
                var field_names = split_str[0];
                json[field_names] = 1;
            }
            else {
                var str = $(this).attr('name');
                var split_str = str.split('_');
                var field_names = split_str[0];
                json[field_names] = 0;
        }
        }
        else
            {
            var str = $(this).attr('name');
            var split_str = str.split('_');
            var field_names = split_str[0];
            json[field_names] = $(this).val();
        }
    });
    return json;
}
 
function get_form_jqgrid_json(grid, form) {
    var json = {};
    $('#' + form + ' *').filter(':input').each(function () {
        if ($(this).attr('name') != 'undefined')
            json[$(this).attr('name')] = $(this).val();
    });
    json['rows'] = grid.jqGrid('getGridParam', 'rowNum');
    json['page'] = grid.jqGrid('getGridParam', 'page');
    json['sidx'] = grid.jqGrid('getGridParam', 'sortname');
    json['sord'] = grid.jqGrid('getGridParam', 'sortorder');
    return json;
}
function JSONToCSVConvertor(JSONData, ShowLabel) { 

    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';

    if (ShowLabel) {
        var row = "";
        for (var index in arrData[0]) {
            row += index.replace("_", " ").toUpperCase() + ',';
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }
        row.slice(0, row.length - 1);
        CSV += row + '\r\n';
    }
    if (CSV == '') {
        alert("Invalid data");
        return;
    }
    //Generate a file name
    var fileName = 'List' + "_";
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
    var link = document.createElement("a");
    link.href = uri;
    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";
    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function ExportExcelCsv(JSONData,ShowLabel) { 

    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = 'List';
    CSV += '\r\n';
  
    if (ShowLabel) {
        var row = "";
        for (var index in arrData[0]) {
            row += '"' + index.replace(",", " ").toUpperCase()  + '",';
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        for (var index in arrData[i]) { 
            row += '"' + arrData[i][index] + '",';
        }       
        row.slice(0, row.length - 1);
        CSV += row + '\r\n';
    }
    if (CSV == '') {
        ShowErrorMessage("Export DATA","Invalid DATA during export !!")
        return;
    }
    CSV = CSV.replace(/&nbsp;/g, " ");
    CSV = CSV.replace("</b>", "");
    CSV = CSV.replace("<b>", ""); 
    var fileName = 'List';
    var uri = 'data:application/xls;charset=utf-8,' + escape(CSV);
    var link = document.createElement("a");
    link.href = uri; 
    link.style = "visibility:hidden";
    link.download = fileName + ".csv"; 
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

function GetClock() {
    var d = new Date();
    var nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), ap;
    if (nhour == 0) { ap = " AM"; nhour = 12; }
    else if (nhour < 12) { ap = " AM"; }
    else if (nhour == 12) { ap = " PM"; }
    else if (nhour > 12) { ap = " PM"; nhour -= 12; }
    if (nyear < 1000) nyear += 1900;
    if (nmin <= 9) nmin = "0" + nmin;
    if (nsec <= 9) nsec = "0" + nsec;
    $('#ribbon_date').text("" + tday[nday] + ", " + tmonth[nmonth] + " " + ndate + ", " + nyear + " " + nhour + ":" + nmin + ":" + nsec + ap + "");
}

function push_locref(loc) {
    (!/[?&]{1}t=[0-9]+/.test(location.href)) &&
        (loc = location.href + (location.href.indexOf("?") === -1 ? "?t=" : "&t=") + Date.now()) &&
            window.history.pushState(null, null, loc);
}