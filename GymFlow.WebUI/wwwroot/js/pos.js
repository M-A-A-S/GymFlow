let cart = [];

let payments = [];

let products =  window.products ?? [];

let subscriptions = window.subscriptions ?? [];

let members = window.members ?? [];

// let resources = window.resources ?? {};

let currentLanguage = window.currentLanguage ?? "en";

console.log('products -> ', products)
console.log('subscriptions -> ', subscriptions)
console.log('members -> ', members)

let selectedType = "Product";

let selectedCategory = 0;

let invoiceId = null;

$(document).ready(function () {


    initializeSelect2();


    initializeEvents();


    calculateInvoice();

    loadItems(selectedType, selectedCategory, "")


});

// Select2 Member Search
function initializeSelect2() {
    $("#MemberId").select2({
        width: "100%",
        allowClear: true,
    });

}

// On select member
//$('#MemberId').on('select2:select', function (e) {

//    let option = $(this).find(':selected')
//    console.log('option -> ', option)

//    console.log(option[0]);
//    console.log(option.attr('data-member-subscriptionName'));
//    console.log(option.attr('data-member-subscriptionStartDate'));
//    console.log(option.attr('data-member-subscriptionEndDate'));
//    console.log(option.data());

//    //$('#memberInfo').show();

//    console.log(option.data('member-subscriptionName'))

//    $('#memberName').text(option.data('member-name'));

//    $('#memberPhone').text(option.data('member-phone'));

//    $('#memberSubscription').text(option.data('member-subscriptionName'));

//    $('#subscriptionStartDate').text(option.data('member-subscriptionStartDate'));

//    $('#subscriptionEndDate').text(option.data('member-subscriptionEndDate'));

//    // $('#lblSubscription').text(member.subscription);

//    // $('#lblRemaining').text(member.remainingDays);

//    // $('#lblBalance').text(member.balance);

//    // $('#lblStatus').html(member.statusBadge);

//});


// Events Initialization
function initializeEvents() {

    $('#MemberId').on('select2:select', handleMemberChange)


    $(".item-type")
        .on("click", changeItemType);



    $(".category-btn")
        .on("click", changeCategory);



    $("#txtItemSearch")
        .on("keyup", filterItems);

    $(document)
        .on("click", ".btn-add-item", openItemSettings);

    $("#btnAddToCart")
        .on("click", addToCart);

    $("#btnClearCart")
        .on("click", clearCart);

    $(document)
        .on("click", ".btnRemoveCart", removeCartItem);

    $(document).on(
        "keyup change",
        "#Quantity,#UnitPrice,#Discount",
        calculateProductTotal
    );

    $("#invoiceDiscount,#invoiceTax")
        .on(
            "keyup change",
            calculateInvoice
        );

    $("#btnCompleteSale")
        .on("click", completeSale);



    $("#btnAddPayment")
        .on("click", addPayment);

    $(document)
        .on(
            "click",
            ".btn-remove-payment",
            removePayment
    );

    $("#btnCompleteSale")
        .on(
            "click",
            completeSale
    );

    $("#btnSaveDraft")
        .on(
            "click",
            function () {

                saveInvoice("Draft");

            }
    );

    $("#btnCancelInvoice")
        .on(
            "click",
            cancelInvoice
    );

    $("#btnPrintInvoice")
        .on(
            "click",
            printInvoice
        );

}

function handleMemberChange(e) {

    console.log("text");

    const memberId = Number($("#MemberId").val());

    console.log("memberId -> ", memberId);

    const member = members.find(x => x.Id === memberId);

    console.log("members -> ", members);

    console.log("member -> ", member);

    if (!member) {
        $("#memberInfo").hide();
        return;
    }

    $("#memberInfo").show();

    $('#memberName').text(member.FullName);

    $('#memberPhone').text(member.PhoneNumber);

    if (member.CurrentSubscription) {
        $('#memberSubscription').text(currentLanguage == "en" ? member.CurrentSubscription.NameEn : member.CurrentSubscription.NameAr)
    }
    else {
        $('#memberSubscription').text("-");
    }

    $('#subscriptionStartDate').text(member?.CurrentSubscription?.StartDate ?? "-");

    $('#subscriptionEndDate').text(member?.CurrentSubscription?.EndDate ?? "-");

}


