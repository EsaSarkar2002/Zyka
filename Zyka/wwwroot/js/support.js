$('#submitTicket').click(function () {

    const reservationIdRaw = $('#reservationId').val();

    const payload = {
        customerName: $('#custName').val().trim(),
        phoneNumber: $('#phone').val().trim(),
        email: $('#email').val().trim(),
        reservationId: reservationIdRaw ? parseInt(reservationIdRaw) : null,
        query: $('#query').val().trim()
    };

    if (!payload.customerName || !payload.phoneNumber || !payload.email || !payload.query) {
        alert('All fields except Booking ID are required');
        return;
    }

    if (payload.query.length > 1000) {
        alert('Query is too long');
        return;
    }

    $.ajax({
        url: '/Customer/CreateSupportTicket',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (res) {
            alert('Your query has been submitted.\nTicket ID: ' + res.ticketId);
            window.location.href = '/';
        },
        error: function (err) {
            alert(err.responseText || 'Something went wrong');
        }
    });
});
