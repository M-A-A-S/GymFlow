let selectedDurationDays = 0;

//  ================= member-subscription  =================
function initializeMemberSubscription(options) {


    // ================= Members =================
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

            processResults: function (data) {

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

    // ================= Subscription Types =================
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

            processResults: function (data) {

                return {
                    results: data.map(function (item) {
                        return {
                            id: item.id,
                            text: options.lang == "en"
                                ? item.nameEn
                                : item.nameAr,
                            price: item.price,
                            durationDays: item.durationDays,
                        };
                    })
                };
            }
        }
    });


    // ================= Subscription Type Changed =================
    $('#subscriptionTypeSelect').on('select2:select', function (e) {

        var selected = e.params.data;

        selectedDurationDays = selected.durationDays;

        // Set price
        $('#priceInput').val(selected.price);

        /// Set start date only if it's empty
        let startDate = $('#startDateInput').val();

        if (!startDate) {
            let today = new Date();
            $('#startDateInput').val(formatDate(today));
        }

        calculateEndDate();
    });

    // ================= Edit Mode =================
    if (options.memberId) {

        console.log("member -> ", options.memberName)

        let option = new Option(
            options.memberName,
            options.memberId,
            true,
            true
        );

        $('#memberSelect')
            .append(option)
            .trigger('change');
    }

    if (options.subscriptionTypeId) {

        let option = new Option(
            options.subscriptionTypeName,
            options.subscriptionTypeId,
            true,
            true
        );

        $('#subscriptionTypeSelect')
            .append(option)
            .trigger('change');

        selectedDurationDays = parseInt(options.durationDays || 0);

        $('#priceInput').val(options.price);

        calculateEndDate();
    }
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

//  ================= trainer-schedule  =================
// TODO: Put it in seprate file
function initializeTrainerSchedule(options) {
    // ================= Trainer =================
    $('#trainerSelect').select2({
        placeholder: options.trainerPlaceholder,
        allowClear: true,
    });

    // ================= Edit Mode =================
    if (options.trainerId) {

        $('#trainerSelect')
            .val(options.trainerId)
            .trigger('change');
    }
}