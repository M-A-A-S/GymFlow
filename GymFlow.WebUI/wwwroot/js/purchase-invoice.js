// ======================================================
// Purchase Invoice
// ======================================================

let detailIndex = 0;
let paymentIndex = 0;

const messages = window.GymFlow.PurchaseInvoice.messages;

$(document).ready(function () {

    initializeSelect2();

    initializeProducts();

    initializePayments();

    calculateInvoiceTotal();

});


// ======================================================
// Select2
// ======================================================

function initializeSelect2() {

    $("#supplierSelect").select2({
        allowClear: true,
        width: "100%"
    });

    $("#productSelect").select2({
        allowClear: true,
        width: "100%"
    });

}


// ======================================================
// Format Money
// ======================================================

function formatMoney(value) {

    return Number(value).toLocaleString(
        document.documentElement.lang || "en-US",
        {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });

}


// ======================================================
// Products
// ======================================================

function initializeProducts() {

    detailIndex = $("#detailsTable tr").length;

    $("#addProduct").on("click", addProduct);

    $(document).on(
        "click",
        ".remove-product",
        removeProduct);

    $(document).on(
        "input",
        ".quantity-input,.price-input",
        updateProductRow);

}


// ======================================================
// Add Product
// ======================================================

function addProduct() {

    let product = getSelectedProduct();

    if (!validateProduct(product))
        return;

    let existingRow = findExistingProduct(product.productId);

    if (existingRow.length > 0) {

        updateExistingProduct(existingRow, product);
        return;

    }

    appendProductRow(product);

}


// ======================================================
// Read Inputs
// ======================================================

function getSelectedProduct() {

    return {

        productId: $("#productSelect").val(),

        productName:
            $("#productSelect option:selected")
                .data("name"),

        quantity:
            parseFloat($("#quantityInput").val()),

        price:
            parseFloat($("#priceInput").val())

    };

}


// ======================================================
// Validation
// ======================================================

function validateProduct(product) {

    if (!product.productId) {

        showError(messages.selectProduct);
        return false;

    }

    if (!product.quantity || product.quantity <= 0) {

        showError(messages.invalidQuantity);
        return false;

    }

    if (!product.price || product.price <= 0) {

        showError(messages.invalidPrice);
        return false;

    }

    return true;

}


// ======================================================
// Existing Product
// ======================================================

function findExistingProduct(productId) {

    return $(
        `#detailsTable tr[data-product-id='${productId}']`
    );

}


// ======================================================
// Update Existing Product
// ======================================================

function updateExistingProduct(row, product) {

    let quantityInput =
        row.find(".quantity-input");

    let currentQuantity =
        parseFloat(quantityInput.val()) || 0;

    quantityInput.val(
        currentQuantity + product.quantity
    );

    row.find(".price-input")
        .val(product.price);

    updateRowTotal(row);

    calculateInvoiceTotal();

    clearProductInputs();

}


// ======================================================
// Append New Row
// ======================================================

function appendProductRow(product) {

    $("#detailsTable")
        .append(buildProductRow(product));

    detailIndex++;

    calculateInvoiceTotal();

    clearProductInputs();

}

// ======================================================
// Build Product Row
// ======================================================

function buildProductRow(product) {

    let total = product.quantity * product.price;

    return `

<tr data-product-id="${product.productId}">

    <td>

        ${product.productName}

        <input
            type="hidden"
            name="PurchaseInvoice.PurchaseDetails[${detailIndex}].ProductId"
            value="${product.productId}" />

    </td>

    <td>

        <input
            type="number"
            class="form-control quantity-input"
            name="PurchaseInvoice.PurchaseDetails[${detailIndex}].Quantity"
            value="${product.quantity}"
            min="1" />

    </td>

    <td>

        <input
            type="number"
            class="form-control price-input"
            step="0.01"
            name="PurchaseInvoice.PurchaseDetails[${detailIndex}].UnitPrice"
            value="${product.price}"
            min="0.01" />

    </td>

    <td class="row-total fw-bold text-primary">

        ${formatMoney(total)}

    </td>

    <td>

        <button
            type="button"
            class="btn btn-sm btn-outline-danger remove-product">

            <i class="fa fa-trash"></i>

        </button>

    </td>

</tr>

`;

}


// ======================================================
// Update Product Row
// ======================================================

function updateProductRow() {

    let row =
        $(this).closest("tr");

    updateRowTotal(row);

    calculateInvoiceTotal();

}


// ======================================================
// Update Row Total
// ======================================================

function updateRowTotal(row) {

    let quantity =
        parseFloat(
            row.find(".quantity-input").val()
        ) || 0;

    let price =
        parseFloat(
            row.find(".price-input").val()
        ) || 0;

    let total =
        quantity * price;

    row.find(".row-total")
        .text(formatMoney(total));

}


// ======================================================
// Remove Product
// ======================================================

function removeProduct() {

    $(this)
        .closest("tr")
        .remove();

    calculateInvoiceTotal();

}


// ======================================================
// Clear Product Inputs
// ======================================================

function clearProductInputs() {

    $("#productSelect")
        .val(null)
        .trigger("change");

    $("#quantityInput")
        .val(1);

    $("#priceInput")
        .val("");

}


// ======================================================
// Calculate Invoice Total
// ======================================================

function calculateInvoiceTotal() {

    let total = 0;

    $("#detailsTable tr").each(function () {

        let quantity =
            parseFloat(
                $(this)
                    .find(".quantity-input")
                    .val()
            ) || 0;

        let price =
            parseFloat(
                $(this)
                    .find(".price-input")
                    .val()
            ) || 0;

        total += quantity * price;

    });

    $("#invoiceTotal")
        .text(formatMoney(total));

}

