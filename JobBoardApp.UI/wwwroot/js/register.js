document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('startRegistration').addEventListener('click', function () {
        console.log("Event listener for Start Registration button added!");
        startRegistration();
    });
});

function startRegistration() {
    // Step 1: Enter Username and Email
    Swal.fire({
        title: 'Step 1: Username and Email',
        html: `<input type="text" id="username" class="swal2-input" placeholder="Username">
               <input type="email" id="email" class="swal2-input" placeholder="Email">`,
        confirmButtonText: 'Next',
        showCancelButton: true,
        preConfirm: () => {
            const username = Swal.getPopup().querySelector('#username').value;
            const email = Swal.getPopup().querySelector('#email').value;
            if (!username || !email) {
                Swal.showValidationMessage(`Please enter both username and email`);
            }

            return fetch(`/Account/CheckUsernameEmail?username=${username}&email=${email}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isUsernameTaken || data.isEmailTaken) {
                        Swal.showValidationMessage(
                            `Username or Email is already taken`
                        );
                    }
                    return { username: username, email: email };
                })
                .catch(() => {
                    Swal.showValidationMessage(`Request failed`);
                });
        }
    }).then((result) => {
        if (result.isConfirmed) {
            step2Password(result.value.username, result.value.email);
        }
    });
}

function step2Password(username, email) {
    Swal.fire({
        title: 'Step 2: Password',
        html: `<input type="password" id="password" class="swal2-input" placeholder="Password">
               <input type="password" id="confirmPassword" class="swal2-input" placeholder="Confirm Password">
               <p>Username: ${username}, Email: ${email}</p>`,
        confirmButtonText: 'Next',
        showCancelButton: true,
        preConfirm: () => {
            const password = Swal.getPopup().querySelector('#password').value;
            const confirmPassword = Swal.getPopup().querySelector('#confirmPassword').value;
            if (!password || !confirmPassword || password !== confirmPassword) {
                Swal.showValidationMessage(`Passwords must match and cannot be empty`);
            }
            return { password: password };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            step3Profile(username, email, result.value.password);
        }
    });
}

function step3Profile(username, email, password) {
    Swal.fire({
        title: 'Step 3: Profile Information',
        html: `<input type="url" id="website" class="swal2-input" placeholder="Website">
               <textarea id="bio" class="swal2-input" placeholder="Bio"></textarea>
               <select id="role" class="swal2-input">
                   <option value="Employer">Employer</option>
                   <option value="JobSeeker">Job Seeker</option>
               </select>`,
        confirmButtonText: 'Next',
        showCancelButton: true,
        preConfirm: () => {
            const website = Swal.getPopup().querySelector('#website').value;
            const bio = Swal.getPopup().querySelector('#bio').value;
            const role = Swal.getPopup().querySelector('#role').value;
            if (!role) {
                Swal.showValidationMessage(`Please select a role`);
            }
            return { website: website, bio: bio, role: role };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            step4RoleSpecific(username, email, password, result.value.website, result.value.bio, result.value.role);
        }
    });
}

function step4RoleSpecific(username, email, password, website, bio, role) {
    if (role === 'Employer') {
        Swal.fire({
            title: 'Step 4: Employer Information',
            html: `<input type="text" id="companyName" class="swal2-input" placeholder="Company Name">`,
            confirmButtonText: 'Submit',
            showCancelButton: true,
            preConfirm: () => {
                const companyName = Swal.getPopup().querySelector('#companyName').value;
                if (!companyName) {
                    Swal.showValidationMessage(`Please enter your company name`);
                }
                return submitRegistration(username, email, password, website, bio, role, companyName);
            }
        });
    } else if (role === 'JobSeeker') {
        Swal.fire({
            title: 'Step 4: Job Seeker Information',
            html: `<input type="file" id="resume" class="swal2-input" placeholder="Upload Resume">`,
            confirmButtonText: 'Submit',
            showCancelButton: true,
            preConfirm: () => {
                const resume = Swal.getPopup().querySelector('#resume').files[0];
                if (!resume) {
                    Swal.showValidationMessage(`Please upload your resume`);
                }
                return submitRegistration(username, email, password, website, bio, role, resume);
            }
        });
    }
}

function submitRegistration(username, email, password, website, bio, role, additionalInfo) {
    const formData = new FormData();
    formData.append('username', username);
    formData.append('email', email);
    formData.append('password', password);
    formData.append('website', website);
    formData.append('bio', bio);
    formData.append('role', role);

    if (role === 'Employer') {
        formData.append('companyName', additionalInfo);
    } else {
        formData.append('resume', additionalInfo);
    }

    fetch('/Account/Register', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire('Registration Successful!', '', 'success').then(() => {
                window.location.href = '/Account/Login';
            });
        } else {
            Swal.fire('Error', 'Registration failed', 'error');
        }
    }).catch(() => {
        Swal.fire('Error', 'An error occurred while registering', 'error');
    });
}
