var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("payment_pending")) {
        loadDataTable("payment_pending")
    }
    else if (url.includes("payment_approved")) {
        loadDataTable("payment_approved")
    }
    else if (url.includes("order_processing")) {
        loadDataTable("order_processing")
    }
    else if (url.includes("order_shipped")) {
        loadDataTable("order_shipped")
    }
    else if (url.includes("order_cancelled")) {
        loadDataTable("order_cancelled")
    }
    else {
        loadDataTable("all")
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/admin/order/getall?status=' + status },
        columns: [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "20%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="text-center">
                     <a href="/admin/order/details?orderId=${data}" class="btn btn-outline-info"> <i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}