//function handleMemberChange(e) {
//    console.log('e -> ', e)
//    let option = $(this).find(':selected')
//    console.log('option -> ', option)

//    console.log(option[0].outerHTML);

//    //console.log('members -> ', members)

//    console.log(option[0]);
//    console.log(option.attr('data-member-subscriptionName'));
//    console.log(option.attr('data-member-subscriptionStartDate'));
//    console.log(option.attr('data-member-subscriptionEndDate'));
//    console.log(option.data());

//    $('#memberInfo').show();

//    console.log(option.data('member-subscriptionName'))

//    $('#memberName').text(option.data('member-name'));

//    $('#memberPhone').text(option.data('member-phone'));

//    $('#memberSubscription').text(option.data('member-subscriptionName'));

//    $('#subscriptionStartDate').text(option.data('member-subscriptionStartDate'));

//    $('#subscriptionEndDate').text(option.data('member-subscriptionEndDate'));
//}

// Item Type Change
function changeItemType() {

    $(".item-type")
        .removeClass("btn-primary active")
        .addClass("btn-outline-primary");


    $(this)
        .removeClass("btn-outline-primary")
        .addClass("btn-primary active");



    selectedType =
        $(this).data("type");

    console.log('selectedType -> ', selectedType)



    $("#SelectedItemType")
        .val(selectedType);



    loadCategories();


    filterItems();

}

// Load Categories
function loadCategories() {

    $(".category-btn")
        .each(function () {

            let type =
                $(this).data("type");


            if (type == selectedType ||
                $(this).data("id") == 0) {

                $(this).show();

            }
            else {

                $(this).hide();

            }


        });

}

// On select Category
// $(document).on("click", ".category-btn", changeCategory);

// Change Category
function changeCategory() {
    $(".category-btn")
        .removeClass("btn-primary active")
        .addClass("btn-outline-primary");

    $(this)
        .removeClass("btn-outline-primary")
        .addClass("btn-primary active");

    let categoryId = $(this).data("id");

    selectedCategory = categoryId;

    console.log("categoryId -> ", categoryId)

    $("#SelectedCategoryId").val(categoryId);

    filterItems();
}


// On Search for category
$("#txtCategorySearch").on("keyup", function () {

    let value = $(this).val().toLowerCase();

    $(".category-btn").each(function () {

        let text = $(this).text().toLowerCase();

        if (text.indexOf(value) > -1)
            $(this).show();
        else
            $(this).hide();

    });

});

// function filterItems() {

//     let type = $("#SelectedItemType").val();

//     let categoryId = Number($("#SelectedCategoryId").val());

//     let search = $("#txtItemSearch").val();

//     loadItems(type, categoryId, search);

// }

// Filter Items

function filterItems() {

    let search =
        $("#txtItemSearch")
            .val()
            .toLowerCase();



    let data =
        selectedType == "Product"
            ? products
            : subscriptions;



    if (selectedCategory > 0) {

        data =
            data.filter(x =>
                x.categoryId == selectedCategory
            );

    }



    if (search) {

        data =
            data.filter(x =>
                x.NameEn.toLowerCase().includes(search) ||
                x.NameAr.includes(search));

    }



    renderItems(data, selectedType);

}

// Load items
function loadItems(type, categoryId, search) {

    let data = [];

    if (type === "Product") {
        data = products;
    }
    else {
        data = subscriptions;
    }

    if (categoryId > 0) {
        data = data.filter(x => x.categoryId == categoryId);
    }

    console.log("data -> ", data)

    if (search) {
        data = data.filter(x =>
            x.NameEn.toLowerCase().includes(search.toLowerCase()) ||
            x.NameAr.includes(search.toLowerCase())
        );
    }

    renderItems(data, type);

}

// Render Item Cards
function renderItems(items, type) {

    console.log("renderItems items -> ", items)
    console.log("type -> ", type)

    let html = "";

    items.forEach(function (item) {
        html += createCard(item, type);
    });

    $("#itemContainer")
        .html(html);

}

