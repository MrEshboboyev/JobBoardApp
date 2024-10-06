function toggleUserStatus(userName, isActive) {
    const action = isActive ? 'Activate' : 'Deactivate';
    const message = isActive ? `Are you sure you want to activate ${userName}?` : `Are you sure you want to deactivate ${userName}?`;

    Swal.fire({
        title: `${action} User?`,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: `Yes, ${action.toLowerCase()}!`,
        showLoaderOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $.post(`/Architect/AppUser/${action}/${userName}`, function (response) {
                if (response.success) {
                    Swal.fire(`${action}d!`, `${userName} has been ${action.toLowerCase()}d.`, 'success').then(() => location.reload());
                } else {
                    Swal.fire('Error!', response.message, 'error');
                }
            });
        }
    });
}

function suspendUser(userName) {
    Swal.fire({
        title: 'Suspend User',
        html: `
            <label for="reason">Reason for Suspension</label>
            <textarea id="reason" class="swal2-textarea" placeholder="Enter reason"></textarea>
            <label for="endDate" class="mt-3">Suspension End Date (optional)</label>
            <input type="date" id="endDate" class="swal2-input">
        `,
        showCancelButton: true,
        confirmButtonText: 'Suspend',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            const reason = document.getElementById('reason').value;
            const endDate = document.getElementById('endDate').value || null;

            if (!reason) {
                Swal.showValidationMessage('Reason for suspension is required');
                return;
            }

            return $.ajax({
                url: '/Architect/AppUser/Suspend',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ userName, reason, endDate }),
                success: function (response) {
                    Swal.fire('Suspended!', `${userName} has been suspended.`, 'success').then(() => location.reload());
                },
                error: function (xhr) {
                    Swal.fire('Error', 'Suspension failed: ' + xhr.responseText, 'error');
                }
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}

function unlockUser(userName) {
    Swal.fire({
        title: 'Unsuspend User',
        text: `Are you sure you want to unsuspend ${userName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Unsuspend!',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            return $.ajax({
                url: '/Architect/AppUser/Unsuspend',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(userName),
                success: function (response) {
                    Swal.fire('Unsuspended!', `${userName} has been unsuspended.`, 'success').then(() => location.reload());
                },
                error: function (xhr) {
                    Swal.fire('Error', 'Unsuspension failed: ' + xhr.responseText, 'error');
                }
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}


function resetPassword(userName) {
    Swal.fire({
        title: 'Reset Password',
        input: 'password',
        inputLabel: 'New Password',
        showCancelButton: true,
        confirmButtonText: 'Reset',
        preConfirm: (newPassword) => {
            return $.ajax({
                url: '/Architect/AppUser/ResetPassword',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ userName: userName, newPassword: newPassword }),
                success: function (response) {
                    Swal.fire('Password Reset!', `${userName}'s password has been reset.`, 'success').then(() => location.reload());
                },
                error: function (xhr) {
                    Swal.fire('Error', 'Reset password failed: ' + xhr.responseText, 'error');
                }
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}

function assignRole(userName) {
    Swal.fire({
        title: 'Assign Role',
        input: 'text',
        inputLabel: 'Role Name',
        showCancelButton: true,
        confirmButtonText: 'Assign',
        preConfirm: (role) => {
            return $.ajax({
                url: '/Architect/AppUser/AssignRole',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ userName: userName, role: role }),
                success: function (response) {
                    Swal.fire('Role assigned!', `${userName} has been assigned the role.`, 'success').then(() => location.reload());
                },
                error: function (xhr) {
                    Swal.fire('Error', 'Assigning role failed: ' + xhr.responseText, 'error');
                }
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}

function confirmDeleteUser(userName) {
    Swal.fire({
        title: 'Delete User?',
        text: `Are you sure you want to delete ${userName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete!',
    }).then((result) => {
        if (result.isConfirmed) {
            $.post(`/Architect/AppUser/Delete/${userName}`, function (response) {
                if (response.success) {
                    Swal.fire('Deleted!', `${userName} has been deleted.`, 'success').then(() => location.reload());
                } else {
                    Swal.fire('Error!', response.message, 'error');
                }
            });
        }
    });
}
