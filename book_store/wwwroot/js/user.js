var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/User/Getall' },
        columns: [
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'role', "width": "15%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var current = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > current) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                <i class="bi bi-lock-fill"></i> Lock
                            </a> 
                            <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white mt-2" style="cursor:pointer; width:150px;">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                        `
                    } else {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:150px;">
                                <i class="bi bi-unlock-fill"></i>  UnLock
                            </a> 
                            <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white mt-2" style="cursor:pointer; width:150px;">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                        `
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id), // data that send to server by JSON format
        contentType: "application/json", // บอก Server ว่าส่ง JSON ไป
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}