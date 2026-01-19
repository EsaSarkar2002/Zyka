//let selectedTime = '';
//let selectedCategory = null;
//let selectedDate = '';

//$(document).ready(function () {
//    // Set min date = today
//    const today = new Date().toISOString().split('T')[0];
//    $('#bookingDate').attr('min', today);

//    $('.zyka-type-card').on('click', function () {
//        $('.zyka-type-card')
//            .removeClass('active')
//            .css('border', 'none');

//        $(this)
//            .addClass('active')
//            .css('border', '2px solid #1a4d2e');

//        selectedCategory = Number($(this).data('type'));
//        const seats = Number($(this).data('seats'));

//        const min = Number($(this).data('min'));
//        const max = Number($(this).data('max'));

//        $('#seatingCapacity').val(seats).attr('min', min).attr('max', max);
//        $('#seatingSection').fadeIn();
//        $('#typeError').hide();

//        console.log('type selected:', selectedCategory);
//        tryFetchAvailability();
//    });

//    $('#bookingDate').on('change', function () {
//        selectedDate = $(this).val();
//        console.log('date selected:', selectedDate);
//        tryFetchAvailability();
//    });

//    function tryFetchAvailability() {
//        const date = $('#bookingDate').val();
//        console.log('tryFetchAvailability start', { date: date, selectedCategory: selectedCategory });

//        if (!date || selectedCategory == null) {
//            console.log('tryFetchAvailability aborted: missing date or category', { date, selectedCategory });
//            return;
//        }

//        $.ajax({
//            url: '/Customer/GetAvailableTimeSlots',
//            type: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify({ reservationDate: date, category: selectedCategory }),
//            success: function (response) {
//                console.log('GetAvailableTimeSlots success:', response);
//                updateTimeSlots(response);
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//                console.error('GetAvailableTimeSlots error:', textStatus, errorThrown, jqXHR);
//            }
//        });
//    }

//    function updateTimeSlots(data) {
//        console.log('Updating slots with:', data);
//        $('.zyka-time-btn').each(function () {
//            const slotId = Number($(this).data('timeslotid'));
//            const slot = data.find(x => x.timeSlotId === slotId);
//            if (!slot || slot.isAvailabel === false) {
//                $(this).addClass('disabled').prop('disabled', true).removeClass('btn-success').addClass('btn-outline-secondary');
//            }
//            else {
//                $(this).removeClass('disabled').prop('disabled', false);
//            }
//        });
//    }

//    /* TIME SLOT SELECTION */
//    $('.zyka-time-btn').on('click', function () {
//        if ($(this).hasClass('disabled'))
//        {
//            return;
//        }
//        $('.zyka-time-btn')
//            .removeClass('btn-success text-white')
//            .addClass('btn-outline-secondary');

//        $(this)
//            .removeClass('btn-outline-secondary')
//            .addClass('btn-success text-white');

//        selectedTime = $(this).data('timeslotid');
//        console.log(selectedTime);
//    });




//    /* FORM SUBMIT */
//    $('#bookingForm').on('submit', function (e) {
//        e.preventDefault();
//        console.log({
//            reservationDate: selectedDate,
//            timeSlotId: selectedTime,
//            category: selectedCategory,
//            numberOfGuests: parseInt($('#seatingCapacity').val()),
//            fullName: $('#customerName').val(),
//            mobileNumber: $('#mobileNumber').val(),
//            whatsappNumber: $('#whatsappNumber').val()
//        });
//        if (selectedCategory==null) {
//            alert('Please select booking type');
//            return;
//        }
//        if (!$('#bookingDate').val()) {
//            alert('Please select date');
//            return;
//        }

//        if (!selectedTime) {
//            alert('Please select time slot');
//            return;
//        }

//        if (!$('#customerName').val().trim()) {
//            alert('Please enter name');
//            return;
//        }

//        $.ajax({
//            url: '/Customer/CreateReservation',
//            type: 'Post',
//            contentType: 'application/json',
//            data: JSON.stringify({
//                reservationDate: selectedDate,
//                timeSlotId: selectedTime,
//                category: parseInt(selectedCategory),
//                numberOfGuests: parseInt($('#seatingCapacity').val()),
//                fullName: $('#customerName').val(),
//                mobileNumber: $('#mobileNumber').val(),
//                whatsappNumber: $('#whatsappNumber').val()
//            }),
//            success: function () {
//                window.location.href = '/Customer/Confirmation';
//            },
//            error: function (err) {
//                alert(err.responseText);
//            }
//        });
//    });

//});

let selectedTime = '';
let selectedCategory = null;
let selectedDate = '';

$(document).ready(function () {

    const today = new Date().toISOString().split('T')[0];
    $('#bookingDate').attr('min', today);

    $('.zyka-type-card').on('click', function () {

        $('.zyka-type-card').removeClass('active').css('border', 'none');

        $(this).addClass('active').css('border', '2px solid #1a4d2e');

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
                $('.zyka-time-btn').each(function () {
                    const slotId = Number($(this).data('timeslotid'));
                    const slot = response.find(x => x.timeSlotId === slotId);

                    if (!slot || slot.isAvailable === false) {
                        $(this).prop('disabled', true).addClass('disabled');
                    } else {
                        $(this).prop('disabled', false).removeClass('disabled');
                    }
                });
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


