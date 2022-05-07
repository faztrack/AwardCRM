var currentUpdateEvent;
var addStartDate;
var addEndDate;
var updateStartDate;
var updateEndDate;
var globalAllDay;
var IsSerTktNumber = false;
var currentStarDate;
var nevent;
var AutoComCustID;
var SalesPersonID;
var SalesPersonName;
var ChildEventID;
var AutoComSection;
var isSelectable;
var ParentEventName;
var ParentEventId;

function openTab(evt, tabName) {
    // console.log(evt, tabName);
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

function updateEvent(event, element) {

    document.getElementById("btnEventLink").click(); // Click On Link Tab
    //$.ajax({
    //    type: "POST",
    //    url: "Copyschedulecalendar.aspx/GetEmployeeById",
    //    data: "{'empId':'" + event.employee_id + "'}",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        console.log("GetEmployeeById:" + data.d);
    //        var result = data.d;
    //        SalesPersonName = result;
    //    },
    //    error: function (e) {
    //        //console.log("there is some error");
    //        console.log(e);
    //    }
    //});

    // console.log("updateEvent, event.employee_id: " + event.employee_id);
    if (!isSelectable) {
        return false;
    }
    var lnkText = event.id;
    var iCounter = 0;

    $("#head_grdCalLinkInfo tr:has(td)").each(function () {
        var cell = $(this).find("td:eq(4)").text().toLowerCase();
        //console.log('cell: ' + cell + ' ' + cell.indexOf(lnkText) + ' ' + iCounter);

        if (cell.indexOf(lnkText) < 0) {
            $(this).css('display', 'none');
        }
        else {
            $(this).css('display', '');
            iCounter++;
        }

    });



    // console.log(event.title);
    // console.log("event.start: " + event.start);
    var stdate = new Date(event.start);
    // var nStartDate = dateformatting(stdate);

    // console.log(nStartDate);

    if ($(this).data("qtip")) $(this).qtip("destroy");

    currentUpdateEvent = event;

    $('#updatedialog').dialog('open');


    if (event.EstimateID == "0" || event.EstimateID == "") {
        $("#eventName").val(decodeHtml(event.title).replace("-", "").replace("[", "").replace("]", "").trim());
    }
    else {
        $("#eventName").val(decodeHtml(event.title));
    }

    ParentEventName = $("#eventName").val();
    ParentEventId = event.id;

    //console.log(" ParentEventName: " + ParentEventName + ", ParentEventId: " + ParentEventId);

    $("#eventDesc").val(decodeHtml(event.description));

    $("#txtNotes").val(decodeHtml(event.operation_notes));

    //  $("#txtTradePartner").val(decodeHtml(event.trade_partner));

    $("#eventId").val(event.id);
    $("#eventStart").val(dateformatting(event.start));//+ " " + timeformatting(event.start));

    $("select#eventStartTime option").each(function () {
        this.selected = (this.text == timeformatting(event.start).replace(/^0/, ""))
    });



    $("#eventSalesPerson").val(event.employee_name);
    SalesPersonID = event.employee_id;

    // console.log("updateEvent, event.employee_name: " + event.employee_name);


    updateStartDate = new Date(dateformatting(event.start) + " " + timeformatting(event.start));

    if (event.end === null) {
        $("#eventEnd").val(dateformatting(event.start));
        $("select#eventEndTime option").each(function () { this.selected = (this.text == timeformatting(event.start).replace(/^0/, "")) });
        updateEndDate = new Date(dateformatting(event.start) + " " + timeformatting(event.start));
    }
    else {
        $("#eventEnd").val(dateformatting(event.end));// + " " + timeformatting(event.end));
        $("select#eventEndTime option").each(function () { this.selected = (this.text == timeformatting(event.end).replace(/^0/, "")) });
        updateEndDate = new Date(dateformatting(event.end) + " " + timeformatting(event.end));
    }

    //console.log(timeformatting(event.start));
    //If Time is 00:00
    if (timeformatting(event.start).indexOf('00:00') != -1) {
        setEventStartTime();

    }
}

function dateformatting(strDate) {
    // console.log("dateformatting, strDate: " + strDate)
    //var date = (strDate.getMonth() + 1) + "/" + strDate.getDate() + "/" + strDate.getFullYear();

    var date = moment(strDate).format('MM/DD/YYYY');

    //  var date = new Date(unix_timestamp * 1000);


    //var a = new Date(strDate * 1000);
    //var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    //var year = a.getFullYear();
    //var month = months[a.getMonth()];
    //var date = a.getDate();
    //var hour = a.getHours();
    //var min = a.getMinutes();
    //var sec = a.getSeconds();
    // var time = date + ' ' + month + ' ' + year + ' ' + hour + ':' + min + ':' + sec;

    //  var time = year + '/' + month + '/' + date ;

    //  console.log("dateformatting, date: " + date);

    return date;
}

function timeformatting(strDate) {
    // var nstrDate = strDate;
    // console.log("nstrDate: " + nstrDate);

    strDate = new Date(moment(strDate).format());




    //  console.log("datetimeNow: " + datetimeNow);
    //  console.log("timeformatting: " + strDate);

    var hh = "";

    if (strDate.getHours() > 12) {
        hh = strDate.getHours() - 12;

        if (hh < 10)
            hh = "0" + hh;
    }
    else if (strDate.getHours() < 10)
        hh = "0" + strDate.getHours();
    else
        hh = strDate.getHours();

    var mm = "";
    if (strDate.getMinutes() === 0) {
        mm = "00";
    }
    else if (strDate.getMinutes() < 10)
        mm = "0" + strDate.getMinutes();
    else
        mm = strDate.getMinutes();

    var ss = "";
    if (strDate.getSeconds() === 0)
        ss = "00";
    else if (strDate.getSeconds() < 10)
        ss = "0" + strDate.getSeconds();
    else
        ss = strDate.getSeconds();

    var tt = strDate.getHours() < 12 ? "AM" : "PM";


    //var time = hh + ":" + mm + ":" + ss + " " + tt;
    var time = hh + ":" + mm + " " + tt;

    return time;
}

function updateSuccess(updateResult) {
    $('#loading').hide();
    console.log(updateResult);
    //var theHtml = updateResult;
    //$("#head_grdCalLinkInfo").empty();
    //$("#head_grdCalLinkInfo").append(theHtml);

    $("#head_hdnEventId").val(updateResult);
    $("#head_btnHdn").click();
}

function updateSuccessNotes(updateResult) {
    //console.log(updateResult);    
}

function updateSuccessTradePartner(updateResult) {
    //console.log(updateResult);    
}

function deleteSuccess(deleteResult) {
    $('#loading').hide();
    //console.log(deleteResult);
    $("#head_btnHdn").click();
}

function cancelSuccess(deleteResult) {
    //console.log(deleteResult);
}

function getSuccess(getResult) {
    //console.log(getResult);   

    var vGotoDate = new Date();
    if (getResult != "") {

        vGotoDate = new Date(getResult);
    }

    var strTitleDate = $(".fc-header-title").text();
    var aryTitleDate = strTitleDate.split(" ");
    var strTitleMonth = aryTitleDate[0];
    var strTitleYear = aryTitleDate[1];

    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    if (strTitleMonth != monthNames[vGotoDate.getMonth()]) {
        $('#calendar').fullCalendar('gotoDate', vGotoDate);


    }
    else {
        $('#calendar').fullCalendar('refetchEvents');

    }
}

function addSuccess(addResult) {
    $('#loading').hide();
    // if addresult is -1, means event was not added
    //console.log("globalAllDay: " + $("#head_hdnCustomerID").val());

    if (addResult != -1) {
        $('#calendar').fullCalendar('renderEvent',
						{
						    title: decodeHtml($("#addEventName").val()),
						    start: addStartDate,
						    end: addEndDate,
						    id: addResult,
						    description: $("#addEventDesc").val(),
						    allDay: false,
						    EstimateID: $("#head_hdnEstimateID").val(),
						    CustomerID: $("#head_hdnCustomerID").val(),
						    employee_id: SalesPersonID,
						    employee_name: $("#addSalesPersonName").val(),
						    TypeID: $("#head_hdnTypeID").val(),
						    className: 'fc-default'//$("#head_hdnServiceCssClass").val()
						},
						true // make the event "stick"
					);


        $('#calendar').fullCalendar('unselect');
        document.getElementById("addEventName").value = '';
        document.getElementById("addEventDesc").value = '';
        document.getElementById("lbladdEventName").className = 'hidden';
        document.getElementById("lbladdEventDesc").className = 'hidden';
        document.getElementById("lblTime").className = 'hidden';
        document.getElementById("lblRequired").className = 'hidden';
        //document.getElementById("head_hdnEstimateID").value = '0';
        //document.getElementById("head_hdnCustomerID").value = '0';
        //document.getElementById("head_hdnEmployeeID").value = '0';
        document.getElementById("head_hdnTypeID").value = '0';
        document.getElementById("head_hdnServiceCssClass").value = '';
    }
}

function UpdateTimeSuccess(updateResult) {
    //debugger;
    //console.log(updateResult);
    //var theHtml = updateResult;
    //$("#" + gridID).html(theHtml);
    $("#head_btnHdn").click();

    $('#loading').hide();
    hideimagefunction();
}

function UpdateTimeSuccessAll(updateResult) {

    $('#calendar').fullCalendar('refetchEvents');

    $('#loading').hide();
    hideimagefunction();
}

function addeventOnDrop(start, title) {

    var addStartDate = new Date(start + " " + "6:00:00 AM");
    var addEndDate = new Date(start + " " + "7:00:00 AM");

    var eventToAdd = {
        title: title,
        description: '',
        start: addStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
        end: addEndDate.format("dd-MM-yyyy hh:mm:ss tt"),
        allDay: false,
        employee_id: 0,
        employee_name: 'TBD TBD'
    };

    PageMethods.addEvent(eventToAdd, addSuccess);
}

function selectDate(start, end, allDay) {


    //  start = moment(start).format('YYYY/MM/DD hh:mm');
    //   end = moment(end).format('YYYY/MM/DD hh:mm');

    console.log("selectDate- start: " + start, "end: " + end);

    $('#addDialog').dialog('open');

    //    $("#addEventStartDate").text("" + start.toLocaleString());
    //    $("#addEventEndDate").text("" + end.toLocaleString());

    $("#addEventStartDate").val("" + dateformatting(start));
    $("#addEventEndDate").val("" + dateformatting(start));

    //set default value in "addEventStartTime" dropdown
    // $("select#addEventStartTime option")
    //.each(function () { this.selected = (this.text == "6:30 AM"); });

    addStartDate = new Date($("#addEventStartDate").val() + " " + $("#addEventStartTime option:selected").text());
    addEndDate = new Date($("#addEventEndDate").val() + " " + $("#addEventEndTime option:selected").text());
    globalAllDay = allDay;
    console.log("selectDate: " + $("#addEventStartDate").val() + ", " + $("#addEventEndDate").val());
}

function setAddEventStartTime() {
    addStartDate = new Date($("#addEventStart").val() + " " + $("#addEventStartTime option:selected").text());
    var nStartDate = addStartDate;

    var endTime = timeformatting(nStartDate.addHours(1)).replace(/^0/, "");
    addStartDate = new Date($("#addEventStart").val() + " " + $("#addEventStartTime option:selected").text());

    $("select#addEventEndTime option").each(function () {
        this.selected = (this.text === endTime)
    });

    addEndDate = new Date($("#addEventEnd").val() + " " + endTime);

    if (addStartDate > addEndDate) {
        document.getElementById("lblTime").className = 'show';
    }
    else {
        document.getElementById("lblTime").className = 'hidden';
    }
}

function setAddEventEndTime() {
    addEndDate = new Date($("#addEventEnd").val() + " " + $("#addEventEndTime option:selected").text());
    if (addStartDate > addEndDate) {
        document.getElementById("lblTime").className = 'show';
    }
    else {
        document.getElementById("lblTime").className = 'hidden';
    }
}

Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}

