
currentPage = 1;
pageSize = 5;
totalPages = 0;

$(document).ready(function () {
    //Teacher details pop up close
    $('.close').click(function () {
        $('#teacherDetailsModal').modal('hide');
    });

    //Add new button
    $('#addNew').click(function () {
        $('#addTeacherForm').show();
        $('#teacherDetails').hide();
        $('#teacherHead').hide();
        $('#paginationNextPrevious').hide();
        $.ajax({
            url: '/Teacher/AddTeacher',
            type: 'GET',
            success: function (response) {
                $('#addTeacherForm').html(response);
                $.validator.unobtrusive.parse($('#addTeacherForm'));

            }
        });
    });



    //Search
    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val().trim();
        if (searchTerm.length >= 2) {
            searchTeacher(searchTerm);
        } else if (searchTerm.length === 0) {
            loadData(currentPage, pageSize)
        }
    });

    //Active filter
    $('#isActiveFilter').change(function () {
        $('#nextButton').removeClass('disabled');
        currentPage = 1;
        loadData(currentPage, pageSize);
    });

    //Sort filter
    $('.sortable').click(function () {
        var column = $(this).index();
        sortTable(column);
    });

    //page size
    $('#pageSize').change(function () {
        pageSize = parseInt($(this).val());
        $('#nextButton').removeClass('disabled');
        currentPage = 1;
        loadData(currentPage, pageSize);
    });

    //Load Data on page load
    loadData(currentPage, pageSize)


});

//initially load the all teacher data [and sory by availability]
async function loadData(currentPage, pageSize) {
    var isActiveFilter = $('#isActiveFilter').val();

    try
    {
        var data = await $.ajax({
            url: '/Teacher/AllTeachers',
            type: 'GET',
            data: { pageNumber: currentPage, pageSize: pageSize, isActive: isActiveFilter }
        });


        $('#tableBody').empty();

        for (const item of data.data) {
            var enableButtonClass = item.IsEnable ? 'btn-success' : 'btn-danger';
            var enableButtonIconClass = item.IsEnable ? 'bi bi-power' : 'bi bi-power';
            var buttonText = item.IsEnable ? 'Enabled' : 'Disabled';
            var toggleState = item.IsEnable ? 'false' : 'true';
            var iconStyle = item.IsEnable ? '' : 'filter: grayscale(100%);';
            var enableButton = '<button type="button" class="btn btn-sm ' + enableButtonClass + '" onclick="toggleEnable(\'' + item.TeacherID + '\', ' + toggleState + ', \'' + item.DisplayName + '\')"><i class="' + enableButtonIconClass + '" style="' + iconStyle + '"></i></button>';
            var editUrl = '/Teacher/EditTeacher/' + item.TeacherID;
            var deleteUrl = '/Teacher/DeleteTeacher/' + item.TeacherID;
            var emailIcon = '<i class="bi bi-envelope-fill "></i>';
            var telIcon = '<i class="bi bi-telephone-fill text-success"></i>';
            var emailLink = '<a href="mailto:' + item.Email + '" title="' + item.Email + '">' + emailIcon + '</a>';
            var contactNo = '<a href="tel:' + item.ContactNo + '"><i class="bi bi-telephone-fill text-success" data-toggle="tooltip" title="' + item.ContactNo + '"></i></a>';
            var teacherID = item.TeacherID;
            var editButton;
            var deleteButton;

            // check student allocated or not
            var response = await $.ajax({
                url: '/Teacher/IsTeacherAllocated',
                type: 'GET',
                data: { teacherID: teacherID }
            });

            if (!response) {
                editButton = '<button type="button" class="btn btn-sm btn-primary" onclick="editTeacher(\'' + item.TeacherID + '\')"><i class="bi bi-pen small-icons"></i></button> ';
                deleteButton = '<button type="button" class="btn btn-sm btn-danger" onclick="deleteTeacher(\'' + item.TeacherID + '\', ' + toggleState + ', \'' + item.FirstName + ' ' + item.LastName + '\')"><i class="bi bi-trash small-icons"></i></button>';
            }
            else {
                editButton = '<span class="badge bg-primary">Allocated</span>';
                deleteButton = "";
            }

            var row = '<tr>' +
                '<td>' + enableButton + '</td>' +
                '<td>' + item.TeacherRegNo + '</td>' +
                '<td>' + item.FirstName + '</td>' +
                '<td>' + item.LastName + '</td>' +
                '<td>' + item.DisplayName + '</td>' +
                '<td>' + emailLink + '</td>' +
                '<td>' + item.Gender + '</td>' +
                '<td>' + contactNo + '</td>' +
                '<td>' + editButton + deleteButton+
               '<button type="button" class="btn btn-sm btn-info m-1" onclick="moreDetails(\'' + item.TeacherID + '\', ' + toggleState + ', \'' + item.FirstName + ' ' + item.LastName + '\')"><i class="bi bi-ticket-detailed small-icons"></i>' +
                '</td>' +
                '</tr>';
            $('#tableBody').append(row);
        }

        totalPages = data.totalPages;

        updatePagination(currentPage, totalPages);
    }
    catch (error) {
        var row = '<tr>' + '<td colspan="9">' + "No data Found" + '</td>' + '</tr>';
        $('#tableBody').append(row);
    }       
    
}

