
//On Click of delete organization.
function onDeleteProduct(Product) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Product.ProductName} ?`,
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
            deleteProductAJAX(Product.ProductId);
        }
    });
}

//AJAX call for delete organization
function deleteProductAJAX(productId) {
    debugger;
    $.ajax({
        url: `/Product/RemoveProduct`,
        type: 'POST',
        data: { productId: productId },
        success: function (res) {
            if (res.success) {
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
            else {
                Snackbar.show({ text:"something went wrong", textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
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
function loadProductPartial(productId) {
    $.ajax({
        url: `/Product/ProductDetail?ProductId=${productId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#productModalLong').modal('show')
            useJQueryNoConflict();
            $('#productDetailDiv').html(res)
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