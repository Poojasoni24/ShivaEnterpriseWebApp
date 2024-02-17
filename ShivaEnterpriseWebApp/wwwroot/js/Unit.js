

//#region DELETE

//On Click of delete organization.
function onDeleteUnit(Unit) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Unit.UnitName} ?`,
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
            deleteUnitAJAX(Unit.UnitId);
        }
    });
}

//AJAX call for delete organization
function deleteUnitAJAX(unitId) {
    debugger;
    $.ajax({
        url: `/Unit/RemoveUnit`,
        type: 'POST',
        data: { unitId: unitId },
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
function loadUnitPartial(unitId) {
    $.ajax({
        url: `/Unit/UnitDetail?unitId=${unitId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#UnitDetailDiv').html(res)
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