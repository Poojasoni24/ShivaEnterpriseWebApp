var ProductId, BrandId;
let updatedSOs = []

$(document).ready(function () {
    var counter = 0;
    $("#tblSalesOrderDetail thead tr").each(function () {
        var self = $(this);
        self.find('th').each(function () {
            self.find(".enable_disable").removeClass("d-none");
        });
    });

    $("#tblSalesOrderDetail tbody tr").each(function () {
        counter++;
        var self = $(this);
        self.addClass("row_" + counter);
        var tdCounter = 0;

        self.find("input[value='Remove']").addClass("d-none");
        self.find('td').each(function () {
            tdCounter++;
            $(this).addClass("row_" + counter + tdCounter);
            self.find("#enable_disable").removeClass("d-none");
            self.find("#enable_disable").addClass("d-block");
            self.find('#enable_disable').on('change', function () {
                //var selectedCheckboxes = $('#enable_disable:checked');
                var isChecked = $(this).prop('checked');
                var qtycell = $(this).closest("tr").find(".qty-cell");
                var unitpricecell = $(this).closest("tr").find(".unitprice-cell");
                var discountcell = $(this).closest("tr").find(".discount-cell");
                var netTotalcell = $(this).closest("tr").find(".nettotal-cell");
                if (isChecked) {

                    var qtycontent = qtycell.text();
                    if (qtycontent != '') {
                        qtycell.html("<input type='Number' id='txtupdatedQty' value='" + qtycontent.trim() + "' onchange='SOCalculationAfterUpdate()' />");
                    }

                    var unitpricecontent = unitpricecell.text();
                    if (unitpricecontent != '') {
                        unitpricecell.html("<input type='Number' id='txtupdatedUnitprice' value='" + unitpricecontent.trim() + "' onchange='SOCalculationAfterUpdate()'/>");
                    }

                    var discountcontent = discountcell.text();
                    if (discountcontent != '') {
                        discountcell.html("<input type='Number' id='txtupdatedDiscount' value='" + discountcontent.trim() + "'onchange='SOUpdateDiscount()' />");
                    }

                    var netTotalContent = netTotalcell.text();
                    if (discountcontent != '') {
                        netTotalcell.html("<input class='form-control' type='text' id='txtupdatednetTotal' value='" + netTotalContent.trim() + "' readonly />");
                    }
                    self.find("#btnupdate").removeClass("d-none");
                    $('#btnAdd').attr("disabled", true);
                    //self.find("#btnupdate").addClass("d-block");


                }
                else {
                    var qtycontent = qtycell.find("input[type='Number']").val();
                    qtycell.html(qtycontent);

                    var unitpricecontent = unitpricecell.find("input[type='Number']").val();
                    unitpricecell.html(unitpricecontent);

                    var discountcontent = discountcell.find("input[type='Number']").val();
                    discountcell.html(discountcontent);

                    var netTotalContent = netTotalcell.find("input[type='text']").val();
                    netTotalcell.html(netTotalContent);

                    self.find("#btnupdate").addClass("d-none");
                    $('#btnAdd').attr("disabled", false);
                    //self.find("#btnupdate").removeClass("d-block");
                }

            });
        });
    });


});


$("#sodetaildiv").on("click", "#btnAdd", function () {
    //Reference the Name and Country TextBoxes.
    var txtsoproduct = $("#ProductId").find(":selected").text();
    ProductId = $("#ProductId").find(":selected").val();
    var txtsoBrand = $("#BrandId").find(":selected").text();
    BrandId = $("#BrandId").find(":selected").val();
    var txtsoQty = $("#txtsoQty");
    var txtsoUnitprice = $("#txtsoUnitprice");
    var txtsoDiscount = $("#txtsoDiscount");
    var txtsoNetTotal = $("#txtsonetTotal");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblSalesOrderDetail > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add ProductId cell.
    var cell = $(row.insertCell(-1));
    cell.html(ProductId);
    cell.addClass("d-none");

    //Add BrandId cell.
    var cell = $(row.insertCell(-1));
    cell.html(BrandId);
    cell.addClass("d-none");

    //Add CreatedBy cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add Createddate cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add Product cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtsoproduct);

    //Add Brand cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtsoBrand);

    //Add Qty cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtsoQty.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoUnitprice.val());

    //Add Discount cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoDiscount.val());

    //Add NetTotal cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoNetTotal.val());
    cell.addClass("nettotal-cell");

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtsoQty.val("");
    txtsoUnitprice.val("");
    txtsoDiscount.val("");
    txtsoNetTotal.val("");
    ProductId.val(0);
    BrandId.val(0);
});


