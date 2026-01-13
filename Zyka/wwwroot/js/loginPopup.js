document.addEventListener("DOMContentLoaded", function () {
    openLoginPopup();
});

function openLoginPopup() {
    document.getElementById("loginOverlay").classList.add("active");
}

function closeLoginPopup() {
    document.getElementById("loginOverlay")
        .classList.remove("active");
}