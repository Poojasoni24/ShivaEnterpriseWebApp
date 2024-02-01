$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteCity(City) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${City.City_Name} ?`,
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
            deleteCityAJAX(City.City_Id);
        }
    });
}

//AJAX call for delete city
function deleteCityAJAX(cityId) {
    debugger;
    $.ajax({
        url: `/City/RemoveCity`,
        type: 'POST',
        data: { cityId: cityId },
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
function loadCityPartial(cityId) {
    $.ajax({
        url: `/Branch/BranchDetails?branchId=${branchId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#branchDetailDiv').html(res)
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