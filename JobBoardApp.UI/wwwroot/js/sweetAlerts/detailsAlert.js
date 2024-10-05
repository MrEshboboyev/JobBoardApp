function openDetails(title, details, iconType = 'info') {
    let detailsHtml = '';
    for (const [key, value] of Object.entries(details)) {
        detailsHtml += `<p><strong>${key}:</strong> ${value}</p>`;
    }

    Swal.fire({
        title: `<strong>${title}</strong>`,
        icon: iconType,
        html: detailsHtml,
        showCloseButton: true,
        focusConfirm: false,
        confirmButtonText: 'Close'
    });
}