function setEventStartTime() {
    updateStartDate = new Date($("#eventStart").val() + " " + $("#eventStartTime option:selected").text());
    //console.log("StartDate: " + updateStartDate + ", EndDate: " + updateEndDate);
    var nStartDate = updateStartDate;

    var endTime = timeformatting(nStartDate.addHours(1)).replace(/^0/, ""); //"8:00 AM";//
    updateStartDate = new Date($("#eventStart").val() + " " + $("#eventStartTime option:selected").text());

    //console.log($("#eventStartTime option:selected").text());

    $("select#eventEndTime option").each(function () {
        this.selected = (this.text === endTime)
    });

    updateEndDate = new Date($("#eventEnd").val() + " " + endTime);

    if (updateStartDate > updateEndDate) {
        //console.log(updateStartDate);
        document.getElementById('lblRequired').innerHTML = 'End time must be later than start time';
        document.getElementById("lblRequired").className = 'show';
    }
    else {
        document.getElementById("lblRequired").className = 'hidden';
    }
    //console.log("StartDate: " + updateStartDate + ", EndDate: " + updateEndDate);
}

function setEventEndTime() {

    updateEndDate = new Date($("#eventEnd").val() + " " + $("#eventEndTime option:selected").text());
    console.log("StartDate: " + updateStartDate + ", EndDate: " + updateEndDate);
    if (updateStartDate > updateEndDate) {
        //console.log(updateStartDate);
        document.getElementById('lblRequired').innerHTML = 'End time must be later than start time';
        document.getElementById("lblRequired").className = 'show';
    }
    else {
        document.getElementById("lblRequired").className = 'hidden';
    }
}

function hideimagefunction() {
    $('#loading').hide();
}

