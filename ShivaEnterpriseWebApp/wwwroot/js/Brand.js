

//#region DELETE

//On Click of delete organization.
function onDeleteBrand(Brand) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Brand.BrandName} ?`,
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
            deleteBrandAJAX(Brand.BrandId);
        }
    });
}

//AJAX call for delete organization
function deleteBrandAJAX(brandId) {
    debugger;
    $.ajax({
        url: `/Brand/RemoveBrand`,
        type: 'POST',
        data: { brandId: brandId },
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
function loadBrandPartial(brandId) {
    $.ajax({
        url: `/Brand/BrandDetail?brandId=${brandId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#BrandDetailDiv').html(res)
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