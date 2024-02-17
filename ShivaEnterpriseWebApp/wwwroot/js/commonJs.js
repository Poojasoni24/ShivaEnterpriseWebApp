//$(document).ready(function () {
//    debugger;
//    $("#navi ul li a").click(function (event) {
//        debugger;
//        if ($(this).find($(".fa")).hasClass('fa-chevron-down')) {
//            $(this).find($(".fa")).removeClass('fa-chevron-down').addClass('fa-chevron-up');
//            $(this).next().slideDown(200);
//        }
//        else if ($(this).find($(".fa")).hasClass('fa-chevron-up')) {

//            $(this).find($(".fa")).removeClass('fa-chevron-up').addClass('fa-chevron-down');
//            $("#navi ul li div").slideUp(200);
//        }

//        //$("#navi ul li div").slideUp(200);

//    });
//});


$(function () {
    debugger;
        $("#navi ul li div").addClass("divdesign");
    $("#navi ul li div").hide();

    $("#navi ul li a").on("click", function () {

            if ($(this).find($(".fa")).hasClass('fa-chevron-up')) {
        $(this).find($(".fa")).removeClass('fa-chevron-up').addClass('fa-chevron-up');
    $(this).next().slideDown(200);
            }
    else if ($(this).find($(".fa")).hasClass('fa-chevron-up')) {

        $(this).find($(".fa")).removeClass('fa-chevron-up').addClass('fa-chevron-down');
    $("#navi ul li div").slideUp(200);
            }

            //$("#navi ul li div").slideUp(200);

        });

    $(document).on("click", function () {
            if (!$("#navi ul li a").is(e.target) && !$("#navi ul li a i").is(e.target)) {

                if ($(this).find($(".fa")).hasClass('fa-chevron-up')) {
        $(this).find($(".fa")).removeClass('fa-chevron-up').addClass('fa-chevron-down');
    $("#navi ul li div").slideUp(200);
                }
            }
        });
    });
