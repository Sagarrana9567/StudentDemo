$(document).ready(function () {
    GetAllStates();
    GetStudentData();
})
function GetStudentData() {
    $("#tblStudent").DataTable().destroy();
    $("#tblStudent").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "destroy": true,
        "ordering": true,
        "ajax": {
            "url": '/Home/GetStudentData',
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "Name", "name": "Id", "autoWidth": true }
            , { "data": "Email", "Name": "Name", "autoWidth": true }
            , { "data": "PhoneNo", "name": "Quantity", "autoWidth": true }
            , {
                "data": "Id", "name": "Id", "autoWidth": true,
                "render": function (data, type, row) {
                    return '<center><a href="javascript:void(0)" class="clsEdit btn btn-primary" style="    margin-right: 10px;" onClick="EditStudent(' + row.Id + ')">Edit</a><a href="javascript:void(0)" class="btn btn-danger" onClick="DeleteStudent(' + row.Id + ')">Delete</a></center>'
                }
            }
        ]
    });
}
function EditStudent(id) {
    $('.clsRed').removeClass("clsRed");
    $.ajax({
        url: "/Home/GetStudentInfoById?id=" + id,
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            if (result.IsSuccess) {
                if (result.ResponseData != null) {
                    $('#hdnId').val(id);
                    $('#txtName').val(result.ResponseData.Name);
                    $('#txtEmail').val(result.ResponseData.Email);
                    GetCityByStateId(result.ResponseData.StateId, result.ResponseData.CityId)
                    $('#txtNo').val(result.ResponseData.PhoneNo);
                    $('#drpState').val(result.ResponseData.StateId);
                    $('#chkAgress').prop("checked", true);
                    $('#txtAddress').val(result.ResponseData.Address);
                    $('#btnSave').attr("disabled", false);
                    $('#mdlStudent').modal("show");
                }

            }
            else {
                toastr.error(result.Message);
            }
        },
        error: function (err) {
            toastr.error("Something Went Wrong.")
        }
    })
}
function DeleteStudent(id) {
    var ans = swal({
        title: "Are you sure?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                if (ans) {
                    $.ajax({
                        url: "/Home/DeleteStudent?id=" + id,
                        type: "GET",
                        contentType: "application/json",
                        success: function (result) {
                            if (result.IsSuccess) {
                                toastr.success(result.Message);
                                GetStudentData();

                            }
                            else {
                                toastr.error(result.Message);
                            }
                        },
                        error: function (err) {
                            toastr.error("Something Went Wrong.")
                        }
                    })
                }
            }
        });

}
$(document).on("click", "#btnAddStudent", function () {
    $('.clsRed').removeClass("clsRed");
    $('#txtName,#txtEmail,#txtNo,#drpState,#drpCity,#txtAddress,#hdnId').val("");
    $('#btnSave').attr("disabled", true);
    $('#chkAgress').prop("checked", false);
    $('#drpState').val("");
    $('#drpCity').empty();
    var chtml = '<option value="">Select City</option>';
    $('#drpCity').html(chtml);
    $('#mdlStudent').modal("show");
})
$(document).on("keyup", "#txtName,#txtEmail,#txtNo,#txtAddress", function () {
    if ($(this).val().trim() != "") {
        $(this).removeClass("clsRed");
    }
})
$(document).on("change", "#drpState", function () {
    if ($(this).val().trim() != "") {
        $(this).removeClass("clsRed");
        GetCityByStateId($(this).val())
    }
    else {
        var html = "";
        $('#drpCity').empty();
        html = '<option value="">Select City</option>';
        $('#drpCity').html(html);
    }

})
$(document).on("change", "#drpCity", function () {
    if ($(this).val().trim() != "") {
        $(this).removeClass("clsRed");
    }
})
$(document).on("change", "#chkAgress", function () {
    if ($(this).is(":checked")) {
        $('#btnSave').attr("disabled", false);
    }
    else {
        $('#btnSave').attr("disabled", true);
    }
});
function GetAllStates() {
    $.ajax({
        url: "/Home/GetAllStates",
        type: "GET",
        contentType: "application/json",
        success: function (result) {
            if (result.IsSuccess) {
                var html = "";
                $('#drpState').empty();
                if (result.ResponseData != null) {
                    html = '<option value="">Select State</option>';
                    for (var i = 0; i < result.ResponseData.length; i++) {
                        html += '<option value=' + result.ResponseData[i].Id + '>' + result.ResponseData[i].State + '</option>';
                    }
                }
                $('#drpState').html(html);
            }
            else {
                toastr.error(result.Message);
            }
        },
        error: function (err) {
            toastr.error("Something Went Wrong.")
        }
    })
}
function GetCityByStateId(id, cityId = null) {
    
    $.ajax({
        url: "/Home/GetCityByStateId?stateId=" + id,
        method: "GET",
        dataType: "JSON",
        success: function (result) {
            if (result.IsSuccess) {
                var html = "";
                $('#drpCity').empty();
                html = '<option value="">Select City</option>';
                if (result.ResponseData != null) {
                    for (var i = 0; i < result.ResponseData.length; i++) {
                        html += '<option value=' + result.ResponseData[i].Id + '>' + result.ResponseData[i].City + '</option>';
                    }
                }

                $('#drpCity').html(html);
                if (cityId != null) {
                    $('#drpCity').val(cityId);
                }
            }
            else {
                toastr.error(result.Message);
            }
        },
        error: function (err) {
            toastr.error("Something Went Wrong.")
        }
    })
}
$(document).on("click", "#btnSave", function () {
    if (CheckValidation()) {
        toastr.error("Please enter valid required fields.")
    }
    else if ($('#txtEmail').val().trim() != "" && !isEmail($('#txtEmail').val())) {
        toastr.error("Please enter valid email format.");

    }
    else if ($('#txtNo').val().trim() != "" && $('#txtNo').val().trim().length < 10) {
        toastr.error("Please enter valid mobile no.");
    }


    else {
        var studentInfo = {
            Id: $('#hdnId').val(),
            Name: $('#txtName').val().trim(),
            Email: $('#txtEmail').val().trim(),
            PhoneNo: $('#txtNo').val().trim(),
            StateId: $('#drpState').val(),
            CityId: $('#drpCity').val(),
            Address: $('#txtAddress').val().trim()
        }
        $.ajax({
            url: "/Home/AddUpdateStudent",
            method: "POST",
            data: studentInfo,
            dataType: "JSON",
            success: function (result) {
                if (result.IsSuccess) {
                    toastr.success(result.Message);
                    $('#mdlStudent').modal("hide");
                    GetStudentData();
                }
                else {
                    toastr.error(result.Message);
                }
            },
            error: function (err) {
                toastr.error("Something Went Wrong.")
            }
        })
    }
});
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function CheckValidation() {
    var IsStatus = false;
    if ($('#txtName').val().trim() == "") {
        $('#txtName').addClass("clsRed");
        IsStatus = true;
    }
    else {
        $('#txtName').removeClass("clsRed");
    }
    if ($('#txtEmail').val().trim() == "") {
        $('#txtEmail').addClass("clsRed");
        IsStatus = true;
    }
    else {
        $('#txtEmail').removeClass("clsRed");
    }

    if ($('#txtNo').val().trim() == "") {
        $('#txtNo').addClass("clsRed");
        IsStatus = true;
    }
    else {
        $('#txtNo').removeClass("clsRed");
    }

    if ($('#drpState').val() == "") {
        $('#drpState').addClass("clsRed");
        IsStatus = true;
    }
    else {
        $('#drpState').removeClass("clsRed");
    }

    if ($('#drpCity').val() == "") {
        $('#drpCity').addClass("clsRed");
        IsStatus = true;
    }
    else {
        $('#drpCity').removeClass("clsRed");
    }
    return IsStatus;
}

$(".OnlyNumbers").keydown(function (event) {
    if (!(event.keyCode == 8                                // backspace
        || event.keyCode == 9                               // tab
        || event.keyCode == 17                              // ctrl
        || event.keyCode == 46                              // delete
        || (event.keyCode >= 35 && event.keyCode <= 40)     // arrow keys/home/end
        || (event.keyCode >= 48 && event.keyCode <= 57)     // numbers on keyboard
        || (event.keyCode >= 96 && event.keyCode <= 105)    // number on keypad
        || (event.keyCode == 65 && prevKey == 17 && prevControl == event.currentTarget.id))          // ctrl + a, on same control
    ) {
        event.preventDefault();     // Prevent character input
    }
    else {
        prevKey = event.keyCode;
        prevControl = event.currentTarget.id;
    }
});