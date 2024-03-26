function onAddSalesOrder() {
    $.ajax({
        url: `/SalesOrder/AddOrEditSalesOrder`,
        type: 'GET',
        success: function (res) {
            window.location.replace('/SalesOrder/AddOrEditSalesOrder');
        },
        async: true,
        error: function (err) {
            Alert("Some thing went wrong");
        }
    });
}

//function onChange() {
//    $.ajax({
//        url: `/SalesOrder/IndexChange`,
//        type: 'GET',
//        success: function (res) {
//            window.location.replace('/SalesOrder/IndexChange');
//        },
//        async: true,
//        error: function (err) {
//            Alert("Some thing went wrong");
//        }
//    });
//}


//    $("#CustomerId").on('change',function () {
//        var url = '@Url.Content("~/")' + "SalesOrder/getCustomerbyId";
//        var ddlsource = "#CustomerId";
//        $.getJSON(url, { id: $(ddlsource).val() }, function (data) {
//            var items = '';
//            $("#CustomerId").empty();
//            $.each(data, function (i, row) {
//                items += "<option value='" + row.value + "'>" + row.text + "</option>";
//            });
//            $("#CustomerId").html(items);
//        })
//    });
//});

$.ajax({  //ajax call
    type: "POST",      //method == POST
    url: "/Home/selectSBU", //url to be called
    data: "id=" + selected_val, //data to be send
    success: function (data) {
        $('#GrpHead').val(data); // here we will set a value of text box
    }
});
    }
$('#unitPrice').on('change', function () {
    var qty = $("#qty").val();
    var unitprice = $("#unitPrice").val();
    var netPrice = qty * unitprice;
    $("#netTotal").val(netPrice);
});

$("#discount").on('change', function () {
    debugger;
    var discount = $("#discount").val();
    var netPrice = $("#netTotal").val();
    var discountAmount = netPrice * (discount / 100);
    $("#netTotal").val(netPrice - discountAmount);
});

//On Click of delete organization.
function onDeleteSO(SalesOrder) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${SalesOrder.Doc_No} ?`,
        //type: "warning",
        buttons: {
            yes: {
                text: "Delete",
                value: true
            },
            no: {
                text: "Cancel",
                value: false
            }
        },
        showCancelButton: false,
        confirmButtonColor: '#c10909'
    }).then(res => {
        if (res) {
            deleteSOAJAX(SalesOrder.SalesOrderId);
        }
    });
}

//AJAX call for delete organization
function deletePOAJAX(salesOrderId) {
    $.ajax({
        url: `/SalesOrder/RemoveSalesOrder`,
        type: 'POST',
        data: { salesOrderId: salesOrderId },
        success: function (res) {
            if (res.success) {
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
            else {
                Snackbar.show({ text: "something went wrong", textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
        },
        async: true,
        error: function (err) {
        }
    });
}

//#endregion

//Load branch Detail partial view

function modalclose() {
    window.location.reload();
}
function loadSOPartial(salesorderId) {
    $.ajax({
        url: `/SalesOrder/SalesOrderDetail?salesorderId=${salesorderId}`,
        type: 'GET',
        success: function (res) {

            $('#soModalLong').modal('show')
            useJQueryNoConflict();
            $('#salesOrderDetailDiv').html(res)
        },
        async: true,
        error: function (err) {
        }
    });

}

//Jquery No conflict 
function useJQueryNoConflict() {
    jQuery.noConflict();
}

