

$(document).ready(function () {
    const today = new Date().toISOString().split('T')[0];
let selectedTime = '';
let selectedCategory = null;
let selectedDate = today;

    $('.zyka-type-card').on('click', function () {

        $('.zyka-type-card').removeClass('active').css('border', 'none');

        $(this).addClass('active').css('border', '2px solid #ffffff');

        selectedCategory = Number($(this).data('type'));

        const seats = Number($(this).data('seats'));
        const min = Number($(this).data('min'));
        const max = Number($(this).data('max'));

        $('#seatingCapacity').val(seats).attr('min', min).attr('max', max);
        $('#seatingSection').fadeIn();

        $('#seatingCapacity').on('input', function () {
            const value = parseInt($(this).val());
            const min = parseInt($(this).attr('min'));
            const max = parseInt($(this).attr('max'));

            if (value > max) {
                $(this).val(max);
            } else if (value < min && $(this).val() !== "") {
                $(this).val(min);
            }
        });

        tryFetchAvailability();
    });

    $('#bookingDate').attr('min', today).val(today)
        .on('click', function () {
            if (typeof this.showPicker === 'function') {
                this.showPicker();
            }
        })
        .on('keydown paste', function (e) {
            e.preventDefault();
        })
        .on('change', function () {
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

        const $mobile = $('#mobileNumber');
        const $whatsapp = $('#whatsappNumber');
        const $checkbox = $('#sameAsMobile');

        // 1. Handle the Checkbox click
        $checkbox.on('change', function () {
            if (this.checked) {
                $whatsapp.val($mobile.val());
                $whatsapp.prop('readonly', true); // Optional: prevent editing while synced
            } else {
                $whatsapp.prop('readonly', false);
            }
        });

        // 2. Sync in real-time if checkbox is checked
        $mobile.on('input', function () {
            if ($checkbox.is(':checked')) {
                $whatsapp.val($(this).val());
            }
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


