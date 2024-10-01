document.getElementById('jobSearchInput').addEventListener('input', function () {
    const searchQuery = this.value.toLowerCase();
    const jobListings = document.querySelectorAll('.job-listing');
    let visibleListings = 0; // Track visible listings

    jobListings.forEach(function (job) {
        const title = job.getAttribute('data-title').toLowerCase();
        const description = job.getAttribute('data-description').toLowerCase();
        const location = job.getAttribute('data-location').toLowerCase();
        const salary = job.getAttribute('data-salary').toLowerCase();
        const jobType = job.getAttribute('data-jobtype').toLowerCase();

        if (
            title.includes(searchQuery) ||
            description.includes(searchQuery) ||
            location.includes(searchQuery) ||
            salary.includes(searchQuery) ||
            jobType.includes(searchQuery)
        ) {
            job.style.display = '';
            visibleListings++;
        } else {
            job.style.display = 'none';
        }
    });

    // Show or hide the "No Matching Job Listing Found" message
    const noResultsMessage = document.getElementById('noResultsMessage');
    if (visibleListings === 0) {
        noResultsMessage.style.display = 'block';
    } else {
        noResultsMessage.style.display = 'none';
    }
});