//pop up teacher detail
function moreDetails(teacherID) {
    $.ajax({
        url: '/Teacher/GetTeacherDetails/' + teacherID,
        type: 'GET',
        success: function (data) {
            $('#teacherDetailsBody').html(data);
            $('#teacherDetailsModal').modal('show');
        },
        error: function (error) {
            console.log(error);
            alert('An error occurred while fetching teacher details.');
        }
    });
}

//back buttton on add and update
function backTeacher() {
    $('#addTeacherForm').hide();
    $('#paginationNextPrevious').show();
    $('#teacherDetails').show();
    $('#teacherHead').show();
    $('#addTeacherForm').find('input[type=text], input[type=number], input[type=date], select').val('');
}

//add teacher
function addTeacherSuccess(response) {
    if (response.success) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: response.message,
            showCancelButton: false,
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.isConfirmed) {
                loadData(currentPage, pageSize)
                $('#addTeacherForm').hide();
                $('#paginationNextPrevious').show();
                $('#addTeacherForm').find('input[type=text], input[type=number], input[type=date], select').val('');
                $('#teacherDetails').show();
                $('#teacherHead').show();
            }
        });
    } else {
        Swal.fire({ icon: 'warning', title: 'Warningry', text: response.message });
    }
}

function addTeacherFailure(error) {
    console.log(error);
    Swal.fire('Error!', 'Error on adding the teacher', 'error');
}


//delete teacher
function deleteTeacher(teacherID, state, fname) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete the teacher "' + fname + '" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Teacher/DeleteTeacher/' + teacherID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#tableBody tr:has(td:contains(' + teacherID + '))').remove();
                        loadData(currentPage, pageSize)
                        Swal.fire('Deleted!', 'Record deleted successfully.', 'success');
                    } else {
                        Swal.fire('Delete Prevented!', response.message, 'warning');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the teacher.', 'error');
                }
            });
        }
    });
}

//toggle teacher availability
function toggleEnable(id, enable, name) {
    Swal.fire({
        title: 'Confirmation',
        text: 'Are you sure you want to change the status of "' + name + '" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Teacher/ToggleEnable',
                type: 'POST',
                data: { id: id, enable: enable },
                success: function (response) {
                    if (response.success) {
                        loadData(currentPage, pageSize)
                        Swal.fire('Success!', response.message, 'success');
                    } else {
                        Swal.fire('Operation Failed!', response.message, 'warning');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while toggling teacher status.', 'error');
                }
            });
        }
    });
}




//Edit Teacher
function editTeacher(teacherID) {
    $.ajax({
        url: '/Teacher/AddTeacher/' + teacherID,
        type: 'GET',
        success: function (data) {
            $('#paginationNextPrevious').hide();
            $('#addTeacherForm').html(data);
            $('#addTeacherForm').show();
            $.validator.unobtrusive.parse($('#addTeacherForm'));
            $('#teacherDetails').hide();
            $('#teacherHead').hide();
        },
        error: function (error) {
            console.log(error);
            Swal.fire('Error!', 'An error occurred while fetching teacher details.', 'error');
        }
    });
}

//search teacher
function searchTeacher() {
    var searchTerm = $('#searchInput').val().trim();
    var searchCriteria = $('#searchCriteria').val();
    if (searchTerm.length >= 2) {
        $.ajax({
            url: '/Teacher/Search',
            type: 'GET',
            data: { searchTerm: searchTerm, searchCriteria: searchCriteria },
            success: function (data) {
                $('#tableBody').html(data);
            },
            error: function (error) {
                console.log(error);
                Swal.fire('Error!', 'An error occurred while searching teacher.', 'error');
            }
        });
    } else if (searchTerm.length === 0) {
        loadData(currentPage, pageSize)
    }
}


//sort by column name click
var sortAscending = true;

function sortTable(column) {
    var tableRows = $('#teacherDetails tbody tr').get();
    tableRows.sort(function (a, b) {
        var valA = $(a).find('td').eq(column).text().toUpperCase();
        var valB = $(b).find('td').eq(column).text().toUpperCase();
        var comparison = valA.localeCompare(valB);
        return sortAscending ? comparison : -comparison;
    });

    $('#tableBody').empty().append(tableRows);

    sortAscending = !sortAscending;
}

//pagination
function updatePagination(currentPage, totalPage) {

    if (currentPage == 1) {
        $('#prevButton').addClass('disabled');
    } else {
        $('#prevButton').removeClass('disabled');
    }


}

//previous data
function previousData() {
    if (currentPage > 1) {
        currentPage--;
        $('#nextButton').removeClass('disabled');
        loadData(currentPage, pageSize);

    }
}

function nextData() {
    if (currentPage == totalPages) {
        $('#nextButton').addClass('disabled');
    }
    if (currentPage < totalPages) {
        currentPage++;
        loadData(currentPage, pageSize);
        if (currentPage === totalPages) {
            $('#nextButton').addClass('disabled');
        }

    }
}