// Create Card
function createCard(item, type) {

    if (type === "Product") {
        return createProductCard(item);
    }


    return createSubscriptionCard(item);

}

// Product Card
function createProductCard(item) {

    
    return `

<div class="col-md-4 col-lg-3">


<div class="card product-card h-100">


<div class="card-body">


<h6>
${currentLanguage == "en" ? item.NameEn : item.NameAr}
</h6>


<span class="badge bg-primary">

${formatMoney(item.SalePrice)}

</span>


</div>


<div class="card-footer">


<button 
class="btn btn-primary w-100 btn-add-item"
data-id="${item.Id}"
data-type="Product">


<i class="fa fa-plus"></i>

 ${resources.add}


</button>


</div>


</div>


</div>

`;

}

// Subscription Card
function createSubscriptionCard(item) {

    return `

<div class="col-md-4 col-lg-3">


<div class="card product-card h-100">


<div class="card-body">


<h6>
${currentLanguage == "en" ? item.NameEn : item.NameAr}
</h6>


<span class="badge bg-primary">

${formatMoney(item.Price)}

</span>


<br/>


<span class="badge bg-warning mt-2">

${formatDays(item.DurationDays)}

</span>


</div>


<div class="card-footer">


<button 
class="btn btn-primary w-100 btn-add-item"
data-id="${item.Id}"
data-type="Subscription">


<i class="fa fa-plus"></i>

${resources.add}


</button>


</div>


</div>


</div>

`;

}

// Format Days

function formatDays(days) {
    if (!days || days <= 0)
        return "";


    let text = resources.day;


    if (currentLanguage === "ar") {

        if (days === 1) {
            text = "يوم";
        }
        else if (days === 2) {
            text = "يومان";
        }
        else if (days >= 3 && days <= 10) {
            text = "أيام";
        }
        else {
            text = "يوم";
        }

    }


    return `${days} ${text}`;
}

// openItemSettings
function openItemSettings() {

    let id = $(this).data("id");
    let type = $(this).data("type");

    $("#ItemId").val(id);
    $("#ItemType").val(type);

    if (type === "Subscription" && !$("#MemberId").val()) {
        toastr.error(resources.memberRequiredForSubscription);
        return;
    }

    if (type === "Product") {
        loadProductSettings(id);
    }


    if (type === "Subscription") {
        loadSubscriptionSettings(id);
    }

    let element =
        document.getElementById(
            "itemSettingsOffcanvas"
        );


    let offcanvas =
        new bootstrap.Offcanvas(element);

    offcanvas.show();

}

function loadProductSettings(id) {

    let product =
        products.find(x => x.Id == id);

    console.log('product -> ', product)
    console.log('id -> ', id)

    if (!product) {
        console.error("Product not found");
        return;
    }


    $("#itemSettingsContainer")
        .html(
            $("#productSettingsTemplate").html()
        );


    $("#ProductName")
        .val(currentLanguage == "en" ? product.NameEn : product.NameAr);


    $("#UnitPrice")
        .val(product.SalePrice);


    $("#Quantity")
        .val(1);

    $("#Discount")
        .val(0);


    calculateProductTotal();

}

function calculateProductTotal() {

    let qty =
        Number($("#Quantity").val());


    let price =
        Number($("#UnitPrice").val());


    let discount =
        Number($("#Discount").val());


    let total =
        qty * price - discount;


    $("#ProductTotal")
        .text(total.toFixed(2));

}

function loadSubscriptionSettings(id) {

    let subscription =
        subscriptions.find(x => x.Id == id);


    console.log(
        "subscription -> ",
        subscription
    );



    if (!subscription) {
        console.error("Subscription not found");
        return;
    }

    $("#itemSettingsContainer")
        .html(
            $("#subscriptionSettingsTemplate").html()
        );


    $("#SubscriptionName")
        .val(currentLanguage == "en" ? subscription.NameEn : subscription.NameAr);


    $("#SubscriptionPrice")
        .val(subscription.Price);

    $("#SubscriptionDuration").html(formatDays(subscription.DurationDays));


    $("#StartDate")
        .val(
            new Date()
                .toISOString()
                .substring(0, 10)
        );


    calculateEndDate(
        subscription.DurationDays
    );

}