function updateEventOnDropResize(event, allDay) {
    //console.log("id: " + event.id + ", CustomerID: " + event.CustomerID + ", EstimateID: " + event.EstimateID);
    //console.log(event.TypeID);

    // debugger;

    var start = new Date(moment(event.start).format());
    var end = new Date(moment(event.end).format());

    //console.log("start: " + start, "end: " + end);

    var nCustId = 0;
    var nEstId = 0;

    if (event.CustomerID === undefined || event.CustomerID === null) {
        nCustId = 0;
    }
    else {
        nCustId = event.CustomerID;
    }

    if (event.EstimateID === undefined || event.EstimateID === null) {
        nEstId = 0;
    }
    else {
        nEstId = event.EstimateID;
    }

    //console.log("nCustId : " + nCustId + ", nEstId : " + nEstId);
    //console.log("allday: " + allDay);
    var eventToUpdate = {
        id: event.id,
        start: event.start,
        customer_id: nCustId,
        estimate_id: nEstId
    };

    var upStart = new Date(moment(eventToUpdate.start).format());
    var upEnd = new Date(moment(eventToUpdate.end).format());

    nevent = event;
    if (event.allDay) {
        upStart.setHours(0, 0, 0);
    }
    // debugger;
    if (event.end === null) {
        upEnd = upStart;
    }
    else {
        upEnd = end;
        if (event.allDay) {
            upEnd.setHours(0, 0, 0);
        }
    }
    eventToUpdate.start = upStart.format("dd-MM-yyyy hh:mm:ss tt");
    eventToUpdate.end = upEnd.format("dd-MM-yyyy hh:mm:ss tt");

    console.log("upStart: " + upStart, "end: " + upEnd);

    if (eventToUpdate.estimate_id != "" && eventToUpdate.estimate_id != "0" && event.TypeID != 2) {
        //console.log("msg 2");
        $(function () {
            // $('#dialog-confirm').dialog('open');
            nUpdateEventTime(nevent);
            //$("#dialog-confirm").dialog({
            //    resizable: false,
            //    height: 150,
            //    modal: true,
            //    buttons: {
            //        Cancel: function () {
            //            nCancle(nevent);
            //            $(this).dialog("close");
            //        },
            //        No: function () {
            //            nUpdateEventTime(nevent);
            //            $(this).dialog("close");
            //        },
            //        Yes: function () {
            //            nUpdateEventTimeAll(nevent);
            //            $(this).dialog("close");
            //        }
            //    },
            //    close: function () {
            //        //nCancle();
            //    },
            //    dialogClass: 'no-close'
            //});
        });

        function nCancle(event) {
            $('#calendar').fullCalendar('refetchEvents');
        }
        function nUpdateEventTime(event) {


            var nStart = new Date(moment(event.start).format());
            var nEnd = new Date(moment(event.end).format());

            var eventToUpdate = {
                id: event.id,
                start: nStart,
                customer_id: event.CustomerID,
                estimate_id: event.EstimateID
            };

            var nUpStart = new Date(moment(eventToUpdate.start).format());
            var nUpEnd = new Date(moment(eventToUpdate.end).format());

            var nevent = eventToUpdate;
            if (event.allDay) {
                nUpStart.setHours(0, 0, 0);

            }

            if (nEnd === null) {
                nUpEnd = nUpStart;
            }
            else {
                nUpEnd = nEnd;

                if (event.allDay) {
                    nUpEnd.setHours(0, 0, 0);
                }
            }
            eventToUpdate.start = nUpStart.format("dd-MM-yyyy hh:mm:ss tt");
            eventToUpdate.end = nUpEnd.format("dd-MM-yyyy hh:mm:ss tt");

            $('#loading').show();

            console.log("nUpdateEventTime" + eventToUpdate.start, eventToUpdate.end);

            PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);
        }
        function nUpdateEventTimeAll(event) {
            var eventToUpdate = {
                id: event.id,
                start: event.start,
                customer_id: event.CustomerID,
                estimate_id: event.EstimateID
            };
            var nevent = eventToUpdate;
            if (allDay) {
                eventToUpdate.start.setHours(0, 0, 0);

            }

            if (event.end === null) {
                eventToUpdate.end = eventToUpdate.start;

            }
            else {
                eventToUpdate.end = event.end;
                if (allDay) {
                    eventToUpdate.end.setHours(0, 0, 0);
                }
            }
            eventToUpdate.start = eventToUpdate.start.format("dd-MM-yyyy hh:mm:ss tt");
            eventToUpdate.end = eventToUpdate.end.format("dd-MM-yyyy hh:mm:ss tt");
            $('#loading').show();
            PageMethods.UpdateEventTimeAll(eventToUpdate, UpdateTimeSuccessAll);
        }
    }
    else {
        //console.log("id: " + eventToUpdate.id + ", CustomerID: " + eventToUpdate.customer_id + ", EstimateID: " + eventToUpdate.estimate_id);

        console.log("else, start: " + eventToUpdate.start, "end: " + eventToUpdate.end);

        $('#loading').show();

        console.log("else nUpdateEventTime" + eventToUpdate.start, eventToUpdate.end);

        PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);
    }

    //console.log("Updated");
}

function updateEventOnResize(event, allDay) {
    //console.log("id: "+event.id+", CustomerID: " + event.CustomerID + ", EstimateID: "+event.EstimateID);
    //console.log("allday: " + allDay);

    // debugger;

    var eventToUpdate = {
        id: event.id,
        start: event.start,
        customer_id: event.CustomerID,
        estimate_id: event.EstimateID
    };

    var start = new Date(moment(event.start).format());
    var end = new Date(moment(event.end).format());

    var upStart = new Date(moment(eventToUpdate.start).format());
    var upEnd = new Date(moment(eventToUpdate.end).format());

    nevent = event;
    if (event.allDay) {
        upStart.setHours(0, 0, 0);
    }

    if (event.end === null) {
        upEnd = upStart;
    }
    else {
        upEnd = end;
        if (allDay) {
            upStart.setHours(0, 0, 0);
        }
    }
    eventToUpdate.start = upStart.format("dd-MM-yyyy hh:mm:ss tt");
    eventToUpdate.end = upEnd.format("dd-MM-yyyy hh:mm:ss tt");

    if (eventToUpdate.estimate_id != "" && eventToUpdate.estimate_id != "0") {

        var eventToUpdate = {
            id: event.id,
            start: event.start,
            customer_id: event.CustomerID,
            estimate_id: event.EstimateID
        };
        var nevent = eventToUpdate;
        if (allDay) {
            eventToUpdate.start.setHours(0, 0, 0);

        }

        if (event.end === null) {
            eventToUpdate.end = eventToUpdate.start;

        }
        else {
            eventToUpdate.end = event.end;
            if (allDay) {
                eventToUpdate.end.setHours(0, 0, 0);
            }
        }
        eventToUpdate.start = eventToUpdate.start.format("dd-MM-yyyy hh:mm:ss tt");
        eventToUpdate.end = eventToUpdate.end.format("dd-MM-yyyy hh:mm:ss tt");
        $('#loading').show();

        console.log("updateEventOnResize" + eventToUpdate.start, eventToUpdate.end);

        PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);


    }
    else {
        $('#loading').show();

        console.log("else, updateEventOnResize" + eventToUpdate.start, eventToUpdate.end);

        PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);
    }

    //console.log("Updated");
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
    //console.log("woke up!");
}

function eventDropped(event, dayDelta, minuteDelta, allDay, revertFunc) {
    console.log(event);
    if (!isSelectable) {
        return false;
    }
    // debugger;
    //console.log(allDay);
    if ($(this).data("qtip")) $(this).qtip("destroy");
    //console.log(event.start);
    updateEventOnDropResize(event, allDay);
}

function eventResized(event, dayDelta, minuteDelta, revertFunc) {

    if (!isSelectable) {
        return false;
    }
    if ($(this).data("qtip")) $(this).qtip("destroy");

    //updateEventOnDropResize(event);
    updateEventOnResize(event);
}

