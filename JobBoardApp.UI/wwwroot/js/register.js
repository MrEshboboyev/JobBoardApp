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

    // Utility function to validate email format (basic regex for email validation)
    function validateEmailFormat(email) {
        var emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
        return emailPattern.test(email);
    }

    // Debounce function to limit how often a function is called
    function debounce(func, delay) {
        var timer;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                func.apply(context, args);
            }, delay);
        };
    }

    // Role selection event handler
    $('#Role').change(function () {
        var role = $(this).val();
        if (role === "Employer") {
            $('#EmployerFields').show();
            $('#JobSeekerFields').hide();
        } else if (role === "JobSeeker") {
            $('#EmployerFields').hide();
            $('#JobSeekerFields').show();
        } else {
            $('#EmployerFields').hide();
            $('#JobSeekerFields').hide();
        }

        // Validate the form when the role changes
        validateFormFields();
    });

    // AJAX Email uniqueness check (with validation and debounce)
    $('#Email').blur(debounce(function () {
        var email = $(this).val();

        // Only proceed if the email is not empty and is valid
        if (email.length > 0 && validateEmailFormat(email)) {
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
        } else {
            $('#Email').addClass('is-invalid');
            emailValid = false;
            checkFormValidity();
        }
    }, 5000)); // 500ms delay before checking email

    // AJAX Username uniqueness check
    $('#UserName').blur(function () {
        var username = $(this).val();

        // Only check if the username is not empty
        if (username.length > 0) {
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
        } else {
            $('#UserName').addClass('is-invalid');
            usernameValid = false;
            checkFormValidity();
        }
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