function calculateEndDate(days) {

    let start =
        new Date(
            $("#StartDate").val()
        );


    start.setDate(
        start.getDate() + days
    );


    $("#EndDate")
        .val(
            start.toISOString()
                .substring(0, 10)
        );

}

$(document).on(
    "change",
    "#StartDate",
    function () {

        let id =
            $("#ItemId").val();


        let plan =
            subscriptions.find(
                x => x.Id == id
            );


        if (plan) {
            calculateEndDate(
                plan.DurationDays
            );
        }

    }
);

// Add To Cart
function addToCart() {

    let type = $("#ItemType").val();

    if (type === "Product") {
        addProductToCart();
    }

    if (type === "Subscription") {
        addSubscriptionToCart();
    }

    renderCart()
    closeItemSettings();
}

// Add Product To Cart
function addProductToCart() {
    console.log("cart -> ", cart)

    let id = Number($("#ItemId").val());

    let type = "Product";

    let name = $("#ProductName").val();

    let quantity = Number($("#Quantity").val());

    let price = Number($("#UnitPrice").val());

    let discount = Number($("#Discount").val());

    let total = Number($("#ProductTotal").text());

    let existing = cart.find(x => x.type == "Product" && Number(x.id) === id)

    if (existing) {
        existing.quantity += quantity;

        existing.discount += discount;

        existing.total = (existing.quantity * existing.price) - existing.discount;

        return;
    }

    let item = {

        id: id,

        type: "Product",

        name: $("#ProductName").val(),

        price: price,

        quantity: quantity,

        discount: discount,

        total: (quantity * price) - discount

    };

    cart.push(item);
}

// Add Subscription To Cart
function addSubscriptionToCart() {

    console.log("cart -> ", cart)

    let id = Number($("#ItemId").val());

    // let existing = cart.find(x =>
    //     x.type === "Subscription" &&
    //     Number(x.id) === id
    // );
    let existing = cart.find(x => x.type === "Subscription");

    if (existing) {

        existing.id = $("#ItemId").val();

        existing.name = $("#SubscriptionName").val();

        existing.startDate = $("#StartDate").val();

        existing.endDate = $("#EndDate").val();

        existing.price = Number($("#SubscriptionPrice").val());

        existing.total = existing.price;

        return;
    }

    let item =
    {

        id: $("#ItemId").val(),

        type: "Subscription",

        name:
            $("#SubscriptionName").val(),


        quantity: 1,


        startDate:
            $("#StartDate").val(),

        endDate:
            $("#EndDate").val(),

        price:
            Number($("#SubscriptionPrice").val()),

        total:
            Number($("#SubscriptionPrice").val())

    };


    cart.push(item);

}

// close Item Settings
function closeItemSettings() {

    let element =
        document.getElementById(
            "itemSettingsOffcanvas"
        );


    let offcanvas =
        bootstrap.Offcanvas.getInstance(element);


    if (offcanvas) {
        offcanvas.hide();
    }

}

//Render Cart
function renderCart() {

    let html = "";


    cart.forEach((item, index) => {


        html += `

        <tr>

            <td>
                <strong>
                    ${item.name}
                </strong>
            </td>


            <td>

                <span class="badge bg-primary">
                    ${item.type == "Product" ? resources.product : resources.subscription}
                </span>

            </td>


            <td>
                ${item.quantity ?? 1}
            </td>


            <td>
                ${formatMoney(item.price)}
            </td>


            <td>
                ${formatMoney(item.total)}
            </td>


            <td>

                <button
                class="btn btn-sm btn-danger btnRemoveCart"
                data-index="${index}">

                    <i class="fa fa-times"></i>

                </button>

            </td>

        </tr>

        `;


    });



    $("#cartContainer")
        .html(html);


    calculateInvoice();

}

// Remove Cart Item
function removeCartItem() {

    let index =
        $(this).data("index");


    cart.splice(index, 1);


    renderCart();

}

// Clear Cart
function clearCart() {

    cart = [];


    renderCart();

}

// Calculations

function calculateTotalPaid() {
    return payments.reduce((sum, payment) => sum + (Number(payment.amount) || 0), 0);
}