function checkForSpecialChars(stringToCheck) {
    //var pattern = /[^A-Za-z0-9 ]/;
    // var pattern = /[^(A-Za-z0-9 ?<=^| )\d+(\.\d+)?(?=$| )|(?<=^| )\.\d+(?=$|.) ]/;
    var pattern = /[^A-Za-z0-9_\-.?'" ]/;

    return pattern.test(stringToCheck);
}

function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    //console.log(txt.value);
    return txt.value;
}

function txtChange(id) {
    if (document.getElementById(id).value == '') {
        document.getElementById("lbl" + id).className = 'show';
    }
    else {
        document.getElementById("lbl" + id).className = 'hidden';
    }
}

function searchKeyPress(e) {

    // look for window.event in case event isn't passed in
    e = e || window.event;
    if (e.keyCode == 13) {
        document.getElementById("head_btnSearch").click();
        return false;
    }
    return true;
}

function ddldependencyType_Onchange() {
    console.log($("#ddldependencyType option:selected").val());

    if ($("#ddldependencyType option:selected").val() === "1") {
        $("#txtOffsetdays").val(0);
        $("#txtOffsetdays").hide();
    }
    else if ($("#ddldependencyType option:selected").val() === "2") {
        $("#txtOffsetdays").val(1);
        $("#txtOffsetdays").hide();
    }
    else if ($("#ddldependencyType option:selected").val() === "3") {
        $("#txtOffsetdays").val();
        $("#txtOffsetdays").show();
    }
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function dependencyTypeChange(objType, objStart, objEnd, objOffset) {
    //  debugger;
    //Parent Event Start & End DateTime
    var vParentEventStart = new Date(document.getElementById("eventStart").value);
    var dParentEventStart = vParentEventStart.getDay();

    var vParentEventEnd = new Date(document.getElementById("eventEnd").value);
    var dParentEventEnd = vParentEventEnd.getDay();

    //Gridview Element ID's
    var typeId = objType.id;
    var startId = objStart.id;
    var endId = objEnd.id;
    var offsetId = objOffset.id;

    //Drop Down
    var eTypeId = document.getElementById(typeId);
    var vType = eTypeId.options[eTypeId.selectedIndex].value;

    //Start Label
    var eStartId = document.getElementById(startId);
    var vStartText = eStartId.innerText;

    var dtStart = new Date(vStartText);

    var dStartDate = dtStart.getDay();
    var tStartTime = timeformatting(vStartText);

    //End Label
    var eEndId = document.getElementById(endId);
    var vEndText = eEndId.innerText;

    var dtEnd = new Date(vEndText);

    var dEndDate = dtEnd.getDay();
    var tEndTime = timeformatting(vEndText);

    //Date Difference
    var nDays = (dEndDate - dStartDate);

    //Offset Days

    var nOffsetDays = document.getElementById(offsetId).value;

    if (vType === "1")//Start Same Time
    {
        var nStart = dateformatting(vParentEventStart);
        var nEnd = dateformatting(vParentEventEnd);

        //Set Datetime
        eStartId.innerText = nStart + " " + tStartTime;
        eEndId.innerText = nEnd + " " + tEndTime;
        $('#' + offsetId + '').css("display", "none");//.hide();
    }
    else if (vType === "2")//Start After Finish
    {
        var nStart = dateformatting(vParentEventEnd.addDays(1));
        var nEnd = dateformatting(vParentEventEnd.addDays(nDays + 1));

        //Set Datetime
        eStartId.innerText = nStart + " " + tStartTime;
        eEndId.innerText = nEnd + " " + tEndTime;
        $('#' + offsetId + '').css("display", "none");//.hide();
    }
    else if (vType === "3")//Offset days
    {
        //var nStart = dateformatting(vParentEventEnd.addDays(nOffsetDays + 1));
        //var nEnd = dateformatting(vParentEventEnd.addDays(nDays + 1));

        //eStartId.innerText = nStart + " " + tStartTime;
        //eEndId.innerText = nEnd + " " + tEndTime;

        $('#' + offsetId + '').show();
        //Set Datetime
        //eStartId.innerText = "";
        //eEndId.innerText = "";
    }

    console.log(vParentEventStart);
    //console.log(eStartId);
    //console.log(vType + ", " + vStart + ", " + vEnd);
}

function txtOffsetChange(objType, objStart, objEnd, objOffset) {
    //  debugger;
    //Parent Event Start & End DateTime
    var vParentEventStart = new Date(document.getElementById("eventStart").value);
    var dParentEventStart = vParentEventStart.getDay();

    var vParentEventEnd = new Date(document.getElementById("eventEnd").value);
    var dParentEventEnd = vParentEventEnd.getDay();

    //Gridview Element ID's
    var typeId = objType.id;
    var startId = objStart.id;
    var endId = objEnd.id;
    var offsetId = objOffset.id;

    //Drop Down
    var eTypeId = document.getElementById(typeId);
    var vType = eTypeId.options[eTypeId.selectedIndex].value;

    //Start Label
    var eStartId = document.getElementById(startId);
    var vStartText = eStartId.innerText;

    var dtStart = new Date(vStartText);

    var dStartDate = dtStart.getDay();
    var tStartTime = timeformatting(vStartText);

    //End Label
    var eEndId = document.getElementById(endId);
    var vEndText = eEndId.innerText;

    var dtEnd = new Date(vEndText);

    var dEndDate = dtEnd.getDay();
    var tEndTime = timeformatting(vEndText);

    //Date Difference
    var nDays = (dEndDate - dStartDate);

    //Offset Days

    var nOffsetDays = parseInt(document.getElementById(offsetId).value);
    console.log("nOffsetDays: " + nOffsetDays);

    if (vType === "3")//Offset days
    {
        var nStart = dateformatting(vParentEventEnd.addDays(nOffsetDays + 1));
        var nEnd = dateformatting(vParentEventEnd.addDays(nDays + 1 + nOffsetDays));

        eStartId.innerText = nStart + " " + tStartTime;
        eEndId.innerText = nEnd + " " + tEndTime;

        $('#' + offsetId + '').css("display", "block");//.show();
        //Set Datetime
        //eStartId.innerText = "";
        //eEndId.innerText = "";
    }

    // console.log(vParentEventStart);
    //console.log(eStartId);
    //console.log(vType + ", " + vStart + ", " + vEnd);
}

function CalStateAction() {
    var isCalOnline = (document.getElementById("head_hdnCalStateAction").value === 'false');

    console.log(isCalOnline);

    var calStatus = "Go Online";

    if (!isCalOnline)
        calStatus = "Go Offline";

    return confirm('Are You sure want to ' + calStatus + '?');
}

$(document).ready(function () {
    //if ($("#head_hdnEventLinkCount").val() == "0") {
    //    $('#btnUpdateLink').css("display", "none");
    //    $('#btnDeleteLink').css("display", "none");
    //}
    //else {
    //    $('#btnUpdateLink').css("display", "block");
    //    $('#btnDeleteLink').css("display", "block");
    //}
    //console.log($("#head_hdnUpdateDialogShow").val());
    //if ($("#head_hdnUpdateDialogShow").val() == "true") {
    //    debugger;
    //    $("#updatedialog").dialog({ draggable: false });
    //}

    //$('#demo').btnSwitch({
    //    // iOS like button
    //    Theme: 'iOS',

    //    // on/off text
    //    OnText: "On",
    //    OffText: "Off",

    //    // values of on/off buttons
    //    OnValue: true,
    //    OffValue: false,

    //    // callbacks
    //    OnCallback: function () { SwitchOnFunction(); },
    //    OffCallback: function () { SwitchOffFunction(); },

    //    // hidden input's ID
    //    HiddenInputId: "hdnValue"
    //});

    //function SwitchOnFunction() {
    //   // $("#result").text('Switch is on! Value of switch is ' + $("#hdnValue").val());
    //    if (confirm("Are You Sure?")) {
    //        $("#head_btnCalStateAction").click();
    //    }
    //}
    //function SwitchOffFunction() {
    //   // $("#result").text('Switch is off! Value of switch is ' + $("#hdnValue").val());
    //    if (confirm("Are You Sure?")) {
    //        $("#head_btnCalStateAction").click();
    //    }
    //}

    $('#addEventStartDate').datepicker({
        //showButtonPanel: true,       
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        showOn: "both",
        buttonImage: "images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Select date",
        // showButtonPanel: true,
        dateFormat: 'm/dd/yy'

    });
    $('#addEventEndDate').datepicker({
        //showButtonPanel: true,      
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        showOn: "both",
        buttonImage: "images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Select date",
        // showButtonPanel: true,
        dateFormat: 'm/dd/yy'

    });

    $('#eventStart').datepicker({
        //showButtonPanel: true,       
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        showOn: "both",
        buttonImage: "images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Select date",
        // showButtonPanel: true,
        dateFormat: 'm/dd/yy'

    });
    $('#eventEnd').datepicker({
        //showButtonPanel: true,       
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        showOn: "both",
        buttonImage: "images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Select date",
        // showButtonPanel: true,
        dateFormat: 'm/dd/yy'

    });

    $('#miniCalendar').datepicker({
        //showButtonPanel: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'DD, d MM, yy',
        onSelect: function (dateText, dp) {
            $('#calendar').fullCalendar('gotoDate', new Date(Date.parse(dateText)));
            $('#calendar').fullCalendar('changeView', 'agendaDay');
        }
    });



    document.getElementById("addEventName").value = document.getElementById("head_hdnAddEventName").value;
    document.getElementById("addEventDesc").value = document.getElementById("head_hdnEventDesc").value;

    //$("#head_txtSearch").attr("placeholder", "Search by Last Name");
    //$("#head_txtSection").attr("placeholder", "Section");
    //var j = jQuery.noConflict();


    var nCusomertId = 0;
    var nEstimateId = 0;

    if (document.getElementById("head_hdnCustIDSelected").value != '')
        nCusomertId = parseInt(document.getElementById("head_hdnCustIDSelected").value);

    if (document.getElementById("head_hdnEstIDSelected").value != '')
        nEstimateId = parseInt(document.getElementById("head_hdnEstIDSelected").value);




    $("#addEventName").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSectionByCustomerId",
                data: "{'keyword':'" + $("#addEventName").val() + "', 'nCustId':'" + nCusomertId + "', 'nEstId':'" + nEstimateId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // console.log("addEventName combobox:" + data.d);
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.section_name,
                            desc: item.section_name,
                            value: item.section_name
                        }
                    }));
                },
                error: function (e) {
                    //console.log("there is some error");
                    console.log(e);
                }
            });
        }//,
        //select: function (event, ui) {
        //    ParentEventName = ui.item.desc;
        //},
        //change: function (event, ui) {
        //    ParentEventName = ui.item.desc;
        //},
    });



    $("#addSalesPersonName").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSalesPerson",
                data: "{'keyword':'" + $("#addSalesPersonName").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // console.log("addEventName combobox:" + data.d);
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.sales_person_name,
                            desc: item.sales_person_id,
                            value: item.sales_person_name
                        }
                    }));
                },
                error: function (e) {
                    //console.log("there is some error");
                    console.log(e);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            SalesPersonID = ui.item.desc;
        },
        change: function (event, ui) {
            SalesPersonID = ui.item.desc;
        },
        messages: {
            noResults: "",
            results: function () { }
        },
        search: function () { $(this).addClass('progress'); },
        open: function () { $(this).removeClass('progress'); },
        response: function () { $(this).removeClass('progress'); }
    });

    $("#eventName").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSectionByCustomerId",
                data: "{'keyword':'" + $("#eventName").val() + "', 'nCustId':'" + nCusomertId + "', 'nEstId':'" + nEstimateId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // console.log("addEventName combobox:" + data.d);
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.section_name,
                            desc: item.section_name,
                            value: item.section_name
                        }
                    }));
                },
                error: function (e) {
                    //console.log("there is some error");
                    console.log(e);
                }
            });
        }
    });

    $("#eventSalesPerson").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSalesPerson",
                data: "{'keyword':'" + $("#eventSalesPerson").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // console.log("addEventName combobox:" + data.d);
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.sales_person_name,
                            desc: item.sales_person_id,
                            value: item.sales_person_name
                        }
                    }));
                },
                error: function (e) {
                    //console.log("there is some error");
                    console.log(e);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            SalesPersonID = ui.item.desc;
        },
        change: function (event, ui) {
            SalesPersonID = ui.item.desc;
        },
        messages: {
            noResults: "",
            results: function () { }
        },
        search: function () { $(this).addClass('progress'); },
        open: function () { $(this).removeClass('progress'); },
        response: function () { $(this).removeClass('progress'); }

    });

    $("#linkToSubsequent").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSubsequentSection",
                data: "{'keyword':'" + $("#linkToSubsequent").val() + "', 'nCustId':'" + nCusomertId + "', 'nEstId':'" + nEstimateId + "', 'ParentEventName':'" + ParentEventName + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // console.log("addEventName combobox:" + data.d);

                    if (data.d != null || data.d != undefined) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.section_name,
                                desc: item.event_id,
                                value: item.section_name
                            }
                        }));
                    }
                },
                error: function (e) {
                    //console.log("there is some error");
                    console.log(e);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            if (ui.item != null)
                ChildEventID = ui.item.desc;
        },
        change: function (event, ui) {
            if (ui.item != null)
                ChildEventID = ui.item.desc;
        },
        messages: {
            noResults: "",
            results: function () { }
        },
        search: function () { $(this).addClass('progress'); },
        open: function () { $(this).removeClass('progress'); },
        response: function () { $(this).removeClass('progress'); }

    });


    $("#head_txtSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetCustomer",
                data: "{'keyword':'" + $("#head_txtSearch").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.customer_name,
                            desc: item.customer_id,
                            value: item.customer_name
                        }
                    }));
                },
                error: function () {
                    //console.log("there is some error");
                    console.log("there is some error");
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            AutoComCustID = ui.item.desc;
        },
        change: function (event, ui) {
            AutoComCustID = ui.item.desc;
        },
        messages: {
            noResults: "",
            results: function () { }
        },
        search: function () { $(this).addClass('progress'); },
        open: function () { $(this).removeClass('progress'); },
        response: function () { $(this).removeClass('progress'); }
    });

    $("#head_txtSection").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Copyschedulecalendar.aspx/GetSection",
                data: "{'keyword':'" + $("#head_txtSection").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;
                    response($.map(data.d, function (item) {
                        return {
                            label: item.section_name,
                            desc: item.section_name,
                            value: item.section_name
                        }
                    }));
                },
                error: function () {
                    //console.log("there is some error");
                    console.log("there is some error");
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            AutoComSection = ui.item.desc;
        },
        change: function (event, ui) {
            AutoComSection = ui.item.desc;
        },
        messages: {
            noResults: "",
            results: function () { }
        },
        search: function () { $(this).addClass('progress'); },
        open: function () { $(this).removeClass('progress'); },
        response: function () { $(this).removeClass('progress'); }
    });



    $("#head_btnSearch").click(function () {
        var nCustId = 0;
        var SectionName = '';

        if ($("#head_txtSearch").val() != '')
            nCustId = AutoComCustID;
        if ($("#head_txtSection").val() != '')
            SectionName = AutoComSection;

        //console.log(AutoComCustID);
        PageMethods.GetEvent(nCustId, SectionName, getSuccess);

    });

    $("#head_lnkViewAll").click(function () {
        $("#head_txtSearch").val('');
        $("#head_txtSection").val('');
        var nCustId = 0;
        var SectionName = '';
        PageMethods.GetEvent(nCustId, SectionName, getSuccess);
        $('.fc-button-today span').click();
    });

    // update Dialog
    //  j('#updatedialog').dialog('open');
    $('#updatedialog').dialog({
        autoOpen: false,
        width: 612,
        dialogClass: 'cssdialog',
        buttons: {
            "Close": function () {
                document.getElementById("head_hdnAddEventName").value = '';
                document.getElementById("head_hdnEventDesc").value = '';
                document.getElementById("addEventName").value = '';
                document.getElementById("addEventDesc").value = '';
                document.getElementById("lbleventName").className = 'hidden';
                document.getElementById("lbleventDesc").className = 'hidden';
                document.getElementById("lblRequired").className = 'hidden';
                document.getElementById("lblNotes").className = 'hidden';
                document.getElementById("lblTradePartner").className = 'hidden';

                $('#calendar').fullCalendar('updateEvent', '');
                $(this).dialog("close");
            },
            "Update": function () {

                document.getElementById("head_hdnAddEventName").value = '';
                document.getElementById("head_hdnEventDesc").value = '';
                if (document.getElementById("eventName").value == '') {
                    document.getElementById("lbleventName").className = 'show';
                }
                    //else if (document.getElementById("eventDesc").value == '') {
                    //    document.getElementById("lbleventDesc").className = 'show';
                    //}
                else if (updateStartDate > updateEndDate) {
                    document.getElementById("lblRequired").className = 'show';
                }
                else {
                    console.log("updatedialog, eventSalesPerson: " + $("#eventSalesPerson").val());
                    var eventToUpdate = {
                        id: currentUpdateEvent.id,
                        title: $("#eventName").val().replace("/", "\/"),
                        description: $("#eventDesc").val().replace("/", "\/").replace(/(\r\n|\n|\r)/gm, " "),
                        start: updateStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        end: updateEndDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        employee_id: SalesPersonID,
                        employee_name: $("#eventSalesPerson").val(),
                        child_event_id: ChildEventID,
                        dependencyType: $("#ddldependencyType option:selected").val(),
                        offsetDays: $("#txtOffsetdays").val()
                    };
                    $('#loading').show();
                    PageMethods.UpdateEvent(eventToUpdate, updateSuccess);
                    document.getElementById("lblRequired").className = 'hidden';
                    //  $(this).dialog("close");

                    currentUpdateEvent.title = $("#eventName").val();
                    currentUpdateEvent.description = $("#eventDesc").val();
                    currentUpdateEvent.start = updateStartDate;
                    currentUpdateEvent.end = updateEndDate;
                    currentUpdateEvent.employee_name = $("#eventSalesPerson").val()


                    $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);
                    //console.log("Test");


                }
            },
            "Delete": function () {
                if (confirm("do you really want to delete this event?")) {
                    $('#loading').show();
                    PageMethods.deleteEvent($("#eventId").val(), deleteSuccess);
                    document.getElementById("head_hdnAddEventName").value = '';
                    document.getElementById("head_hdnEventDesc").value = '';
                    $(this).dialog("close");

                    $('#calendar').fullCalendar('removeEvents', $("#eventId").val());
                }
            },
            "Save": function () {
                if (document.getElementById("txtTradePartner").value == '') {
                    document.getElementById('lblTradePartner').innerHTML = 'Required';
                    document.getElementById("lblTradePartner").className = 'show';
                    document.getElementById('lblRequired').innerHTML = '';

                }
                else {
                    var eventToUpdate = {
                        id: currentUpdateEvent.id,
                        start: updateStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        end: updateEndDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        trade_partner: $("#txtTradePartner").val().replace("/", "\/").replace(/(\r\n|\n|\r)/gm, " "),
                        operation_notes: $("#txtNotes").val().replace("/", "\/").replace(/(\r\n|\n|\r)/gm, " ")
                    };

                    //console.log(updateStartDate.format("dd-MM-yyyy hh:mm:ss tt"));

                    if (updateStartDate > updateEndDate) {
                        //console.log(updateStartDate);
                        document.getElementById('lblRequired').innerHTML = 'End time must be later than start time';
                        document.getElementById("lblRequired").className = 'show';
                        document.getElementById('lblTradePartner').innerHTML = '';
                    }
                    else {

                        PageMethods.UpdateTradePartner(eventToUpdate, updateSuccessTradePartner);
                        //console.log(currentUpdateEvent.title);
                        $("#eventName").val(currentUpdateEvent.temptitle + ' - ' + $("#txtTradePartner").val());
                        currentUpdateEvent.operation_notes = $("#txtNotes").val();
                        currentUpdateEvent.title = currentUpdateEvent.temptitle + ' - ' + $("#txtTradePartner").val(),
                        currentUpdateEvent.trade_partner = $("#txtTradePartner").val();
                        currentUpdateEvent.start = updateStartDate;
                        currentUpdateEvent.end = updateEndDate;

                        document.getElementById('lblTradePartner').innerHTML = '';

                        document.getElementById('lblRequired').innerHTML = 'Saved Successfully';
                        document.getElementById("lblRequired").className = 'showsuccess';
                        $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);
                    }


                }
            },

            "Go to Schedule": function () {
                if (currentUpdateEvent.EstimateID != "0" || currentUpdateEvent.EstimateID == "") {
                    $(this).dialog("close");
                    window.location = "cust_schedule.aspx?cid=" + currentUpdateEvent.CustomerID + "&eid=" + currentUpdateEvent.EstimateID;
                }
            },
            "Go to Activity log": function () {
                //debugger;
                $('#loading').show();
                //console.log(currentUpdateEvent.TypeID);
                if (currentUpdateEvent.TypeID == "2") {
                    $(this).dialog("close");
                    window.location = "customer_details.aspx?cid=" + currentUpdateEvent.CustomerID + "&callid=" + currentUpdateEvent.EstimateID;
                }
            }
        }
    });
    $("#updatedialog").bind("dialogopen", function (event, ui) {
        console.log(currentUpdateEvent.TypeID);
        if (currentUpdateEvent.TypeID == "1") {
            $('button:contains(Update)').show();
            $('button:contains(Go to Schedule)').hide();
            $('button:contains(Save Notes)').hide();
            $('button:contains(Save)').hide();
            $('button:contains(Go to Activity log)').hide();
            $('button:contains(Delete)').show();
            $('#trNotes').hide();
            $('#trTradePartner').hide();
            $("#txtOffsetdays").val(0);
            $("#txtOffsetdays").hide();
            // $('#eventName').trigger('blur');
            // $("#eventName").attr("class", "txtReadOnly");
            //  $("#eventDesc").attr("class", "txtReadOnly");
            //   $("#eventName").attr("disabled", "disabled");
            //  $("#eventDesc").attr("disabled", "disabled");
        }
        else if (currentUpdateEvent.TypeID == "2") {
            $('button:contains(Update)').show();
            $('button:contains(Go to Schedule)').hide();
            $('button:contains(Save Notes)').hide();
            $('button:contains(Save)').hide();
            $('button:contains(Go to Activity log)').show();
            //  $("#eventName").attr("class", "txtReadOnly");
            $("#eventName").attr("class", "scTextArea");
            $("#eventDesc").attr("class", "scTextArea");
            //  $("#eventName").attr("disabled", "disabled");
            $("#eventName").removeAttr("disabled");
            $("#eventDesc").removeAttr("disabled");
            $('button:contains(Delete)').show();
            $('#trNotes').hide();
            $('#trTradePartner').hide();
        }
        else {
            $('button:contains(Update)').show();
            $('button:contains(Go to Schedule)').hide();
            $('button:contains(Save Notes)').hide();
            $('button:contains(Save)').hide();
            $('button:contains(Go to Activity log)').hide();
            $("#eventName").attr("class", "scTextArea");
            $("#eventDesc").attr("class", "scTextArea");
            $("#eventName").removeAttr("disabled");
            $("#eventDesc").removeAttr("disabled");
            $('button:contains(Delete)').show();
            $('#trNotes').hide();
            $('#trTradePartner').hide();
            $("#txtOffsetdays").val(0);
            $("#txtOffsetdays").hide();

        }
    });

    //$("#updatedialog").bind("dialogbeforeclose", function (event, ui) {
    //    $("#head_hdnUpdateDialogShow").val("false");
    //});

    //add dialog
    $('#addDialog').dialog({
        autoOpen: false,
        width: 470,
        dialogClass: 'cssdialog',
        buttons: {
            "Close": function () {
                document.getElementById("addEventDesc").value = '';
                if (document.getElementById("head_hdnEventDesc").value != '')
                    document.getElementById("addEventDesc").value = document.getElementById("head_hdnEventDesc").value

                document.getElementById("lbladdEventName").className = 'hidden';
                document.getElementById("lbladdEventDesc").className = 'hidden';
                document.getElementById("lblTime").className = 'hidden';

                $("select#addEventStartTime option")
                .each(function () { this.selected = (this.text == "6:00 AM"); });

                $("select#addEventEndTime option")
                .each(function () { this.selected = (this.text == "7:00 AM"); });

                $(this).dialog("close");
            },
            "Add": function () {
                if (document.getElementById("addEventName").value == '') {
                    document.getElementById("lbladdEventName").className = 'show';
                }
                    //else if (document.getElementById("addEventDesc").value == '') {
                    //    document.getElementById("lbladdEventDesc").className = 'show';
                    //}
                else if (addStartDate > addEndDate) {
                    document.getElementById("lblTime").className = 'show';
                }
                else {
                    console.log("addSalesPersonName: " + $("#addSalesPersonName").text() + ", " + $("#addSalesPersonName").val());
                    $('#loading').show();
                    document.getElementById("lbladdEventName").className = 'hidden';
                    document.getElementById("lbladdEventDesc").className = 'hidden';
                    document.getElementById("lblTime").className = 'hidden';
                    var eventToAdd = {
                        title: $("#addEventName").val(),
                        description: $("#addEventDesc").val().replace(/(\r\n|\n|\r)/gm, " "),
                        start: addStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        end: addEndDate.format("dd-MM-yyyy hh:mm:ss tt"),
                        allDay: false,
                        employee_id: SalesPersonID,
                        employee_name: $("#addSalesPersonName").val()
                    };

                    PageMethods.addEvent(eventToAdd, addSuccess);
                    $(this).dialog("close");
                }
            },
            "Cancel Schedule": function () {
                if (confirm("do you really want to cancel this event?")) {
                    PageMethods.cancelEvent(cancelSuccess);

                    document.getElementById("addEventName").value = '';
                    document.getElementById("addEventDesc").value = '';
                    document.getElementById("head_hdnAddEventName").value = '';
                    document.getElementById("head_hdnEventDesc").value = '';

                    $(this).dialog("close");
                    window.location = "customer_details.aspx?cid=" + document.getElementById("head_hdnCustomerID").value;
                    //$('#calendar').fullCalendar('removeEvents', $("#eventId").val());
                }
            }
        }
    });

    $("#addDialog").bind("dialogopen", function (event, ui) {
        //debugger;
        if (document.getElementById("head_hdnEstimateID").value == "0") {
            $('button:contains(Cancel Schedule)').hide();
        }
        else {
            $('button:contains(Cancel Schedule)').show();
        }
    });

    $('#external-events .fc-event').each(function () {

        // store data so the calendar knows to render an event upon drop
        $(this).data('event', {
            title: $.trim($(this).text()), // use the element's text as the event title
            stick: true // maintain when user navigates (see docs on the renderEvent method)
        });

        // make the event draggable using jQuery UI
        $(this).draggable({
            helper: 'clone',
            zIndex: 9999999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0,  //  original position after the drag
            drag: function (event, ui) {
                if (isSelectable) {

                    return true;
                }
                else {
                    alert("Calendar is Online");
                    return false;
                }
            }
        });

    });

    var date = new Date();

    if (document.getElementById("head_hdnEventStartDate").value != '') {
        date = new Date(document.getElementById("head_hdnEventStartDate").value);
    }
    isSelectable = (document.getElementById("head_hdnCalStateAction").value === 'false');
    //  console.log("head_hdnCalStateAction: " + document.getElementById("head_hdnCalStateAction").value);
    // console.log("isSelectable: " + isSelectable);

    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    var tt = date.getTime();
    var firstHour = date.getHours();
    // console.log(d);
    var calendar = $('#calendar').fullCalendar({
        //        dayClick: function (date, jsEvent, view) {
        //           console.log(date.format('DD-MM-YYYY'));
        //        },
        loading: function (isLoading, view) {
            if (isLoading) {
                $('#calendar').fullCalendar('removeEvents');
                $('#loading').show();
            }
            else {
                //console.log("msg");
                $('#loading').hide();
            }
        },
        //  eventBackgroundColor: '#378006',
        //year: y,
        //month: m,
        //date: d,
        //scrollTime: tt,
        // firstHour: firstHour,

        theme: true,
        header: {
            left: '',
            center: 'prev, title, next, today',
            right: 'month,agendaWeek,agendaDay,listWeek'
        },
        //buttonText: {
        //    prevYear: parseInt(new Date().getFullYear(), 10) - 1,
        //    nextYear: parseInt(new Date().getFullYear(), 10) + 1
        //},
        viewDisplay: function (view) {
            var d = $('#calendar').fullCalendar('getDate');
            console.log("viewDisplay: " + d);
            $(".fc-button-prevYear .fc-button-content").text(parseInt(d.getFullYear(), 10) - 1);
            $(".fc-button-nextYear .fc-button-content").text(parseInt(d.getFullYear(), 10) + 1);
        },

        ignoreTimezone: false,
        defaultView: 'month',//'agendaWeek',
        eventClick: updateEvent,
        selectable: isSelectable,
        selectHelper: true,
        select: selectDate,

        eventStartEditable: isSelectable,
        events: "CopyJsonResponse.ashx",



        eventDrop: eventDropped,

        eventResize: eventResized,
        eventRender: function (event, element) {
            var touchavailable = ("ontouchend" in document);
            if (touchavailable) {
                $(element).addtouch();
            }
            // console.log(event.title);
            element.qtip({
                content: event.title + "<br/>" + event.description,
                position: { corner: { tooltip: 'bottomRight', target: 'topRight' } },
                style: {
                    border: {
                        width: 1,
                        radius: 3,
                        color: '#2779AA'
                    },
                    padding: 10,
                    textAlign: 'left',
                    tip: true, // Give it a speech bubble tip with automatic corner detection
                    name: 'cream', // Style it according to the preset 'cream' style
                    width: 300
                }

            });
        },
        droppable: isSelectable, // this allows things to be dropped onto the calendar
        drop: function (date, jsEvent, ui, resourceId) {
            var title = jsEvent.target.innerText;
            var strDate = dateformatting(date);
            $(this).remove();
            console.log("Dropped on " + date + ", dateformatting:" + strDate + ", " + jsEvent.target.innerText);
            //console.log("===========================================================================");
            //console.log(jsEvent);
            //console.log("===========================================================================");
            //console.log(ui);
            //console.log("===========================================================================");
            //console.log(resourceId);
            //// is the "remove after drop" checkbox checked?
            //if ($('#drop-remove').is(':checked')) {
            //    // if so, remove the element from the "Draggable Events" list
            //    $(this).remove();
            //}

            addeventOnDrop(strDate, title);
            $('#calendar').fullCalendar('refetchEvents');
        }
    });
}
);
function AutoComplete(control) {

}

