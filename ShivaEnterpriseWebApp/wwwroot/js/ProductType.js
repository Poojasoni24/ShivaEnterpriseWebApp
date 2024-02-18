//#region DELETE

//On Click of delete organization.
function onDeleteProductType(ProductType) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${ProductType.ProductTypeName} ?`,
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
            deleteProductTypeAJAX(ProductType.ProductTypeId);
        }
    });
}

//AJAX call for delete organization
function deleteProductTypeAJAX(productTypeId) {
    debugger;
    $.ajax({
        url: `/ProductType/RemoveProductType`,
        type: 'POST',
        data: { productTypeId: productTypeId },
        success: function (res) {
            if (res.success) {
                debugger;
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
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
function loadProductTypePartial(productTypeId) {
    $.ajax({
        url: `/ProductType/ProductTypeDetail?productTypeId=${productTypeId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#producttypeModalLong').modal('show')
            useJQueryNoConflict();
            $('#ProductTypeDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
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