function calculateRemaingAmount() {
    var paidAmount = calculateTotalPaid();

    var grandTotal = calculateGrandTotal();

    return grandTotal - paidAmount;
}

function calculateSubtotal() {
    return cart.reduce((sum, item) => sum + (Number(item.total) || 0), 0);
}

function calculateGrandTotal() {

    let discount =
        Number($("#invoiceDiscount").val()) || 0;


    let tax =
        Number($("#invoiceTax").val()) || 0;

    let subTotal = calculateSubtotal();

    return subTotal + tax - discount;
}

function calculateChangeAmount() {
    let grandTotal = calculateGrandTotal();
    let paidAmound = calculateTotalPaid();

    let remaining = grandTotal - paidAmound;

    if (remaining < 0) {
        return Math.abs(remaining);
    }


    return 0;
}

// function calculateTotalPaid() {

//     let total =
//         payments.reduce(
//             (sum, p) =>
//                 sum + p.amount,
//             0
//         );


//     $("#totalPaid")
//         .text(
//             formatMoney(total)
//         );


//     calculatePaymentStatus();

// }

// function calculateInvoiceTotals() {

//     let subtotal =
//         cart.reduce(
//             (sum, item) =>
//                 sum + Number(item.total),
//             0
//         );


//     let discount =
//         Number($("#invoiceDiscount").val()) || 0;


//     let tax =
//         Number($("#invoiceTax").val()) || 0;


//     let grandTotal =
//         subtotal - discount + tax;



//     return {

//         subtotal: subtotal,

//         discount: discount,

//         tax: tax,

//         total: grandTotal

//     };

// }

function calculateInvoice() {

    let subtotalAmount = calculateSubtotal();

    let grandTotalAmount = calculateGrandTotal();

    let totalPaidAmount = calculateTotalPaid();

    let remaingAmount = calculateRemaingAmount();

    let changeAmount = calculateChangeAmount();

    // let totals =
    //     calculateInvoiceTotals();


    $("#subtotal")
        .text(
            formatMoney(subtotalAmount)
        );


    $("#grandTotal")
        .text(
            formatMoney(grandTotalAmount)
        );


    $("#paidAmount")
        .text(
            formatMoney(totalPaidAmount)
        );


    $("#remainingAmount")
        .text(
            formatMoney(remaingAmount)
        );


    $("#changeAmount")
        .text(
            formatMoney(changeAmount)
        );

    $("#totalPaid")
        .text(
            formatMoney(totalPaidAmount)
        );


    // calculatePaymentStatus();

}



// function calculatePaymentStatus() {

//     let total =
//         getGrandTotal();



//     let paid =
//         payments.reduce(
//             (sum, p) => sum + Number(p.amount),
//             0
//         );



//     let remaining =
//         total - paid;


//     let change = 0;



//     if (remaining < 0) {

//         change =
//             Math.abs(remaining);


//         remaining = 0;

//     }



//     $("#paidAmount")
//         .text(
//             formatMoney(paid)
//         );



//     $("#remainingAmount")
//         .text(
//             formatMoney(remaining)
//         );



//     $("#changeAmount")
//         .text(
//             formatMoney(change)
//         );

// }

// function getGrandTotal() {

//     let value =
//         $("#grandTotal")
//             .text()
//             .replace(/[^\d.-]/g, '');


//     return Number(value) || 0;

// }

// On search for item
$("#txtItemSearch").on("keyup", function () {

    loadItems(
        selectedType,
        selectedCategory,
        $(this).val()
    );

});

function addPayment() {



    let payment =
    {

        id: Date.now(),

        amount:
            Number($("#paymentAmount").val()) || 0,


        paymentMethod:
            Number($("#paymentMethod").val()),


        paymentMethodName:
            $("#paymentMethod option:selected")
                .text(),


        referenceNo:
            $("#referenceNo").val(),


        paymentDate:
            $("#paymentDate").val()

    };

    

    if (payment.amount <= 0) {
        toastr.error(resources.invalidPaymentAmount);
        return;
    }

    if (!payment.paymentMethod || payment.paymentMethod <= 0) {
        toastr.error(resources.paymentMethodRequired);
        return;
    }

    if (!payment.paymentDate) {
        toastr.error(resources.invalidPaymentDate);
        return;
    }

    let remaining = calculateRemaingAmount();

    let amount =
        Number($("#paymentAmount").val()) || 0;

    if (amount > remaining) {
        toastr.error(resources.paymentExceedsInvoiceTotal);
        return;
    }


    console.log("payment -> ", payment)

    payments.push(payment);


    calculateInvoice();
    renderPayments();
    clearPaymentInputs();
}

