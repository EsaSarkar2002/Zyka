$(document).ready(function () {
    // 1. Security Check
    const bookingData = JSON.parse(localStorage.getItem('currentBooking'));
    if (!bookingData) {
        window.location.href = '/Customer/Booking';
        return;
    }

    // 2. Display Data
    $('#summaryType').text(bookingData.bookingType);
    $('#summaryGuests').text(bookingData.seatingCapacity + ' people');
    $('#summaryDate').text(new Date(bookingData.date).toLocaleDateString());
    $('#summaryTime').text(bookingData.timeSlot);
    $('#summaryName').text(bookingData.customerName);

    // 3. Amount Logic
    const baseRates = { date: 499, family: 999, meeting: 2499, party: 4999 };
    const amount = baseRates[bookingData.bookingType] || 400;
    $('#baseAmount, #totalAmount').text('₹' + amount);

    // 4. Method Selection
    $('.payment-method').click(function () {
        $('.payment-method').removeClass('active');
        $(this).addClass('active');
        const method = $(this).data('method');

        $('#upiForm, #cardForm').hide();
        if (method === 'upi') $('#upiForm').show();
        else $('#cardForm').show();

        $('#payBtn').fadeIn();
    });

    // 5. Process Payment
    $('#payBtn').click(function () {
        const btn = $(this);
        btn.prop('disabled', true);
        $('#payBtnText').text('Processing...');
        $('#payBtnSpinner').show();

        setTimeout(function () {
            const confirmation = {
                ...bookingData,
                bookingId: 'BK' + Date.now(),
                status: 'confirmed'
            };

            // Save for Confirmation page
            localStorage.setItem('lastBooking', JSON.stringify(confirmation));

            // Redirect to Confirmation
            window.location.href = '/Customer/Confirmation';
        }, 2000);
    });
});