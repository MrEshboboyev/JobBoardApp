document.addEventListener("DOMContentLoaded", function () {
    fetchUnreadNotifications(); // Call the function when the page loads
});

function fetchUnreadNotifications() {
    fetch('/notification/GetUnreadCount')
        .then(response => response.json())
        .then(data => {
            document.getElementById('notificationCount').innerText = data.unreadCount;
        })
        .catch(error => console.error("Error fetching unread notifications:", error));
}