function renderPayments() {

    let html = "";


    payments.forEach((payment, index) => {


        html += `

        <tr>


            <td>

            ${formatMoney(payment.amount)}

            </td>



            <td>

            ${payment.paymentMethodName}

            </td>



            <td>

            ${payment.referenceNo ?? ""}

            </td>



            <td>

            ${payment.paymentDate}

            </td>



            <td>


                <button
                class="btn btn-sm btn-danger btn-remove-payment"
                data-index="${index}">


                    <i class="fa fa-trash"></i>


                </button>


            </td>



        </tr>

        `;


    });



    $("#paymentsTable")
        .html(html);



    calculateTotalPaid();


}

function removePayment() {

    let index =
        $(this).data("index");


    payments.splice(index, 1);


    renderPayments();

}



function clearPaymentInputs() {

    $("#paymentAmount").val("");

    $("#paymentMethod").val("");

    $("#referenceNo").val("");

}

function calculateInvoiceTotals() {

    let subtotal =
        cart.reduce(
            (sum, item) =>
                sum + Number(item.total),
            0
        ); 


    let discount =
        Number($("#invoiceDiscount").val()) || 0;

    let tax =
        Number($("#invoiceTax").val()) || 0;

    let grandTotal =
        subtotal - discount + tax;

    return {

        subtotal: subtotal,

        discount: discount,

        tax: tax,

        total: grandTotal

    };


}

function validatePayment() {

    let totals =
        calculateInvoiceTotals();


    let paid =
        payments.reduce(
            (sum, p) =>
                sum + Number(p.amount),
            0
        );


    console.log("Total:", totals.total);
    console.log("Paid:", paid);


    return paid >= totals.total;

}

// Add Item To Cart
// function addToCart() {

//     let item =
//     {

//         id:
//             $("#ItemId").val(),


//         type:
//             $("#ItemType").val(),


//         name:
//             $("#ProductName").val()
//             ||
//             $("#SubscriptionName").val(),


//         price:
//             Number(
//                 $("#UnitPrice").val()
//                 ||
//                 $("#SubscriptionPrice").val()
//             ),


//         quantity:
//             Number(
//                 $("#Quantity").val()
//                 ||
//                 1
//             ),


//         startDate:
//             $("#StartDate").val(),


//         endDate:
//             $("#EndDate").val()


//     };


//     item.total =
//         item.price *
//         item.quantity;



//     cart.push(item);



//     renderCart();


//     calculateInvoice();

// }

// Format Currency
function formatMoney(value) {

    return new Intl.NumberFormat(
        "en-SD",
        {
            style: "currency",
            currency: "SDG"
        }
    ).format(value);

}

function getGrandTotal() {

    let value =
        $("#grandTotal")
            .text()
            .replace(/,/g, '');


    return Number(value) || 0;

}

function getInvoiceData() {

    return {

        memberId:
            $("#MemberId").val(),


        items:
            cart,


        payments:
            payments,


        discount:
            Number(
                $("#invoiceDiscount").val()
            ) || 0,


        tax:
            Number(
                $("#invoiceTax").val()
            ) || 0

    };

}

function completeSale() {

    if (!validatePayment()) {

        toastr.error(
            resources.paymentNotEnough
        );

        return;

    }

    console.log("cart -> ", cart)



    buildInvoiceModel();

    $("#invoiceForm").submit();

}

// function completeSale() {

//     if (!validatePayment()) {

//         toastr.error(
//             resources.paymentNotEnough
//         );

//         return;

//     }



//     let data =
//         getInvoiceData();



//     data.status =
//         "Completed";



//     saveInvoice(data);

// }


