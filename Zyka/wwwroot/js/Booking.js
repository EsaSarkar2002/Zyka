let selectedType = '';
let selectedTime = '';

$(document).ready(function () {
    // 1. RELOAD FIX: Temporary bypass for testing
    // Agar aap login nahi ho phir bhi page khulega
    if (!localStorage.getItem('userType')) {
        localStorage.setItem('userType', 'customer');
    }

    // Set min date to today
    const today = new Date().toISOString().split('T')[0];
    $('#bookingDate').attr('min', today);

    // Booking type selection
    $('.booking-type-card').click(function () {
        $('.booking-type-card').removeClass('active border-primary').css('border', 'none');
        $(this).addClass('active').css('border', '2px solid #1a4d2e');

        selectedType = $(this).data('type');
        const seats = Number($(this).data('seats'));
        const min = Number($(this).data('min'));
        const max = Number($(this).data('max'));

        $('#seatingCapacity').val(seats).attr('min', min).attr('max', max);
        $('#seatingSection').fadeIn();
        $('#typeError').hide();
    });

    // Time slot selection
    $('.time-slot').click(function () {
        $('.time-slot').removeClass('btn-success text-white').addClass('btn-outline-secondary');
        $(this).removeClass('btn-outline-secondary').addClass('btn-success text-white');
        selectedTime = $(this).data('time');
        $('#timeError').hide();
    });

    // Process to Payment Button Logic
    // Form submit ki jagah hum direct ID use karenge taaki button pakka chale
    $('#submitBtn, #bookingForm').on('click submit', function (e) {
        // Agar submit event hai toh default action roko
        if (e.type === 'submit') e.preventDefault();

        // Agar click event hai aur wo form submit wala button nahi hai toh manual handle karo
        if (e.target.id === 'submitBtn' || e.type === 'submit') {

            let isValid = true;
            if (!selectedType) { alert('Please select a booking type'); isValid = false; }
            if (!$('#bookingDate').val()) { $('#bookingDate').addClass('is-invalid'); isValid = false; }
            if (!selectedTime) { alert('Please select a time slot'); isValid = false; }
            if (!$('#customerName').val()) { $('#customerName').addClass('is-invalid'); isValid = false; }

            if (!isValid) return false;

            const bookingData = {
                bookingType: selectedType,
                seatingCapacity: $('#seatingCapacity').val(),
                date: $('#bookingDate').val(),
                timeSlot: selectedTime,
                customerName: $('#customerName').val().trim(),
                mobileNumber: $('#mobileNumber').val(),
                whatsappNumber: $('#whatsappNumber').val()
            };

            console.log("Saving data...", bookingData);
            localStorage.setItem('currentBooking', JSON.stringify(bookingData));

            // REDIRECT FIX: Seedha Payment page par bhejo
            window.location.href = '/Customer/Payment';
            return false;
        }
    });
});