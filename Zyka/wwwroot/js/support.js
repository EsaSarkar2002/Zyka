$('#submitTicket').click(function () {

    const payload = {
        customerName: $('#custName').val(),
        phoneNumber: $('#phone').val(),
        email: $('#email').val(),
        reservationId: $('#reservationId').val() || null,
        query: $('#query').val()
    };

    if (!payload.customerName || !payload.phoneNumber || !payload.email || !payload.query) {
        alert('All fields except Booking ID are required');
        return;
    }

    $.ajax({
        url: '/Customer/CreateSupportTicket',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function () {
            alert('Your query has been successfully submitted');
            window.location.href = '/';
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
});

