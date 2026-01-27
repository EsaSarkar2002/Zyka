//////$(document).ready(function () {
//////    // 1. Security Check
//////    const bookingData = JSON.parse(localStorage.getItem('currentBooking'));
//////    if (!bookingData) {
//////        window.location.href = '/Customer/Booking';
//////        return;
//////    }

//////    // 2. Display Data
//////    $('#summaryType').text(bookingData.bookingType);
//////    $('#summaryGuests').text(bookingData.seatingCapacity + ' people');
//////    $('#summaryDate').text(new Date(bookingData.date).toLocaleDateString());
//////    $('#summaryTime').text(bookingData.timeSlot);
//////    $('#summaryName').text(bookingData.customerName);

//////    // 3. Amount Logic
//////    const baseRates = { date: 499, family: 999, meeting: 2499, party: 4999 };
//////    const amount = baseRates[bookingData.bookingType] || 400;
//////    $('#baseAmount, #totalAmount').text('₹' + amount);

//////    // 4. Method Selection
//////    $('.payment-method').click(function () {
//////        $('.payment-method').removeClass('active');
//////        $(this).addClass('active');
//////        const method = $(this).data('method');

//////        $('#upiForm, #cardForm').hide();
//////        if (method === 'upi') $('#upiForm').show();
//////        else $('#cardForm').show();

//////        $('#payBtn').fadeIn();
//////    });

//////    // 5. Process Payment
//////    $('#payBtn').click(function () {
//////        const btn = $(this);
//////        btn.prop('disabled', true);
//////        $('#payBtnText').text('Processing...');
//////        $('#payBtnSpinner').show();

//////        setTimeout(function () {
//////            const confirmation = {
//////                ...bookingData,
//////                bookingId: 'BK' + Date.now(),
//////                status: 'confirmed'
//////            };

//////            // Save for Confirmation page
//////            localStorage.setItem('lastBooking', JSON.stringify(confirmation));

//////            // Redirect to Confirmation
//////            window.location.href = '/Customer/Confirmation';
//////        }, 2000);
//////    });
//////});



////$(document).ready(function () {

////    const bookingData = JSON.parse(sessionStorage.getItem('currentReservation'));

////    if (!bookingData || !bookingData.reservationId) {
////        window.location.href = '/Customer/Reservation';
////        return;
////    }

////    // Populate summary
////    $('#summaryType').text(bookingData.categoryText);
////    $('#summaryGuests').text(bookingData.numberOfGuests + ' people');
////    $('#summaryDate').text(new Date(bookingData.reservationDate).toLocaleDateString());
////    $('#summaryTime').text(bookingData.timeText);
////    $('#summaryName').text(bookingData.fullName);

////    const amountMap = {
////        Date: 499,
////        Family: 999,
////        Meeting: 2499,
////        Celebration: 4999
////    };

////    const amount = amountMap[bookingData.categoryText] || 400;
////    $('#baseAmount, #totalAmount').text('₹' + amount);

////    let selectedMethod = null;

////    $('.payment-method').click(function () {
////        $('.payment-method').removeClass('active');
////        $(this).addClass('active');

////        selectedMethod = $(this).data('method');

////        $('#upiForm, #cardForm').hide();
////        if (selectedMethod === 'upi') $('#upiForm').show();
////        if (selectedMethod === 'card') $('#cardForm').show();

////        $('#payBtn').fadeIn();
////    });

////    $('#payBtn').click(function () {

////        if (!selectedMethod) {
////            alert('Please select payment method');
////            return;
////        }

////        $.post('/Customer/CreatePayment', {
////            reservationId: bookingData.reservationId,
////            method: selectedMethod === 'upi' ? 0 : 1
////        })
////            .done(function () {
////                sessionStorage.removeItem('currentReservation');
////                window.location.href = '/Customer/Confirmation';
////            })
////            .fail(function (err) {
////                alert(err.responseText);
////            });
////    });
////});












































////$(document).ready(function () {

////    // 1. Security Check

////    const bookingData = JSON.parse(localStorage.getItem('currentBooking'));

////    if (!bookingData) {

////        window.location.href = '/Customer/Booking';

////        return;

////    }

////    // 2. Display Data

////    $('#summaryType').text(bookingData.bookingType);

////    $('#summaryGuests').text(bookingData.seatingCapacity + ' people');

////    $('#summaryDate').text(new Date(bookingData.date).toLocaleDateString());

////    $('#summaryTime').text(bookingData.timeSlot);

////    $('#summaryName').text(bookingData.customerName);

////    // 3. Amount Logic

////    const baseRates = { date: 499, family: 999, meeting: 2499, party: 4999 };

////    const amount = baseRates[bookingData.bookingType] || 400;

