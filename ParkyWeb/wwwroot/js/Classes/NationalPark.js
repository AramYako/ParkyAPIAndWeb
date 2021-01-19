
let dataTable;

let refTableId = "#nationalDataTable";

$(document).ready(function () {
    initNationalParkTable();
});



let initNationalParkTable = () => {
    $(refTableId).DataTable();
}

function loadDataTable() {
    dataTable = $(refTableId).DataTable({
        ajax: '/nationalParks/GetAllNationalPark',
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "state", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/nationalParks/Upsert/${data}" 
                                    class='btn btn-success text-white'
                                        style='cursor:pointer;'> <i class=far fa-edit'></i></a>
                        & nbsp;
                                <a onclick=Delete("/nationalParks/Delete/${data}") 
                                    class='btn btn-danger text-white'
                                        style='cursor:pointer;'> <i class=far fa-trash-alt'></i></a>
                    </div>`
                },
                "width": "30%"
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure you want to delete?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(url, {
                method: 'DELETE'
            }).then(response => response.json())
                .then((response) => {
                    if (response.success) {
                        toastr.success("Content deleted")
                        location.reload();
                    }
                    else {
                        toastr.error(response.message)
                    }
                });
        }
    })
}
