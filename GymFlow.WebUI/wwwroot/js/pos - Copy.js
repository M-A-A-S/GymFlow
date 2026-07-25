let cart = [];

let payments = [];

let products = [];

let subscriptions = [];

let selectedType = "Product";

let selectedCategory = 0;

$(document).ready(function () {


    initializeSelect2();


    initializeEvents();


    calculateInvoice();


});

// Select2 Member Search
function initializeSelect2() {

    $("#MemberId").select2({

        width: "100%",

        placeholder:
            resources.searchMember,


        allowClear: true,


        ajax: {


            url:
                "/Member/Search",


            dataType: "json",


            delay: 300,


            data: function (params) {

                return {

                    term: params.term

                };

            },


            processResults: function (data) {


                return {

                    results: data

                };

            }


        }


    });

}


// Events Initialization
function initializeEvents() {

    $(".item-type")
        .on("click", changeItemType);



    $(".category-btn")
        .on("click", changeCategory);



    $("#txtItemSearch")
        .on("keyup", filterItems);



    $("#btnAddToCart")
        .on("click", addToCart);



    $("#btnCompleteSale")
        .on("click", completeSale);



    $("#btnAddPayment")
        .on("click", addPayment);



}

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
                x.name
                    .toLowerCase()
                    .includes(search)
            );

    }



    renderItems(data);

}

// Render Item Cards
function renderItems(items) {

    let html = "";


    items.forEach(item => {


        html += `

        <div class="col-md-4 mb-3">


            <div class="card product-card">


                <div class="card-body">


                    <h6>
                    ${item.name}
                    </h6>


                    <span class="badge bg-primary">

                    ${formatMoney(item.price)}

                    </span>


                </div>


                <div class="card-footer">


                <button
                class="btn btn-success w-100 btn-add-item"
                data-id="${item.id}">


                ${resources.add}


                </button>


                </div>


            </div>


        </div>

        `;


    });



    $("#itemContainer")
        .html(html);

}

// Add Item To Cart
function addToCart() {

    let item =
    {

        id:
            $("#ItemId").val(),


        type:
            $("#ItemType").val(),


        name:
            $("#ProductName").val()
            ||
            $("#SubscriptionName").val(),


        price:
            Number(
                $("#UnitPrice").val()
                ||
                $("#SubscriptionPrice").val()
            ),


        quantity:
            Number(
                $("#Quantity").val()
                ||
                1
            ),


        startDate:
            $("#StartDate").val(),


        endDate:
            $("#EndDate").val()


    };


    item.total =
        item.price *
        item.quantity;



    cart.push(item);



    renderCart();


    calculateInvoice();

}

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

// Complete Sale

function completeSale() {


    if (!validatePayment()) {

        showError(
            resources.paymentRequired
        );

        return;

    }



    let model =
    {

        memberId:
            $("#MemberId").val(),


        items:
            cart,


        payments:
            payments,


        discount:
            Number(
                $("#invoiceDiscount").val()
            ),


        tax:
            Number(
                $("#invoiceTax").val()
            )

    };



    $.ajax({

        url:
            "/SalesInvoice/Create",


        type: "POST",


        contentType:
            "application/json",


        data:
            JSON.stringify(model),


        success: function (result) {


            if (result.success) {

                showSuccess(
                    resources.saved
                );


                clearInvoice();

            }


        }

    });


}





