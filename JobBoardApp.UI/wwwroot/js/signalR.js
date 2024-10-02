const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")  // Hub URL
    .build();

// Start the connection
connection.start().then(() => {
    console.log("Connected to SignalR hub for notifications.");
}).catch(err => console.error(err.toString()));

// Listen for real-time notifications
connection.on("ReceiveNotification", (message) => {
    // Update the UI with the new notification
    alert("New Notification: " + message);  // For now, just alert the user

    // Optionally, update the unread count dynamically in the UI
    updateUnreadCount();
});

// Function to update unread notification count in the UI
function updateUnreadCount() {
    fetch('/notification/Index')
        .then(response => response.json())
        .then(data => {
            document.getElementById('notificationCount').innerText = data.unreadCount;
        });
}