function AddEventLink() {


    var linkEvent = {
        parent_event_id: currentUpdateEvent.id,
        child_event_id: ChildEventID
    }

    console.log(currentUpdateEvent.id + " " + ChildEventID);

    $.ajax({
        type: "POST",
        url: "Copyschedulecalendar.aspx/AddEventLink",
        data: "{'parent_event_id':'" + currentUpdateEvent.id + "','child_event_id':'" + ChildEventID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data.d);
            var result = data.d.IsSuccess;
            console.log(data.d);
            if (result == "Ok") {
                var newRow = "<tr>" +
                                "<td align='left' style='width: 5%;'>" +
                                    "<input id='chkLink' type='checkbox' name=''>" +
                                "</td>" +
                                "<td style='width: 65%;'>" + data.d.title + "</td>" +
                                "<td style='width: 15%;'>" + data.d.start + "</td>" +
                                "<td style='width: 15%;'>" + data.d.end + "</td>" +
                                "<td class='hdnColumnCss'>" + data.d.parent_event_id + "</td>" +
                                "<td class='hdnColumnCss'>" + data.d.link_id + "</td>" +
                            "</tr>";
                $("#head_grdCalLinkInfo").append(newRow);
            }

        },
        error: function (e) {
            //console.log("there is some error");
            console.log(e);
        }
    });

    $("#head_grdCalLinkInfo").each(function () {

        var tds = '<tr>';
        jQuery.each($('tr:last td', this), function () {
            tds += '<td>' + $(this).html() + '</td>';
        });
        tds += '</tr>';
        if ($('tbody', this).length > 0) {
            $('tbody', this).append(tds);
        } else {
            $(this).append(tds);
        }
    });

}