$("#sodetaildiv").on("click", "#btnupdate", function () {
    debugger;
    var salesOrderDetailIdcell = $(this).closest("tr").find(".salesOrderDetailId-cell");
    var salesOrderIdcell = $(this).closest("tr").find(".salesOrderId-cell");
    var productIdcell = $(this).closest("tr").find(".productId-cell");
    var BrandIdcell = $(this).closest("tr").find(".brandId-cell");
    var qtycell = $(this).closest("tr").find(".qty-cell");
    var unitpricecell = $(this).closest("tr").find(".unitprice-cell");
    var discountcell = $(this).closest("tr").find(".discount-cell");
    var netTotalcell = $(this).closest("tr").find(".nettotal-cell");
    var CreatedBy = $(this).closest("tr").find(".createdby-cell");
    var CreatedDt = $(this).closest("tr").find(".createddate-cell");

    var salesOrderDetailIdContent = salesOrderDetailIdcell.text();
    var salesOrderIdContent = salesOrderIdcell.text();
    var productIdContent = productIdcell.text();
    var brandIdContent = BrandIdcell.text();
    var netTotalContent = netTotalcell.text();
    var createdByContent = CreatedBy.text();
    var createdDtContent = CreatedDt.text();
    var parts = createdDtContent.split(/[- :]/);
    // Note: Month is 0-based, so subtract 1 from the month value
    var dateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);

    var qtyContent = qtycell.find("input[type='Number']").val();
    qtycell.html(qtyContent);

    var unitpriceContent = unitpricecell.find("input[type='Number']").val();
    unitpricecell.html(unitpriceContent);

    var discountContent = discountcell.find("input[type='Number']").val();
    discountcell.html(discountContent);

    var netTotalContent = netTotalcell.find("input[type='text']").val();
    netTotalcell.html(netTotalContent);
    $("#tblSalesOrderDetail tbody tr").each(function () {
        var counter = 0;
        counter++;
        var self = $(this);
        var tdCounter = 0;
        self.find('td').each(function () {
            tdCounter++;
            if (self.find('#enable_disable').prop('checked')) {
                self.find('#enable_disable').prop('checked', false).trigger('change');
            }

        });
    });

    SOTotalAmountCalculation();

    let SalesOrderDetail = {
        SalesOrderDetailId: salesOrderDetailIdContent,
        SalesOrderId: salesOrderIdContent,
        ProductId: productIdContent,
        BrandId: brandIdContent,
        Quantity: qtyContent,
        Discount: discountContent,
        UnitPrice: unitpriceContent,
        NetTotal: netTotalContent,
        Tax_Percentage: $('#SalesOrder_tax').val().toString(),
        CreatedBy: createdByContent,
        //CreatedDateTime: createdDtContent,
    }
    updatedSOs.push(SalesOrderDetail);
});
function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblSalesOrderDetail")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("#btnSalesSave").on("click", function () {
    debugger;
    //Loop through the Table rows and build a JSON array.
    let Sodetails = [];
    let SoHeaders = [];
    var createdDtContent = $('#SalesOrder_CreatedDateTime').val() == "" ? "01-01-0001 00:00:00" : $('#SalesOrder_CreatedDateTime').val();
    var parts = createdDtContent.split(/[- :]/);
    // Note: Month is 0-based, so subtract 1 from the month value
    var dateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);

    $("#tblSalesOrderDetail TBODY TR").each(function () {
        var row = $(this);
        var soCreatedDt = row.find("TD").eq(5).html() == "" ? "01-01-0001 00:00:00" : row.find("TD").eq(5).html();
        var parts = soCreatedDt.split(/[- :]/);
        var sodetaildateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);
        // Note: Month is 0-based, so subtract 1 from the month value
        let SODetail = {
            SalesOrderDetailId: row.find("TD").eq(0).html() == "" ? "00000000-0000-0000-0000-000000000000" : row.find("TD").eq(0).html(),
            SalesOrderId: row.find("TD").eq(1).html() == "" ? "00000000-0000-0000-0000-000000000000" : row.find("TD").eq(1).html(),
            ProductId: row.find("TD").eq(2).html(),
            BrandId: row.find("TD").eq(3).html(),
            CreatedBy: row.find("TD").eq(4).html() == "" ? null : row.find("TD").eq(4).html(),
            CreatedDateTime: sodetaildateTimeObject,
            Quantity: row.find("TD").eq(8).html(),
            UnitPrice: row.find("TD").eq(9).html(),
            Discount: row.find("TD").eq(10).html(),
            NetTotal: row.find("TD").eq(11).html(),
        };
        Sodetails.push(SODetail);
    });
    //Sodetails = Sodetails.filter(val => !updatedSOs.includes(val));
    let SOHeader = {

        SalesOrderId: $('#SalesOrder_SalesOrderId').val() == "" ? "00000000-0000-0000-0000-000000000000" : $('#SalesOrder_SalesOrderId').val(),
        CustomerId: $('#SalesOrder_CustomerId').val(),
        OrderDate: $('#SalesOrder_orderDate').val(),
        DeliveryDate: $('#SalesOrder_deliveryDate').val(),
        TotalAmount: $('#SalesOrder_totalNumber').val(),
        Doc_No: $('#docNo').val(),
        Tax_Percentage: $('#SalesOrder_tax').val(),
        CreatedBy: $('#SalesOrder_CreatedBy').val() == "" ? null : $('#SalesOrder_CreatedBy').val(),
        CreatedDateTime: dateTimeObject,
    }


    let SalesOrderViewModel = {
        SalesOrder: SOHeader,
        SODetail: Sodetails,
        UpdatedSODetail: updatedSOs
    }

    SoHeaders.push(SOHeader);
    //Send the JSON array to Controller using AJAX.
    $.ajax({
        url: "/SalesOrder/AddOrEditSalesOrder",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(SalesOrderViewModel),
        success: function (r) {
            if (r === "Product Quantity is less than required.") {
                swal({
                    title: "Warning!!",
                    text: r,
                    //type: "warning",
                    buttons: {
                        no: {
                            text: "Cancel",
                            value: false
                        }
                    },
                    showCancelButton: false,
                    confirmButtonColor: '#c10909'
                }).then(res => {
                    var isError3 = true;
                });
            }
            else {
                Snackbar.show({ text: "Sales Order Created", textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.replace('/SalesOrder', 'SalesOrder/AddOrEditSalesOrder') }, 1500);
            }
        }
    });
});

