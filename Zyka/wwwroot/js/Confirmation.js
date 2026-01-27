$(document).ready(function () {

    // Local storage se data nikalna

    const booking = JSON.parse(localStorage.getItem('lastBooking'));

    // Agar data nahi milta toh home page par redirect karna

    if (!booking) {

        window.location.href = '/';

        return;

    }

    // UI elements ko update karna

    // booking.bookingId is now plain numeric string (padded) e.g. "07"

    $('#bookingId').text(booking.bookingId || (booking.reservationId ? booking.reservationId.toString().padStart(2, '0') : '00'));

    $('#bookingType').text(booking.bookingType || '');

    if (booking.date) {

        const dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };

        $('#bookingDate').text(new Date(booking.date).toLocaleDateString(undefined, dateOptions));

    }

    $('#bookingTime').text(booking.timeSlot || '');

    const guests = booking.seatingCapacity || booking.guests || '0';

    $('#bookingGuests').text(guests + ' people');

});
