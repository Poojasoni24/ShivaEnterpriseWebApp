$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete country.
function onDeleteCountry(Country) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Country.Country_Name} ?`,
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
            deleteCountryAJAX(Country.Country_Id);
        }
    });
}

//AJAX call for delete organization
function deleteCountryAJAX(countryId) {
    debugger;
    $.ajax({
        url: `/Country/RemoveCountry`,
        type: 'POST',
        data: { countryId: countryId },
        success: function (res) {
            if (res.success) {
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

//Load country Detail partial view

function modalclose() {
    window.location.reload();
}
function loadCountryPartial(countryId) {
    $.ajax({
        url: `/Country/CountryDetail?countryId=${countryId}`,
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