////    $('#baseAmount, #totalAmount').text('₹' + amount);

////    // 4. Method Selection

////    $('.payment-method').click(function () {

////        $('.payment-method').removeClass('active');

////        $(this).addClass('active');

////        const method = $(this).data('method');

////        $('#upiForm, #cardForm').hide();

////        if (method === 'upi') $('#upiForm').show();

////        else $('#cardForm').show();

////        $('#payBtn').fadeIn();

////    });

////    // 5. Process Payment

////    $('#payBtn').click(function () {

////        const btn = $(this);

////        btn.prop('disabled', true);

////        $('#payBtnText').text('Processing...');

////        $('#payBtnSpinner').show();

////        setTimeout(function () {

////            const confirmation = {

////                ...bookingData,

////                bookingId: 'BK' + Date.now(),

////                status: 'confirmed'

////            };

////            // Save for Confirmation page

////            localStorage.setItem('lastBooking', JSON.stringify(confirmation));

////            // Redirect to Confirmation

////            window.location.href = '/Customer/Confirmation';

////        }, 2000);

////    });

////});
























//$(document).ready(function () {

//    const bookingData = JSON.parse(sessionStorage.getItem('currentReservation'));

//    if (!bookingData || !bookingData.reservationId) {

//        window.location.href = '/Customer/Reservation';

//        return;

//    }

//    // Populate summary (if Payment page shows a summary)

//    $('#summaryType').text(bookingData.categoryText || bookingData.bookingType || '');

//    $('#summaryGuests').text((bookingData.numberOfGuests || bookingData.seatingCapacity || 0) + ' people');

//    $('#summaryDate').text(new Date(bookingData.reservationDate).toLocaleDateString());

//    $('#summaryTime').text(bookingData.timeText || bookingData.timeSlot || '');

//    $('#summaryName').text(bookingData.fullName || bookingData.customerName || '');

//    // Normalize category and map to amounts (handle different naming: "party" vs "celebration" etc.)

//    const amountMap = {

//        date: 499,

//        family: 999,

//        meeting: 2499,

//        celebration: 4999,

//        //party: 4999

//    };

//    function resolveAmount(data) {

//        // 1. Try explicit text fields

//        const text = (data.categoryText || data.bookingType || '').toString().trim().toLowerCase();

//        if (text && amountMap[text] !== undefined) return amountMap[text];

//        // 2. Try numeric enum value if present (some flows store category as number)

//        if (typeof data.category !== 'undefined' && data.category !== null) {

//            // Map enum integer to amounts (match server enum: Date=..., Family=..., Meeting=..., Celebration=...)

//            switch (parseInt(data.category, 10)) {

//                case 0: // Date

//                    return amountMap['date'];

//                case 1: // Family

//                    return amountMap['family'];

//                case 2: // Meeting

//                    return amountMap['meeting'];

//                case 3: // Celebration

//                    return amountMap['celebration'];

//            }

//        }

//        // 3. Fallback default

//        return 400;

//    }

//    const amount = resolveAmount(bookingData);

//    $('#baseAmount, #totalAmount').text('₹' + amount);

//    let selectedMethod = null;

//    $('.payment-method').click(function () {

//        $('.payment-method').removeClass('active');

//        $(this).addClass('active');

//        selectedMethod = $(this).data('method');

//        $('#upiForm, #cardForm').hide();

//        if (selectedMethod === 'upi') $('#upiForm').show();

//        if (selectedMethod === 'card') $('#cardForm').show();

//        $('#payBtn').fadeIn();

//    });

//    $('#payBtn').click(function () {

//        if (!selectedMethod) {

//            alert('Please select payment method');

//            return;

//        }

//        const btn = $(this);

//        btn.prop('disabled', true);

//        $.post('/Customer/CreatePayment', {

//            reservationId: bookingData.reservationId,

//            method: selectedMethod === 'upi' ? 0 : 1

//        })

//            .done(function (res) {
//                // Build a small confirmation object for client-side confirmation page

//                const confirmation = {

//                    bookingId: res && res.reservationId ?  res.reservationId : ( Date.now()),

//                    bookingType: bookingData.categoryText || bookingData.bookingType || '',

//                    date: bookingData.reservationDate,

//                    timeSlot: bookingData.timeText || bookingData.timeSlot || '',

//                    seatingCapacity: bookingData.numberOfGuests || bookingData.seatingCapacity || 0,

//                    guests: bookingData.numberOfGuests || bookingData.guests || 0,

//                    customerName: bookingData.fullName || bookingData.customerName || '',

//                    reservationId: res && res.reservationId ? res.reservationId : null,

//                    amount: amount

//                };

//                // Persist to localStorage for backward-compat confirmation page

//                try {

//                    localStorage.setItem('lastBooking', JSON.stringify(confirmation));

