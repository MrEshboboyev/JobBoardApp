function markAsRead(id) {
    fetch(`/notification/MarkAsRead/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                // Reload the page after marking the notification as read
                location.reload();
            } else {
                console.error('Failed to mark notification as read.');
            }
        })
        .catch(error => console.error('Error:', error));
}