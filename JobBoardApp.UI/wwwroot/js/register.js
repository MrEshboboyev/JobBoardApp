$(document).ready(function () {
    var emailValid = false;
    var usernameValid = false;
    var formValid = false;

    // Function to check if the form is valid and enable/disable the register button
    function checkFormValidity() {
        if (emailValid && usernameValid && formValid) {
            $('#registerButton').prop('disabled', false);
        } else {
            $('#registerButton').prop('disabled', true);
        }
    }

    // Role selection event handler
    $('#Role').change(function () {
        var role = $(this).val();
        if (role === "Employer") {
            $('#EmployerFields').show();
            $('#BioField').show();
        } else if (role === "JobSeeker") {
            $('#EmployerFields').hide();
            $('#BioField').show();
        } else {
            $('#EmployerFields').hide();
            $('#BioField').hide();
        }

        // Validate the form when the role changes
        validateFormFields();
    });

    // AJAX Email uniqueness check
    $('#Email').blur(function () {
        var email = $(this).val();
        $.ajax({
            url: '/Account/CheckEmail', // Endpoint to check email
            type: 'GET',
            data: { email: email },
            success: function (response) {
                if (response.exists) {
                    alert("Email already exists!");
                    $('#Email').addClass('is-invalid');
                    emailValid = false;
                } else {
                    $('#Email').removeClass('is-invalid');
                    emailValid = true;
                }
                checkFormValidity(); // Check if the form is valid after email validation
            },
            error: function () {
                console.log("Error checking email.");
                emailValid = false;
                checkFormValidity();
            }
        });
    });

    // AJAX Username uniqueness check
    $('#UserName').blur(function () {
        var username = $(this).val();
        $.ajax({
            url: '/Account/CheckUsername', // Endpoint to check username
            type: 'GET',
            data: { username: username },
            success: function (response) {
                if (response.exists) {
                    alert("Username already exists!");
                    $('#UserName').addClass('is-invalid');
                    usernameValid = false;
                } else {
                    $('#UserName').removeClass('is-invalid');
                    usernameValid = true;
                }
                checkFormValidity(); // Check if the form is valid after username validation
            },
            error: function () {
                console.log("Error checking username.");
                usernameValid = false;
                checkFormValidity();
            }
        });
    });

    // Other form fields validation
    $('#registerForm').on('input', function () {
        validateFormFields();
    });

    // Function to validate the form fields and set formValid
    function validateFormFields() {
        formValid = $('#registerForm')[0].checkValidity(); // Uses HTML5 form validation
        checkFormValidity(); // Check if the form is valid after validating other fields
    }
});
