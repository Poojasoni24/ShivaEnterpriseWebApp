$(document).ready(function () {
    $('#SaleOrderId').change(function () {
        var selectedValue = $(this).val();
        if (selectedValue != "") {
            myFunction(selectedValue);
        }
    });
});

function myFunction(value) {
    $.ajax({
        url: '/Outward/GetProductsAndCustomers',
        type: 'GET',
        data: { saleOrderId: value },
        success: function (response) {
            // Handle the response from the server
            console.log("Server response: " + response);

            var dropdown2 = $('#ProductId');
            dropdown2.empty();
            var appenddata2 = "";

            var dropdown1 = $('#CustomerId');
            dropdown1.empty();
            var appenddata1 = "";
            
            appenddata1 += "<option value = '" + "" + " '>" + "Select an option" + " </option>";
            appenddata2 += "<option value = '" + "" + " '>" + "Select an option" + " </option>";

            $.each(response.products, function (i, item) {
                appenddata2 += "<option value = '" + item.productId + " '>" + item.productName + " </option>";
            });

            $.each(response.customers, function (i, item) {
                appenddata1 += "<option value = '" + item.customerId + " '>" + item.customerName + " </option>";
            });

            $("#ProductId").append(appenddata2);
            $("#CustomerId").append(appenddata1);
        },
        error: function (xhr, status, error) {
            console.error("Error loading data");
        }
    });
}



function onDeleteOutward(Outward) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete outward details ?`,
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
            deleteOutwardAJAX(Outward.OutwardId);
        }
    });
}

//AJAX call for delete organization
function deleteOutwardAJAX(outwardId) {
    $.ajax({
        url: `/Outward/RemoveOutward`,
        type: 'POST',
        data: { outwardId: outwardId },
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