// ======================================================
// Payments
// ======================================================

function initializePayments() {

    paymentIndex = $("#paymentsTable tr").length;

    $("#addPayment").on(
        "click",
        addPayment);

    $(document).on(
        "click",
        ".remove-payment",
        removePayment);

}


// ======================================================
// Add Payment
// ======================================================

function addPayment() {

    let payment = getPayment();

    if (!validatePayment(payment))
        return;

    appendPaymentRow(payment);

}


// ======================================================
// Read Payment Inputs
// ======================================================

function getPayment() {

    return {

        amount:
            parseFloat($("#paymentAmount").val()),

        paymentMethod:
            $("#paymentMethod").val(),

        paymentMethodText:
            $("#paymentMethod option:selected").text(),

        referenceNo:
            $("#referenceNo").val(),          

        paymentDate:
            $("#paymentDate").val()

    };

}


// ======================================================
// Validate Payment
// ======================================================

function validatePayment(payment) {

    if (!payment.amount || payment.amount <= 0) {

        showError(messages.invalidAmount);

        return false;

    }

    if (!payment.paymentMethod) {

        showError(messages.selectPaymentMethod);

        return false;

    }

    if (!payment.paymentDate) {

        showError(messages.invalidPaymentDate);

        return false;

    }

    return true;

}


// ======================================================
// Append Payment
// ======================================================

function appendPaymentRow(payment) {

    $("#paymentsTable")
        .append(buildPaymentRow(payment));

    paymentIndex++;

    clearPaymentInputs();

    calculatePaidAmount();

}


// ======================================================
// Build Payment Row
// ======================================================

function buildPaymentRow(payment) {

    return `

<tr>

    <td>

        ${formatMoney(payment.amount)}

        <input
            type="hidden"
            name="PurchaseInvoice.PurchasePayments[${paymentIndex}].Amount"
            value="${payment.amount}" />

    </td>

    <td>

        ${payment.paymentMethodText}

        <input
            type="hidden"
            name="PurchaseInvoice.PurchasePayments[${paymentIndex}].PaymentMethod"
            value="${payment.paymentMethod}" />

    </td>

        <td>

        ${payment.referenceNo}

        <input
            type="hidden"
            name="PurchaseInvoice.PurchasePayments[${paymentIndex}].ReferenceNo"
            value="${payment.referenceNo}" />

    </td>

    <td>

        ${payment.paymentDate}

        <input
            type="hidden"
            name="PurchaseInvoice.PurchasePayments[${paymentIndex}].PaymentDate"
            value="${payment.paymentDate}" />

    </td>

    <td class="text-center">

        <button
            type="button"
            class="btn btn-sm btn-outline-danger remove-payment">

            <i class="fa fa-trash"></i>

        </button>

    </td>

</tr>

`;

}


// ======================================================
// Remove Payment
// ======================================================

function removePayment() {

    $(this)
        .closest("tr")
        .remove();

    calculatePaidAmount();

}


// ======================================================
// Clear Payment Inputs
// ======================================================

function clearPaymentInputs() {

    $("#paymentAmount").val("");

    $("#paymentMethod")
        .val("")
        .trigger("change");

    $("#paymentDate")
        .val(new Date().toISOString().split("T")[0]);

}

// ======================================================
// Paid Amount
// ======================================================

function calculatePaidAmount() {

    let paid = 0;

    $("#paymentsTable tr").each(function () {

        let amount =
            parseFloat(
                $(this)
                    .find("input[name$='.Amount']")
                    .val()
            ) || 0;

        paid += amount;

    });

    $("#paidAmount").text(formatMoney(paid));

    calculateRemainingAmount();

}


// ======================================================
// Remaining Amount
// ======================================================

function calculateRemainingAmount() {

    let invoiceTotal = getInvoiceTotal();

    let paid = getPaidAmount();

    let remaining = invoiceTotal - paid;

    if (remaining < 0)
        remaining = 0;

    $("#remainingAmount")
        .text(formatMoney(remaining));

}


// ======================================================
// Get Invoice Total
// ======================================================

function getInvoiceTotal() {

    let total = 0;

    $("#detailsTable tr").each(function () {

        let quantity =
            parseFloat(
                $(this)
                    .find(".quantity-input")
                    .val()
            ) || 0;

        let price =
            parseFloat(
                $(this)
                    .find(".price-input")
                    .val()
            ) || 0;

        total += quantity * price;

    });

    return total;

}


// ======================================================
// Get Paid Amount
// ======================================================

function getPaidAmount() {

    let total = 0;

    $("#paymentsTable tr").each(function () {

        total +=
            parseFloat(
                $(this)
                    .find("input[name$='.Amount']")
                    .val()
            ) || 0;

    });

    return total;

}


// ======================================================
// Override Invoice Total Calculation
// ======================================================

function calculateInvoiceTotal() {

    let total = getInvoiceTotal();

    $("#invoiceTotal")
        .text(formatMoney(total));

    calculateRemainingAmount();

}


// ======================================================
// Initialize Existing Data (Edit Mode)
// ======================================================

function initializeExistingRows() {

    $("#detailsTable tr").each(function () {

        updateRowTotal($(this));

    });

    calculateInvoiceTotal();

    calculatePaidAmount();

}


// ======================================================
// Reset Form
// ======================================================

function resetProductForm() {

    clearProductInputs();

}

function resetPaymentForm() {

    clearPaymentInputs();

}


// ======================================================
// Optional Helper
// ======================================================

function showError(message) {

    //alert(message);
    toastr.error(message);

}