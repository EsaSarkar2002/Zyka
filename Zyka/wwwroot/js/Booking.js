

let selectedTime = '';
let selectedCategory = null;
let selectedDate = '';

$(document).ready(function () {

    const today = new Date().toISOString().split('T')[0];
    $('#bookingDate').attr('min', today);

    $('.zyka-type-card').on('click', function () {

        $('.zyka-type-card').removeClass('active').css('border', 'none');

        $(this).addClass('active').css('border', '2px solid #ffffff');

        selectedCategory = Number($(this).data('type'));

        const seats = Number($(this).data('seats'));
        const min = Number($(this).data('min'));
        const max = Number($(this).data('max'));

        $('#seatingCapacity').val(seats).attr('min', min).attr('max', max);
        $('#seatingSection').fadeIn();

        tryFetchAvailability();
    });

    $('#bookingDate').on('change', function () {
        selectedDate = $(this).val();
        tryFetchAvailability();
    });

    function tryFetchAvailability() {
        if (!selectedDate || selectedCategory === null) return;

        $.ajax({
            url: '/Customer/GetAvailableTimeSlots',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                reservationDate: selectedDate, 
                category: selectedCategory
            }),
            success: function (response) {

                
                console.log("Available slots API response:", response);

                // 🔴 SAFETY: if response is empty or not an array
                if (!Array.isArray(response) || response.length === 0) {
                    $('.zyka-time-btn')
                        .prop('disabled', true)
                        .addClass('disabled');
                    return;
                }

                // 🔑 Normalize response into pure number IDs
                const availableSlotIds = response.map(Number);

                console.log("Normalized slot IDs:", availableSlotIds);

                $('.zyka-time-btn').each(function () {
                    const slotId = Number($(this).data('timeslotid'));

                    if (availableSlotIds.includes(slotId)) {
                        $(this)
                            .prop('disabled', false)
                            .removeClass('disabled');
                    } else {
                        $(this)
                            .prop('disabled', true)
                            .addClass('disabled');
                    }
                });
            },
            error: function (err) {
                console.error("GetAvailableTimeSlots failed:", err);
                $('.zyka-time-btn')
                    .prop('disabled', true)
                    .addClass('disabled');
            }
        });
    }



    $('.zyka-time-btn').on('click', function () {

        if ($(this).hasClass('disabled')) return;

        $('.zyka-time-btn').removeClass('btn-success text-white')
            .addClass('btn-outline-secondary');

        $(this).addClass('btn-success text-white')
            .removeClass('btn-outline-secondary');

        selectedTime = $(this).data('timeslotid');
    });

    $('#bookingForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Customer/CreateReservation',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                reservationDate: selectedDate,
                timeSlotId: selectedTime,
                category: selectedCategory,
                numberOfGuests: parseInt($('#seatingCapacity').val()),
                fullName: $('#customerName').val(),
                mobileNumber: $('#mobileNumber').val(),
                whatsappNumber: $('#whatsappNumber').val()
            }),
            success: function (res) {
                console.log(res);
                // 🔐 Save data for Payment page
                sessionStorage.setItem('currentReservation', JSON.stringify({
                    reservationId: res.reservationId,
                    categoryText: $('.zyka-type-card.active span').text(),
                    numberOfGuests: parseInt($('#seatingCapacity').val()),
                    reservationDate: selectedDate,
                    timeText: $('.zyka-time-btn.btn-success').text(),
                    fullName: $('#customerName').val()
                }));

                window.location.href = '/Customer/Payment';
            },
            error: function (err) {
                alert(err.responseText);
            }
        });
    });
});


