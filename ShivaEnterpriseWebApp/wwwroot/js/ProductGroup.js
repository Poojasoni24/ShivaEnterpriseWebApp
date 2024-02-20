//#region DELETE

//On Click of delete organization.
function onDeleteProductGroup(ProductGroup) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${ProductGroup.ProductGroupName} ?`,
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
            deleteProductGroupAJAX(ProductGroup.ProductGroupId);
        }
    });
}

//AJAX call for delete organization
function deleteProductGroupAJAX(productGroupId) {
    debugger;
    $.ajax({
        url: `/ProductGroup/RemoveProductGroup`,
        type: 'POST',
        data: { productGroupId: productGroupId },
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
function loadProductGroupPartial(productGroupId) {
    $.ajax({
        url: `/ProductGroup/ProductGroupDetail?productGroupId=${productGroupId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#productgroupModalLong').modal('show')
            useJQueryNoConflict();
            $('#productgroupDetailDiv').html(res)
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