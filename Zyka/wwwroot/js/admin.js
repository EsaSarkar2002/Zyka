/* ======================================================
 
   GLOBAL STATE

====================================================== */

//history page correction--01//

//alert("admin.js loaded")

//-----------01--------------//

let ALL_BOOKINGS = [];

/* ======================================================
 
   INITIALIZER (Called from Razor views)
 
====================================================== */
function updateCounts() {

    if (document.getElementById("todayCount"))

        document.getElementById("todayCount").innerText = getTodayBookings().length;

    if (document.getElementById("futureCount"))

        document.getElementById("futureCount").innerText = getFutureBookings().length;

}
function renderBookingsTable(bookings) {

    const tbody = document.getElementById("bookingsTableBody");

    if (!tbody) return;

    if (bookings.length === 0) {

        tbody.innerHTML = `
<tr>
<td colspan="6" class="text-center py-5 text-muted">
 
                    No bookings found
</td>
</tr>`;

        return;

    }

    let html = "";

    bookings.forEach(b => {

        const badgeClass =

            b.Status === "confirmed" ? "success" :

                b.Status === "completed" ? "secondary" : "danger";

        html += `
<tr>
<td>${b.CustomerName}</td>
<td class="text-capitalize">${b.TableCategory}</td>
<td>${b.TableNumber}</td>
<td>${formatDateForTitle(b.Date)}</td>
<td>${b.Time}</td>
<td>
<span class="badge bg-${badgeClass} text-capitalize">
 
                        ${b.Status}
</span>
</td>
</tr>
 
        `;

    });

    tbody.innerHTML = html;

}
function initBookings(bookings) {

    ALL_BOOKINGS = bookings || [];

    // If bookings table exists (Bookings page)

    if (document.getElementById("bookingsTableBody")) {

        renderBookingsTable(ALL_BOOKINGS);

        updateCounts();

    }

}

/* ======================================================
 
   DATE HELPERS
 
====================================================== */

function getTodayDate() {

    return new Date().toISOString().split("T")[0];

}

function formatDateForTitle(dateStr) {

    return new Date(dateStr).toLocaleDateString("en-US", {

        month: "long",

        day: "numeric",

        year: "numeric"

    });

}

/* ======================================================
 
   BOOKINGS FILTERS
 
====================================================== */

function getTodayBookings() {

    const today = getTodayDate();

    return ALL_BOOKINGS.filter(b => b.Date.startsWith(today));

}

function getFutureBookings() {

    const today = getTodayDate();

    return ALL_BOOKINGS.filter(b => b.Date > today);

}

function getBookingsByDate(dateStr) {

    return ALL_BOOKINGS.filter(b =>

        b.Date.split('T')[0] === dateStr

    );

}


/* ======================================================
 
   BOOKINGS TABLE (Admin → Bookings page)
 
====================================================== */



/* ======================================================
 
   HISTORY PAGE – CALENDAR
 
====================================================== */

function renderCalendar(currentDate, daysContainerId, titleId, onDateClick) {

    const daysContainer = document.getElementById(daysContainerId);

    const title = document.getElementById(titleId);

    if (!daysContainer || !title) return;

    daysContainer.innerHTML = "";

    const year = currentDate.getFullYear();

    const month = currentDate.getMonth();

    title.innerText = currentDate.toLocaleString("default", {

        month: "long",

        year: "numeric"

    });

    const firstDay = new Date(year, month, 1).getDay();

    const daysInMonth = new Date(year, month + 1, 0).getDate();

    // Empty slots before month starts

    for (let i = 0; i < firstDay; i++) {

        daysContainer.appendChild(document.createElement("div"));

    }

    // Actual days

    for (let day = 1; day <= daysInMonth; day++) {

        const dateStr = `${year}-${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;

        const bookingsCount = getBookingsByDate(dateStr).length;

        const dayEl = document.createElement("div");

        dayEl.className = "calendar-day";

        dayEl.innerText = day;

        if (bookingsCount > 0) {

            dayEl.classList.add("active");

            const badge = document.createElement("small");

            badge.innerText = bookingsCount;

            dayEl.appendChild(badge);

        }

        dayEl.onclick = () => onDateClick(dateStr);

        daysContainer.appendChild(dayEl);

    }

}

/* ======================================================
 
   HISTORY PAGE – BOOKINGS LIST
 
====================================================== */

function renderBookingCards(bookings, containerId) {

    const container = document.getElementById(containerId);

    if (!container) return;

    if (bookings.length === 0) {

        container.innerHTML = `
<p class="text-muted text-center py-5">
 
                No bookings for this date
</p>`;

        return;

    }

    let html = "";

    bookings.forEach(b => {

        const badgeClass = b.Status === "confirmed" ? "success" : "danger";

        html += `
<div class="border rounded p-3 mb-3">
<strong>${b.CustomerName}</strong><br>
 
                ${b.TableCategory} - ${b.TableNumber}<br>
 
                ${b.Time}<br>
<span class="badge bg-${badgeClass}">
 
                    ${b.Status}
</span>
</div>
 
        `;

    });

    container.innerHTML = html;

}