$('#txtsoUnitprice').on('change', function () {
    var qty = $("#txtsoQty").val();
    var unitprice = $("#txtsoUnitprice").val();
    var netPrice = qty * unitprice;
    $("#txtsonetTotal").val(netPrice);
});

$("#txtsoDiscount").on('change', function () {
    var discount = $("#txtsoDiscount").val();
    var netPrice = $("#txtsonetTotal").val();
    var discountAmount = netPrice * (discount / 100);
    $("#txtsonetTotal").val(netPrice - discountAmount);
});

function SOTotalAmountCalculation() {
    var total = 0;
    var tax = $('#tax').val();
    $("#tblSalesOrderDetail tbody tr").each(function () {
        var Networkcell = parseInt($(this).closest("tr").find(".nettotal-cell").text());
        total = total + Networkcell;
    });

    total = total + (total * tax / 100);
    $('#totalNumber').val(total);
}

function SOCalculationAfterUpdate() {
    var qty = $("#txtupdatedQty").val();
    var unitprice = $("#txtupdatedUnitprice").val();
    var netPrice = qty * unitprice;
    $("#txtupdatednetTotal").val(netPrice);
    SOUpdateDiscount();
}

function SOUpdateDiscount() {
    var discount = $("#txtupdatedDiscount").val();
    var netPrice = $("#txtupdatednetTotal").val();
    var discountAmount = netPrice * (discount / 100);
    $("#txtupdatednetTotal").val(netPrice - discountAmount);
}

function SOCalculation() {
    var qty = $("#txtQty").val();
    var unitprice = $("#txtUnitprice").val();
    var netPrice = qty * unitprice;
    $("#txtnetTotal").val(netPrice);
}
