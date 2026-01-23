document.addEventListener('DOMContentLoaded', function () {

    document.querySelectorAll('.cancel-booking-btn').forEach(btn => {

        btn.addEventListener('click', function () {

            const reservationId = this.dataset.id;

            if (!confirm('Are you sure you want to cancel this booking?'))

                return;

            fetch('/Customer/CancelReservation', {

                method: 'POST',

                headers: {

                    'Content-Type': 'application/x-www-form-urlencoded'

                },

                body: `reservationId=${reservationId}`

            })

                .then(res => {

                    if (!res.ok) throw new Error('Cancel failed');

                    return res.json();

                })

                .then(() => {

                    alert('Booking cancelled successfully');

                    window.location.reload();

                })

                .catch(() => {

                    alert('Unable to cancel booking');

                });

        });

    });

});



