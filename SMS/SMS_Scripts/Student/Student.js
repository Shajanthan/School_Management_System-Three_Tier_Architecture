
currentPage = 1;
pageSize = 5;
totalPages = 0;

$(document).ready(function () {

    //Add new button
    $('#addNew').click(function () {
        $('#addStudentForm').show();
        $('#studentDetails').hide();
        $('#studentHead').hide();
        $('#paginationNextPrevious').hide();
        $.ajax({
            url: '/Student/AddStudent',
            type: 'GET',
            success: function (response) {
                $('#addStudentForm').html(response);
                $.validator.unobtrusive.parse($('#addStudentForm'));

            }
        });
    });

    //Active filter
    $('#isActiveFilter').change(function () {
        $('#nextButton').removeClass('disabled');
        currentPage = 1;
        loadData(currentPage, pageSize);
    });

    //Student details pop up close
    $('.close').click(function () {
        $('#studentDetailsModal').modal('hide');
    });

    //search
    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val().trim();
        if (searchTerm.length >= 2) {
            searchStudent(searchTerm);
        } else if (searchTerm.length === 0) {
            loadData(currentPage, pageSize)
        }
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


//initially load the all Student data [and sory by availability]
async function loadData(currentPage, pageSize) {
    var isActiveFilter = $('#isActiveFilter').val();
    try
    {
        var data = await $.ajax({
            url: '/Student/AllStudent',
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
            var enableButton = '<button type="button" class="btn btn-sm ' + enableButtonClass + '" onclick="toggleEnable(\'' + item.StudentID + '\', ' + toggleState + ', \'' + item.DisplayName + '\')"><i class="' + enableButtonIconClass + '" style="' + iconStyle + '"></i></button>';
            var editUrl = '/Student/EditStudent/' + item.StudentID;
            var deleteUrl = '/Student/DeleteStudent/' + item.StudentID;
            var emailIcon = '<i class="bi bi-envelope-fill "></i>';
            var telIcon = '<i class="bi bi-telephone-fill text-success"></i>';
            var emailLink = '<a href="mailto:' + item.Email + '" title="' + item.Email + '">' + emailIcon + '</a>';
            var contactNo = '<a href="tel:' + item.ContactNo + '"><i class="bi bi-telephone-fill text-success" data-toggle="tooltip" title="' + item.ContactNo + '"></i></a>';
            var studentID = item.StudentID;
            var editButton;
            var deleteButton;

            // check student allocated or not
            var response = await $.ajax({
                url: '/Student/IsStudentAllocated',
                type: 'GET',
                data: { studentID: studentID }
            });

            if (!response) {
                editButton = '<button type="button" class="btn btn-sm btn-primary" onclick="editStudent(\'' + item.StudentID + '\')"><i class="bi bi-pen small-icons"></i></button> ';
                deleteButton = '<button type="button" class="btn btn-sm btn-danger" onclick="deleteStudent(\'' + item.StudentID + '\', ' + toggleState + ', \'' + item.FirstName + ' ' + item.LastName + '\')"><i class="bi bi-trash small-icons"></i></button>';
            } else {
                editButton = "";
                deleteButton = "";
            }

            var row = '<tr>' +
                '<td>' + enableButton + '</td>' +
                '<td>' + item.StudentRegNo + '</td>' +
                '<td>' + item.FirstName + '</td>' +
                '<td>' + item.LastName + '</td>' +
                '<td>' + item.DisplayName + '</td>' +
                '<td>' + emailLink + '</td>' +
                '<td>' + item.Gender + '</td>' +
                '<td>' + contactNo + '</td>' +
                '<td>' +
                editButton + deleteButton +
                '<button type="button" class="btn btn-sm btn-info m-1" onclick="moreDetails(\'' + item.StudentID + '\', ' + toggleState + ', \'' + item.FirstName + ' ' + item.LastName + '\')"><i class="bi bi-ticket-detailed small-icons"></i>' +
                '</td>' +
                '</tr>';
            $('#tableBody').append(row);
        }

        totalPages = data.totalPages;

        updatePagination(currentPage, totalPages);
    }
    catch (error) {
        console.log(error);
        alert('An error occurred while loading data.');
    }
}


//pop up Student detail
function moreDetails(studentID) {
    $.ajax({
        url: '/Student/GetStudentDetails/' + studentID,
        type: 'GET',
        success: function (data) {
            $('#studentDetailsBody').html(data);
            $('#studentDetailsModal').modal('show');
        },
        error: function (error) {
            console.log(error);
            alert('An error occurred while fetching student details.');
        }
    });
}

//back buttton on add and update
function backStudent() {
    $('#addStudentForm').hide();
    $('#studentDetails').show();
    $('#studentHead').show();
    $('#addStudentForm').find('input[type=text], input[type=number], input[type=date], select').val('');
    $('#paginationNextPrevious').show();
}

//add Student
function addStudentSuccess(response) {
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
                $('#addStudentForm').hide();
                $('#paginationNextPrevious').show();
                $('#addStudentForm').find('input[type=text], input[type=number], input[type=date], select').val('');
                $('#studentDetails').show();
                $('#studentHead').show();
            }
        });
    } else {
        Swal.fire({ icon: 'warning', title: 'Warning', text: response.message });
    }
}

function addStudentFailure(error) {
    console.log(error);
    Swal.fire('Error!', 'Error on adding the student', 'error');
}

//delete teacher
function deleteStudent(studentID, state, fname) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete the student "' + fname + '" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Student/DeleteStudent/' + studentID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#tableBody tr:has(td:contains(' + studentID + '))').remove();
                        loadData(currentPage, pageSize)
                        Swal.fire('Deleted!', 'Record deleted successfully.', 'success');
                    } else {
                        Swal.fire('Delete Prevented!', response.message, 'warning');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the student.', 'error');
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
                url: '/Student/ToggleEnable',
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
                    Swal.fire('Error!', 'An error occurred while toggling student status.', 'error');
                }
            });
        }
    });
}


function editStudent(studentID) {
    $.ajax({
        url: '/Student/AddStudent/' + studentID,
        type: 'GET',
        success: function (data) {
            $('#paginationNextPrevious').hide();
            $('#addStudentForm').html(data);
            $('#addStudentForm').show();
            $.validator.unobtrusive.parse($('#editStudentForm'));
            $('#studentDetails').hide();
            $('#studentHead').hide();
        },
        error: function (error) {
            console.log(error);
            Swal.fire('Error!', 'An error occurred while fetching student details.', 'error');
        }
    });
}

//search student
function searchStudent() {
    var searchTerm = $('#searchInput').val().trim();
    var searchCriteria = $('#searchCriteria').val();
    if (searchTerm.length >= 2) {
        $.ajax({
            url: '/Student/Search',
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
    var tableRows = $('#studentDetails tbody tr').get();
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

//next data
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

