currentPage = 1;
pageSize = 5;
totalPages = 0;

$(document).ready(function () {

    //add Click
    $('#addNew').click(function () {
        $('#subjectDetails').hide();
        $('#subjectHead').hide();
        $('#paginationNextPrevious').hide();
        $.ajax({
            url: '/Subject/AddSubject',
            type: 'GET',
            success: function (response) {
                $('#addSubjectForm').html(response);
                $.validator.unobtrusive.parse($('#addSubjectForm'));
                $('#addSubjectForm').show();
            }
        });


    });


    //search
    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val().trim();
        if (searchTerm.length >= 2) {
            searchSubject(searchTerm);
        } else if (searchTerm.length === 0) {
            loadData(currentPage, pageSize)
        }
    });

    //active filter
    $('#isActiveFilter').change(function () {
        $('#nextButton').removeClass('disabled');
        currentPage = 1;
        loadData(currentPage, pageSize);
    });

    //sort filter
    $('.sortable').click(function () {
        var column = $(this).index();
        sortTable(column);
    });

    //page size
    $('#pageSize').change(function () {
        pageSize = parseInt($(this).val());
        $('#nextButton').removeClass('disabled');
        //load the first page on the pagesize change
        currentPage = 1;
        loadData(currentPage, pageSize);
    });

    //Load Data on page load
    loadData(currentPage, pageSize)

});

//back button fn
function backSubject() {
    $('#addSubjectForm').hide();
    $('#subjectDetails').show();
    $('#subjectHead').show();
    $('#addSubjectForm').find('input[type=text], input[type=number], input[type=date], select').val('');
    $('#paginationNextPrevious').show();
}

//load subject data
async function loadData(currentPage, pageSize) {
    var isActiveFilter = $('#isActiveFilter').val();

    try
    {
        var data = await $.ajax({
            url: '/Subject/AllSubjects',
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
            var enableButton = '<button type="button" class="btn btn-sm ' + enableButtonClass + '" onclick="toggleEnable(\'' + item.SubjectID + '\', ' + toggleState + ', \'' + item.Name + '\')"><i class="' + enableButtonIconClass + '" style="' + iconStyle + '"></i></button>';
            var editUrl = '/Subject/EditSubject/' + item.SubjectID;
            var deleteUrl = '/Subject/Delete/' + item.SubjectID;
            var subjectID = item.SubjectID;
            var editButton;
            var deleteButton;

            // check subject is allocated or not
            var response = await $.ajax({
                url: '/Subject/IsSubjectAllocated',
                type: 'GET',
                data: { subjectID: subjectID }
            });

            if (!response) {
                editButton = '<button type="button" class="btn btn-sm btn-primary" onclick="editSubject(\'' + item.SubjectID + '\')"><i class="bi bi-pen small-icons"></i></button> ';
                deleteButton = '<button type="button" class="btn btn-sm btn-danger" onclick="deleteSubject(\'' + item.SubjectID + '\', ' + toggleState + ', \'' + item.Name + '\')"><i class="bi bi-trash small-icons"></i></button>';
            } else {
                editButton = "";
                deleteButton = "";
            }

            var row = '<tr>' +
                '<td>' + enableButton + '</td>' +
                '<td>' + item.SubjectCode + '</td>' +
                '<td>' + item.Name + '</td>' +
                '<td>' +
                editButton + deleteButton +
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

//Subject Add
function addSubjectSuccess(response) {
    if (response.success) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: response.message,
            showCancelButton: false,
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.isConfirmed) {
                currentPage === 1;
                loadData(currentPage, pageSize);
                $('#paginationNextPrevious').show();
                $('#addSubjectForm').hide();
                $('#addSubjectForm').find('input[type=text], input[type=number], input[type=date], select').val('');
                $('#subjectDetails').show();
                $('#subjectHead').show();
            }
        });
    } else {
        Swal.fire({ icon: 'warning', title: 'Warning', text: response.message });
    }
}

function addSubjectFailure(error) {
    console.log(error);
    Swal.fire('Error!', 'Error on adding the subject', 'error');
}

//Subject delete
function deleteSubject(subjectID, state, name) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete the subject "' + name + '" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Subject/DeleteSubject/' + subjectID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#tableBody tr:has(td:contains(' + subjectID + '))').remove();
                        loadData(currentPage, pageSize)
                        Swal.fire('Deleted!', 'Record deleted successfully.', 'success');
                    } else {
                        Swal.fire('Delete Prevented!', response.msg, 'warning');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the subject.', 'error');
                }
            });
        }
    });
}

//Subject available toggle
function toggleEnable(id, enable, name) {
    Swal.fire({
        title: 'Confirmation',
        text: 'Are you sure to change the status of "' + name + '" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Subject/ToggleEnable',
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
                    Swal.fire('Error!', 'An error occurred while toggling Subject enable status.', 'error');
                }
            });
        }
    });
}


function editSubject(SubjectID) {
    $.ajax({
        url: '/Subject/AddSubject/' + SubjectID,
        type: 'GET',
        success: function (data) {
            $('#paginationNextPrevious').hide();
            $('#addSubjectForm').html(data);
            $('#addSubjectForm').show();
            $.validator.unobtrusive.parse($('#addSubjectForm'));
            $('#subjectDetails').hide();
            $('#subjectHead').hide();
        },
        error: function (error) {
            console.log(error);
            Swal.fire('Error!', 'An error occurred while fetching subject details.', 'error');
        }
    });
}

//Subject search
function searchSubject() {
    var searchTerm = $('#searchInput').val().trim();
    var searchCriteria = $('#searchCriteria').val();
    if (searchTerm.length >= 2) {
        $.ajax({
            url: '/Subject/Search',
            type: 'GET',
            data: { searchTerm: searchTerm, searchCriteria: searchCriteria },
            success: function (data) {
                $('#tableBody').html(data);
            },
            error: function (error) {
                console.log(error);
                Swal.fire('Error!', 'An error occurred while searching subjects.', 'error');
            }
        });
    } else if (searchTerm.length === 0) {
        loadData(currentPage, pageSize)
    }
}

//Subject sort
var sortAscending = true;

function sortTable(column) {
    var tableRows = $('#subjectDetails tbody tr').get();
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