
$(document).ready(function () {

    $('#isActiveFilter').change(function () {
        loadStudentAllocationData()
    });

    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val().trim();
        if (searchTerm.length >= 2) {
            searchStudentAllocation(searchTerm);
        } else if (searchTerm.length === 0) {
            loadStudentAllocationData()
        }
    });


    $('#searchInputSub').on('input', function () {
        var searchTerm = $(this).val().trim();
        if (searchTerm.length >= 2) {
            searchSubjectAllocation(searchTerm);
        } else if (searchTerm.length === 0) {
            loadSubjectAllocationData()
        }
    });


    //Add new subject allocation button
    $('#addNewSubjectAllocation').click(function () {
        $('#addTeacherSubjectAllocationForm').show();
        $('#subjectAllocationDetails').hide();
        $('#addNewSubjectAllocation').hide();
        $('.search_bar2').hide();
        $.ajax({
            url: '/Allocation/AddTeacherSubjectAllocation',
            type: 'GET',
            success: function (response) {
                $('#addTeacherSubjectAllocationForm').html(response);
                activeSubjectTeacher();
                $.validator.unobtrusive.parse($('#addTeacherSubjectAllocationForm'));
                $('#addTeacherSubjectAllocationForm').show();
            }
        });


    });

    //Add new student allocation button
    $('#addNewStudentAllocation').click(function () {
        $('#addStudentAllocationForm').show();
        $('#studentAllocationDetails').hide();
       /* $('#addNewStudentAllocation').hide();*/
        $('.search_bar').hide();
        $.ajax({
            url: '/Allocation/AddStudentAllocation',
            type: 'GET',
            success: function (response) {
                $('#addStudentAllocationForm').html(response);
                activeSubjectTeacher();
                $.validator.unobtrusive.parse($('#addStudentAllocationForm'));
                $('#addStudentAllocationForm').show();
            }
        });
    });

    //Load Data on page load
    loadSubjectAllocationData();
    loadStudentAllocationData();

});



function loadSubjectAllocationData() {
    $.ajax({
        url: '/Allocation/AllSubjectAllocation',
        type: 'GET',
        success: function (data) {
            $('#subjectAllocationTableBody').html(data);
            /* console.log(data);*/
        },
        error: function (error) {
            console.log(error);
            alert('An error occurred while loading data.');
        }
    });
}

function addTeacherSubjectAllocationSuccess(response) {
    if (response.success) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: response.message,
            showCancelButton: false,
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.isConfirmed) {
                loadSubjectAllocationData();

                $('.search_bar2').show();
                $('#addTeacherSubjectAllocationForm').hide();
                $('#addTeacherSubjectAllocationForm').find('select').val('');
                $('#subjectAllocationDetails').show();
                $('#addNewSubjectAllocation').show();
            }
        });
    } else {
        Swal.fire({ icon: 'warning', title: 'Warning', text: response.message });
    }
}

function addTeacherSubjectAllocationFailure(xhr, status, error) {
    console.error("Error adding teacher subject allocation:", error);
    Swal.fire('Error!', 'An error occurred while adding the teacher subject allocation.', 'error');
}


function backSubjectAllocation() {
    $('#addTeacherSubjectAllocationForm').hide();
    $('#addTeacherSubjectAllocationForm').find('select').val('');
    $('#subjectAllocationDetails').show();
    $('#addNewSubjectAllocation').show();
    $('.search_bar2').show();
}


function deleteSubjectAllocation(subjectAllocationID) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete this Allocation ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Allocation/DeleteSubjectAllocation/' + subjectAllocationID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#subjectAllocationTableBody tr:has(td:contains(' + subjectAllocationID + '))').remove();
                        loadSubjectAllocationData();
                        Swal.fire('Deleted!', 'Allocation deleted successfully.', 'success');
                    } else {
                        Swal.fire('Delete Prevented!', response.message, 'warning');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the Allocation.', 'error');
                }
            });
        }
    });
}

//editSubjectAllocation
function editSubjectAllocation(subjectAllocationID) {
    $.ajax({
        url: '/Allocation/AddTeacherSubjectAllocation/' + subjectAllocationID,
        type: 'GET',
        success: function (data) {
            $('#addTeacherSubjectAllocationForm').html(data);
            $('#addTeacherSubjectAllocationForm').show();
            $.validator.unobtrusive.parse($('#addTeacherSubjectAllocationForm'));
            $('#subjectAllocationDetails').hide();
            $('#addNewSubjectAllocation').hide();
            $('.search_bar2').hide();
        },
        error: function (error) {
            console.log(error);
            Swal.fire('Error!', 'An error occurred while fetching student details.', 'error');
        }
    });
}
//searchSubjectAllocation
function searchSubjectAllocation() {
    var searchTerm = $('#searchInputSub').val().trim();
    var searchCriteria = $('#searchCriteriaSub').val();
    if (searchTerm.length >= 2) {
        $.ajax({
            url: '/Allocation/SubjectAllocationSearch',
            type: 'GET',
            data: { searchTerm: searchTerm, searchCriteria: searchCriteria },
            success: function (data) {
                $('#subjectAllocationTableBody').html(data);
            },
            error: function (error) {
                console.log(error);
                Swal.fire('Error!', 'An error occurred while searching subject.', 'error');
            }
        });
    } else if (searchTerm.length === 0) {
        loadSubjectAllocationData();
    }
}


