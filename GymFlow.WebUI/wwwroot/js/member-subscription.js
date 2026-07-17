console.log("member-subscription.js loaded - v2");
let selectedDurationDays = 0;

function initializeMemberSubscription(options) {

    console.log("test");


    $('#memberSelect').select2({

        placeholder: options.memberPlaceholder,

        allowClear: true,

        ajax: {
            url: '/Members/Search',
            dataType: 'json',
            delay: 250,

            data: function (params) {

                return {
                    search: params.term
                };
            },

            //processResults: function (data) {
            //    return {
            //        results: data
            //    };
            //}

            processResults: function (data) {
                console.log("subscriptionTypeSelect data -> ", data);

                return {
                    results: data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.fullName
                        };
                    })
                };
            }
        }
    });

    $('#subscriptionTypeSelect').select2({

        placeholder: options.subscriptionPlaceholder,

        allowClear: true,

        ajax: {
            url: '/SubscriptionTypes/Search',
            dataType: 'json',
            delay: 250,

            data: function (params) {
                return {
                    search: params.term
                };
            },

            //processResults: function (data) {
            //    return {
            //        results: data
            //    };
            //}

            processResults: function (data) {

                console.log("subscriptionTypeSelect data -> ", data)

                return {
                    results: data.map(function (item) {
                        return {
                            id: item.id,
                            text: options.lang == "en" ? item.nameEn : item.nameAr,
                            price: item.price,
                            durationDays: item.durationDays,
                        };
                    })
                };
            }
        }
    });


    $('#subscriptionTypeSelect').on('select2:select', function (e) {

        var selected = e.params.data;

        selectedDurationDays = selected.durationDays;

        console.log(selected);

        // Set price
        $('#priceInput').val(selected.price);

        // Set start date to today
        let today = new Date();

        $('#startDateInput')
            .val(formatDate(today));

        calculateEndDate();
    });


}

$('#startDateInput').on('change', function () {
    calculateEndDate();

});

function formatDate(date) {

    var year = date.getFullYear();

    var month = String(date.getMonth() + 1)
        .padStart(2, '0');

    var day = String(date.getDate())
        .padStart(2, '0');


    return `${year}-${month}-${day}`;
}

function calculateEndDate() {

    let startDateValue = $('#startDateInput').val();


    if (!startDateValue || selectedDurationDays <= 0) {
        return;
    }


    let endDate = new Date(startDateValue);


    endDate.setDate(
        endDate.getDate() + selectedDurationDays
    );


    $('#endDateInput')
        .val(formatDate(endDate));

}