function saveInvoice(data) {

    $.ajax({

        url: "/SalesInvoice/Create",

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(data),


        success: function (result) {

            if (result.success) {

                invoiceId = result.id;


                toastr.success(
                    resources.saved
                );


                clearInvoice();

            }
            else {

                toastr.error(
                    result.message
                );

            }

        }

    });

}

$("#btnSaveDraft")
    .on(
        "click",
        function () {

            let data =
                getInvoiceData();


            data.status =
                "Draft";


            saveInvoice(data);

        });

function cancelInvoice() {

    Swal.fire({

        title:
            resources.confirm,


        text:
            resources.clearInvoice,


        icon:
            "warning",


        showCancelButton: true


    })
        .then((result) => {


            if (result.isConfirmed) {

                clearInvoice();

            }


        });


}

function clearInvoice() {

    cart = [];

    payments = [];


    renderCart();


    renderPayments();


    calculateInvoice();



    $("#MemberId")
        .val(null)
        .trigger("change");



    $("#invoiceDiscount")
        .val(0);


    $("#invoiceTax")
        .val(0);

}

function printInvoice() {

    if (!invoiceId) {

        toastr.warning(
            resources.saveFirst
        );

        return;

    }


    window.open(
        "/SalesInvoice/Print?id=" + invoiceId,
        "_blank"
    );

}

function buildInvoiceModel() {

    let container = $("#hiddenInvoiceData");

    container.empty();

    // Header
    buildInvoiceModelHeader(container);   

    // Details
    buildInvoiceModelDetails(container);  

    // Payments
    buildInvoiceModelPayments(container);


}

function buildInvoiceModelHeader(container) {

    container.append(createHidden(
        "SalesInvoice.MemberId",
        $("#MemberId").val()
    ));

    container.append(createHidden(
        "SalesInvoice.Discount",
        $("#invoiceDiscount").val()
    ));

    container.append(createHidden(
        "SalesInvoice.Tax",
        $("#invoiceTax").val()
    ));

    // container.append(createHidden(
    //     "SalesInvoice.Status",
    //     $("#InvoiceStatus").val()
    // ));

    // Draft = 1, 
    container.append(createHidden(
        "SalesInvoice.Status",
        1
    ));

}

function buildInvoiceModelDetails(container) {

    cart.forEach((item, index) => {

        container.append(createHidden(
            `SalesInvoice.Details[${index}].ItemId`,
            item.id
        ));

        container.append(createHidden(
            `SalesInvoice.Details[${index}].ItemType`,
            item.type
        ));

        container.append(createHidden(
            `SalesInvoice.Details[${index}].Quantity`,
            item.quantity
        ));

        container.append(createHidden(
            `SalesInvoice.Details[${index}].UnitPrice`,
            item.price
        ));

        container.append(createHidden(
            `SalesInvoice.Details[${index}].Discount`,
            item.discount ?? 0
        ));

        container.append(createHidden(
            `SalesInvoice.Details[${index}].Total`,
            item.total
        ));

        if (item.type === "Subscription") {

            //container.append(createHidden(
            //    `SalesInvoice.Details[${index}].StartDate`,
            //    item.startDate
            //));

            container.append(createHidden(
                `SalesInvoice.Details[${index}].SubscriptionStartDate`,
                item.startDate
            ));

            container.append(createHidden(
                `SalesInvoice.Details[${index}].EndDate`,
                item.endDate
            ));
        }

    });
}

function buildInvoiceModelPayments(container) {

    console.log('payments -> ', payments)
    payments.forEach((payment, index) => {

        container.append(createHidden(
            `SalesInvoice.Payments[${index}].PaymentMethod`,
            payment.paymentMethod
        ));

        container.append(createHidden(
            `SalesInvoice.Payments[${index}].Amount`,
            payment.amount
        ));

        container.append(createHidden(
            `SalesInvoice.Payments[${index}].ReferenceNo`,
            payment.referenceNo
        ));

        container.append(createHidden(
            `SalesInvoice.Payments[${index}].PaymentDate`,
            payment.paymentDate
        ));

    })
}

function createHidden(name, value) {

    return `<input type="hidden"
                   name="${name}"
                   value="${value ?? ""}" />`;

}
