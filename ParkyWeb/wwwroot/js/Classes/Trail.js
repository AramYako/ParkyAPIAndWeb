
let dataTable;

let refTableId = "#trailDataTable";

$(document).ready(function () {
    initTrailTable();
});



let initTrailTable = () => {
    $(refTableId).DataTable();
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