/*-------------------------------------------------------------------------STUDENT ALLOCATIONS PARTS--------------------------------------------------------------------------------------------*/

function backStudentAllocation() {
    $('#addStudentAllocationForm').hide();
    $('#addStudentAllocationForm').find('select').val('');
    $('#subjectAllocationID').val('');
    $('#studentAllocationDetails').show();
    $('.search_bar').show();
    $('#studentAllocationSubMenu').show();
    
}

//Load Student Allocation
function loadStudentAllocationData() {
    var isActiveFilter = $('#isActiveFilter').val();
    $.ajax({
        url: '/Allocation/AllStudentAllocation',
        type: 'GET',
        data: { isActive: isActiveFilter },
        success: function (data) {
            $('#studentAllocationTableBody').html(data);
            /*console.log(data);*/
            activeSubjectTeacher();
        },
        error: function (error) {
            console.log(error);
            alert('An error occurred while loading data.');
        }
    });
}

//Toggle student enable
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
                        loadStudentAllocationData() 
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



//Add Student allocation
function addStudentAllocationSuccess(response) {
    if (response.success) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: response.message,
            showCancelButton: false,
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.isConfirmed) {
                loadStudentAllocationData();
                $('#addStudentAllocationForm').hide();
                $('#subjectAllocationID').val('');
                $('#addStudentAllocationForm').find('select').val('');
                $('#studentAllocationDetails').show();
                $('#addNewStudentAllocation').show();
                $('.search_bar').show();
            }
        });
    } else {
        Swal.fire({ icon: 'warning', title: 'Warning', text: response.message, error: response.error });
    }
}

function addStudentAllocationFailure(xhr, status, error) {
    console.error("Error adding teacher subject allocation:", error);
    Swal.fire('Error!', 'An error occurred while adding the teacher subject allocation.', 'error');
}


function deleteStudentAllocation(studentAllocationID) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete this Student Allocation ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Allocation/DeleteStudentAllocation/' + studentAllocationID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#studentAllocationTableBody tr:has(td:contains(' + studentAllocationID + '))').remove();
                        loadStudentAllocationData();
                        Swal.fire('Deleted!', 'Student Allocation deleted successfully.', 'success');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the Allocation.', 'error');
                }
            });
        }
    });
}

function deleteAllStudentAllocation(studentID,name) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure you want to delete All Student Allocation for "'+name+'" ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Allocation/DeleteAllStudentAllocation/' + studentID,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $('#studentAllocationTableBody tr:has(td:contains(' + studentID + '))').remove();
                        loadStudentAllocationData();
                        Swal.fire('Deleted!', 'Student Allocation deleted successfully.', 'success');
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire('Error!', 'An error occurred while deleting the Allocation.', 'error');
                }
            });
        }
    });
}


//editSubjectAllocation
function editStudentAllocation(studentAllocationID) {
    $.ajax({
        url: '/Allocation/AddStudentAllocation/' + studentAllocationID,
        type: 'GET',
        success: function (data) {
            $('#addStudentAllocationForm').html(data);
            $('#addStudentAllocationForm').show();
            $.ajax({
                url: '/Allocation/GetAllocatedSubject',
                type: 'GET',
                success: function (response) {
                    if (response.success) {
                        $.each(response.data, function (index, item) {
                            $('#subjectDropdown').append($('<option>', {
                                value: item.SubjectID,
                                text: item.Name
                            }));
                        });
                    } else {
                        console.log('No data found.');
                    }
                },
                error: function () {
                    console.log('Error fetching allocated subjects.');
                }
            });
            $.validator.unobtrusive.parse($('#addStudentAllocationForm'));
            $('#studentAllocationDetails').hide();
            $('#addNewStudentAllocation').hide();
        },
        error: function (error) {
            console.log(error);
            Swal.fire('Error!', 'An error occurred while fetching student allocation details.', 'error');
        }
    });
}


//Available subject and teacher
function activeSubjectTeacher() {
    $.ajax({
        url: '/Allocation/GetAllocatedSubject',
        type: 'GET',
        success: function (response) {
            $('#subjectDropdown').empty().append($('<option>', {
                value: '',
                text: '-Select Subject-'
            }));
            $('#teacherDropdown').empty().append($('<option>', {
                value: '',
                text: '-Select Teacher-'
            }));

            if (response.success) {
                $.each(response.data, function (index, item) {
                    $('#subjectDropdown').append($('<option>', {
                        value: item.SubjectID,
                        text: item.Name
                    }));
                });
            } else {
                console.log('No data found.');
            }
        },
        error: function () {
            console.log('Error fetching allocated subjects.');
        }
    });
}
//search
//search student
function searchStudentAllocation() {
    var searchTerm = $('#searchInput').val().trim();
    var searchCriteria = $('#searchCriteria').val();
    if (searchTerm.length >= 2) {
        $.ajax({
            url: '/Allocation/StudentAllocationSearch',
            type: 'GET',
            data: { searchTerm: searchTerm, searchCriteria: searchCriteria },
            success: function (data) {
                $('#studentAllocationTableBody').html(data);
            },
            error: function (error) {
                console.log(error);
                Swal.fire('Error!', 'An error occurred while searching teacher.', 'error');
            }
        });
    } else if (searchTerm.length === 0) {
        loadStudentAllocationData();
    }
}

