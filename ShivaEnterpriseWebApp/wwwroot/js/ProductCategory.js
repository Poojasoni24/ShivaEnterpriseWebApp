
//On Click of delete organization.
function onDeleteProductCategory(ProductCategory) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${ProductCategory.ProductCategoryName} ?`,
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
            deleteBranchAJAX(ProductCategory.ProductCategoryId);
        }
    });
}

//AJAX call for delete organization
function deleteProductCategoryAJAX(productCategoryId) {
    debugger;
    $.ajax({
        url: `/ProductCategory/RemoveProductCatgory`,
        type: 'POST',
        data: { productCategoryId: productCategoryId },
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
function loadProductCategoryPartial(productCategoryId) {
    $.ajax({
        url: `/ProductCategory/ProductCategoryDetail?productCategoryId=${productCategoryId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#ProductCategoryDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
        },
        async: true,
        error: function (err) {
        }
    });

}