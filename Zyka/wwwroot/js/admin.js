/*GLOBAL STATE */
let ALL_BOOKINGS = [];

/* INITIALIZER*/
function initBookings(bookingsFromServer) {
    ALL_BOOKINGS = bookingsFromServer || [];
}

/* DATE GETTERS*/
function getTodayDate() {
    return new Date().toISOString().split('T')[0];
}

function formatDateForTitle(dateStr) {
    const date = new Date(dateStr);
    return date.toLocaleDateString('en-US', {
        month: 'long',
        day: 'numeric',
        year: 'numeric'
    });
}

/***********************
 * FILTER FUNCTIONS
 ***********************/
function getTodayBookings() {
    const today = getTodayDate();
    return ALL_BOOKINGS.filter(b => b.Date === today);
}

function getFutureBookings() {
    const today = getTodayDate();
    return ALL_BOOKINGS.filter(b => b.Date > today);
}

function getBookingsByDate(dateStr) {
    return ALL_BOOKINGS.filter(b => b.Date === dateStr);
}

function renderBookingCards(bookings, containerId) {
    const container = document.getElementById(containerId);

    if (!container) return;

    if (bookings.length === 0) {
        container.innerHTML =
            `<p class="text-muted text-center py-5">No bookings found</p>`;
        return;
    }

    let html = '';

    bookings.forEach(booking => {
        const statusClass =
            booking.Status === 'confirmed' ? 'success' :
                booking.Status === 'completed' ? 'secondary' : 'danger';

        html += `
            <div class="border rounded p-3 mb-3 hover:border-warning transition">
                <p class="text-dark-green fw-bold mb-2">${booking.CustomerName}</p>
                <p class="text-muted mb-1 small">
                    <i class="bi bi-grid-3x3 me-1"></i>
                    ${booking.TableCategory} - ${booking.TableNumber}
                </p>
                <p class="text-muted mb-2 small">
                    <i class="bi bi-clock me-1"></i>
                    ${booking.Time}
                </p>
                <span class="badge bg-${statusClass} text-capitalize">
                    ${booking.Status}
                </span>
            </div>
        `;
    });

    container.innerHTML = html;
}

/* CALENDAR RENDERING (History page) */
function renderCalendar(currentDate, calendarContainerId, monthTitleId, onDateClick) {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();

    const monthNames = [
        'January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'
    ];

    document.getElementById(monthTitleId).innerText =
        `${monthNames[month]} ${year}`;

    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const daysInMonth = lastDay.getDate();
    const startDay = firstDay.getDay();

    let html = '';

    for (let i = 0; i < startDay; i++) {
        html += `<div></div>`;
    }

    for (let day = 1; day <= daysInMonth; day++) {
        const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
        const hasBookings = getBookingsByDate(dateStr).length > 0;
        const isToday = getTodayDate() === dateStr;

        let classes = 'calendar-day';
        if (hasBookings) classes += ' has-bookings';
        if (isToday) classes += ' today';

        html += `
            <div class="${classes}" data-date="${dateStr}">
                <div>${day}</div>
                ${hasBookings ? `<small>${getBookingsByDate(dateStr).length}</small>` : ''}
            </div>
        `;
    }

    const calendar = document.getElementById(calendarContainerId);
    calendar.innerHTML = html;

    calendar.querySelectorAll('.calendar-day').forEach(dayEl => {
        dayEl.addEventListener('click', () => {
            calendar.querySelectorAll('.calendar-day')
                .forEach(d => d.classList.remove('active'));

            dayEl.classList.add('active');
            onDateClick(dayEl.dataset.date);
        });
    });
}