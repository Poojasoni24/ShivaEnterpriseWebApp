//$(document).ready(function () {
//    console.log("SalesReturn DOcument FUnction Called")
//    $('#SalesOrderID').change(function () {
//        var selectedValue = $(this).val();
//        if (selectedValue != "") {
//            fetchSalesOrderDetails(selectedValue);
//        }
//    });
//});

//function fetchSalesOrderDetails(salesOrderId) {
//    $.ajax({
//        url: '/SalesReturn/GetProductBrandAndQuantity', // The API endpoint that returns the data
//        type: 'GET',
//        data: { salesOrderId: salesOrderId },
//        success: function (response) {
//            // Handle the response from the server
//            console.log("Server response: " + response);

//            // Update Product Dropdown
//            var productDropdown = $('#ProductId');
//            productDropdown.empty();
//            var productOptions = "<option value=''>Select an option</option>";
//            $.each(response.products, function (i, item) {
//                productOptions += "<option value='" + item.productId + "'>" + item.productName + "</option>";
//            });
//            productDropdown.append(productOptions);

//            // Update Brand Dropdown
//            var brandDropdown = $('#BrandId');
//            brandDropdown.empty();
//            var brandOptions = "<option value=''>Select an option</option>";
//            $.each(response.brands, function (i, item) {
//                brandOptions += "<option value='" + item.brandId + "'>" + item.brandName + "</option>";
//            });
//            brandDropdown.append(brandOptions);

//            // Update Current Quantity
//            $('#CurrentQuantity').val(response.currentQuantity);
//        },
//        error: function (xhr, status, error) {
//            console.error("Error loading data: " + error);
//        }
//    });
//}

// Function to confirm deletion
function onDeleteSR(SalesReturn) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete?`,
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
            // Ensure proper case here
            deleteSRAJAX(SalesReturn.SalesReturnID);
        }
    });
}

// AJAX call for deleting SalesReturn
function deleteSRAJAX(SalesReturnID) {
    $.ajax({
        url: `/SalesReturn/DeleteSalesReturn`,
        type: 'POST',
        data: { SalesReturnId: SalesReturnID }, // Consistent naming here
        success: function (res) {
            if (res.success) {
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
            else {
                Snackbar.show({ text: "Something went wrong", textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
        },
        async: true,
        error: function (err) {
            console.error(err); // Add error logging here for debugging
        }
    });
}

//#endregion

//Load branch Detail partial view

function modalclose() {
    window.location.reload();
}
function loadSOPartial(SalesReturnId) {
    $.ajax({
        url: `/SalesReturn/SalesReturnDetail?SalesReturnId=${SalesReturnId}`,
        type: 'GET',
        success: function (res) {

            $('#soModalLong').modal('show')
            useJQueryNoConflict();
            $('#SalesReturnDetailDiv').html(res)
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



