// wwwroot/js/site-clock.js
function updateProjectClock() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('en-GB', { hour12: false });

    // This handles pages using 'currentTime' (Dashboard/Booking)
    const element1 = document.getElementById('currentTime');
    if (element1) element1.textContent = timeString;

    // This handles pages using 'liveClock' (Tables)
    const element2 = document.getElementById('liveClock');
    if (element2) element2.textContent = timeString;
}

// Initialize the clock immediately and set interval
document.addEventListener('DOMContentLoaded', () => {
    updateProjectClock();
    setInterval(updateProjectClock, 1000);
});