//                } catch (e) {

//                    console.warn('Could not write to localStorage', e);

//                }

//                // Remove transient session data

//                sessionStorage.removeItem('currentReservation');

//                // Redirect to server-rendered confirmation with reservationId (if available).

//                if (confirmation.reservationId) {

//                    window.location.href = '/Customer/Confirmation?reservationId=' + confirmation.reservationId;

//                } else {

//                    // Fallback to client-side confirmation page (reads localStorage.lastBooking)

//                    window.location.href = '/Customer/Confirmation';

//                }

//            })

//            .fail(function (err) {

//                alert(err.responseText || 'Payment failed');

//                btn.prop('disabled', false);

//            });

//    });

//});

























$(document).ready(function () {

    const bookingData = JSON.parse(sessionStorage.getItem('currentReservation'));

    if (!bookingData || !bookingData.reservationId) {

        window.location.href = '/Customer/Reservation';

        return;

    }

    // Populate summary (if Payment page shows a summary)

    $('#summaryType').text(bookingData.categoryText || bookingData.bookingType || '');

    $('#summaryGuests').text((bookingData.numberOfGuests || bookingData.seatingCapacity || 0) + ' people');

    $('#summaryDate').text(new Date(bookingData.reservationDate).toLocaleDateString());

    $('#summaryTime').text(bookingData.timeText || bookingData.timeSlot || '');

    $('#summaryName').text(bookingData.fullName || bookingData.customerName || '');

    const amountMap = {

        date: 499,

        family: 999,

        meeting: 2499,

        celebration: 4999,

        party: 4999

    };

    function resolveAmount(data) {

        const text = (data.categoryText || data.bookingType || '').toString().trim().toLowerCase();

        if (text && amountMap[text] !== undefined) return amountMap[text];

        if (typeof data.category !== 'undefined' && data.category !== null) {

            switch (parseInt(data.category, 10)) {

                case 0: return amountMap['date'];

                case 1: return amountMap['family'];

                case 2: return amountMap['meeting'];

                case 3: return amountMap['celebration'];

            }

        }

        return 400;

    }

    const amount = resolveAmount(bookingData);

    $('#baseAmount, #totalAmount').text('₹' + amount);

    let selectedMethod = null;

    $('.payment-method').click(function () {

        $('.payment-method').removeClass('active');

        $(this).addClass('active');

        selectedMethod = $(this).data('method');

        $('#upiForm, #cardForm').hide();

        if (selectedMethod === 'upi') $('#upiForm').show();

        if (selectedMethod === 'card') $('#cardForm').show();

        $('#payBtn').fadeIn();

    });

    $('#payBtn').click(function () {

        if (!selectedMethod) {

            alert('Please select payment method');

            return;

        }

        const btn = $(this);

        btn.prop('disabled', true);

        $.post('/Customer/CreatePayment', {

            reservationId: bookingData.reservationId,

            method: selectedMethod === 'upi' ? 0 : 1

        })

            .done(function (res) {

                // Build a small confirmation object for client-side confirmation page

                var reservationIdValue = null;

                if (res && res.reservationId) reservationIdValue = res.reservationId;

                else if (bookingData.reservationId) reservationIdValue = bookingData.reservationId;

                const bookingIdStr = reservationIdValue ? reservationIdValue.toString().padStart(2, '0') : ('00');

                const confirmation = {

                    bookingId: bookingIdStr, // no 'BK' prefix

                    bookingType: bookingData.categoryText || bookingData.bookingType || '',

                    date: bookingData.reservationDate,

                    timeSlot: bookingData.timeText || bookingData.timeSlot || '',

                    seatingCapacity: bookingData.numberOfGuests || bookingData.seatingCapacity || 0,

                    guests: bookingData.numberOfGuests || bookingData.guests || 0,

                    customerName: bookingData.fullName || bookingData.customerName || '',

                    reservationId: reservationIdValue,

                    amount: amount

                };

                // Persist to localStorage for backward-compat confirmation page

                try {

                    localStorage.setItem('lastBooking', JSON.stringify(confirmation));

                } catch (e) {

                    console.warn('Could not write to localStorage', e);

                }

                // Remove transient session data

                sessionStorage.removeItem('currentReservation');

                // Redirect to server-rendered confirmation with reservationId (if available).

                if (confirmation.reservationId) {

                    window.location.href = '/Customer/Confirmation?reservationId=' + confirmation.reservationId;

                } else {

                    // Fallback to client-side confirmation page (reads localStorage.lastBooking)

                    window.location.href = '/Customer/Confirmation';

                }

            })

            .fail(function (err) {

                alert(err.responseText || 'Payment failed');

                btn.prop('disabled', false);

            });

    });

});