function DeleteEventLink() {
    var strEventId = "";
    $("#head_grdCalLinkInfo tr:has(td)").each(function () {


        var isChecked = $(this).find('input[type="checkbox"]').prop("checked");
        //console.log(isChecked);

        if (isChecked) {
            var strId = $(this).find("td:eq(5)").text();
            strEventId += strId + ', ';
        }
        // console.log('strEventId: ' + strEventId);
    });



    if (strEventId.length > 0) {
        $.ajax({
            type: "POST",
            url: "Copyschedulecalendar.aspx/DeleteEventLink",
            data: "{'eventId':'" + strEventId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log("DeleteEventLink:" + data.d);
                var result = data.d;


                if (result == "Ok") {
                    $("#head_grdCalLinkInfo tr:has(td)").each(function () {
                        var isChecked = $(this).find('input[type="checkbox"]').prop("checked");
                        if (isChecked) {
                            $(this).css('display', 'none');
                        }
                    });
                }

            },
            error: function (e) {
                //console.log("there is some error");
                console.log(e);
            }
        });
    }
    else {
        alert("Select Link for Delete");
    }
}

function UpdateEventLink() {

    if (document.getElementById("head_hdnCustIDSelected").value != '')
        nCusomertId = parseInt(document.getElementById("head_hdnCustIDSelected").value);

    if (document.getElementById("head_hdnEstIDSelected").value != '')
        nEstimateId = parseInt(document.getElementById("head_hdnEstIDSelected").value);

    var scEventLinks = new Array();


    $("#head_grdCalLinkInfo tr:has(td)").each(function () {
        var scEventLink = {};
        var ddlType = $(this).find('.cssddldependencyType').prop("id");
        var offsetDays = $(this).find('.csstxtOffsetdays').prop("id");

        var eventStart = $(this).find('.csslblStart').prop("id");
        var eventEnd = $(this).find('.csslblEnd').prop("id");

        var ilink_id = $(this).find('.csslink_id');

        var vDependencyType = $("#" + ddlType + " option:selected").val();
        var vOffsetDays = $("#" + offsetDays + "").val();

        var vEventStart = $("#" + eventStart + "").text();
        var vEventEnd = $("#" + eventEnd + "").text();

        scEventLink.link_id = $(ilink_id).text(); //$(this).find("td:nth-child(5)").html();
        scEventLink.dependencyType = vDependencyType;
        scEventLink.event_start = vEventStart;//.trim().replace("/", "-").trim().replace("/", "-");
        scEventLink.event_end = vEventEnd;//.trim().replace("/", "-").trim().replace("/", "-");
        scEventLink.offsetdays = vOffsetDays;

        scEventLinks.push(scEventLink);
        console.log(scEventLinks);
    });

    $.ajax({
        type: 'POST',
        url: "Copyschedulecalendar.aspx/UpdateEventLink",
        data: "{'datascEventLinks':'" + JSON.stringify(scEventLinks) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data.d);
            var result = data.d;


            if (result == "Ok") {
                $('#loading').hide();

                //var theHtml = updateResult;
                //$("#head_grdCalLinkInfo").empty();
                //$("#head_grdCalLinkInfo").append(theHtml);

                $("#head_btnHdn").click();
            }

        },
        error: function (e) {
            //console.log("there is some error");
            console.log(e);
        }
    });
}

