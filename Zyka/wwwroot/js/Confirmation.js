$(document).ready(function () {
    // Local storage se data nikalna
    const booking = JSON.parse(localStorage.getItem('lastBooking'));

    // Agar data nahi milta toh home page par redirect karna
    if (!booking) {
        window.location.href = '/';
        return;
    }

    // UI elements ko update karna
    $('#bookingId').text(booking.bookingId || 'BK' + Date.now());
    $('#bookingType').text(booking.bookingType || 'N/A');

    // Date format karna
    if (booking.date) {
        const dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };
        $('#bookingDate').text(new Date(booking.date).toLocaleDateString(undefined, dateOptions));
    }

    $('#bookingTime').text(booking.timeSlot || 'N/A');

    // Guests capacity handle karna
    const guests = booking.seatingCapacity || booking.guests || '0';
    $('#bookingGuests').text(guests + ' people');
});