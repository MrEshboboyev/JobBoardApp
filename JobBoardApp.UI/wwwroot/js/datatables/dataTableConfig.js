function initializeDataTable(tableId, nonSortableColumnIndex) {
    $(document).ready(function () {
        $(tableId).DataTable({
            // Enable Bootstrap 5 styling
            "dom": '<"top"f>rt<"bottom"lp><"clear">',
            "paging": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "responsive": true,
            "columnDefs": [
                { "orderable": false, "targets": nonSortableColumnIndex } // Disable sorting on specified column
            ]
        });
    });
}
