function validationHandler () {
    $('#userpassword').keyup(function () {
        $('#result').html(checkStrength($('#userpassword').val(), $('#result')));
    });

    $('#userpasswordRegister').keyup(function () {
        $('#resultPRegister').html(checkStrength($('#userpasswordRegister').val(), $('#resultPRegister')));
    });	

    $('#userpasswordEditUser').keyup(function () {
        $('#resultPEditUser').html(checkStrength($('#userpasswordEditUser').val(), $('#resultPEditUser')));
    });

    
    /* --------------- login modal  validation -------------- */
    
    $('#loginForm input').on('blur', function () {
        if ($('#loginEmail').valid() && $('#loginPassword').valid() ) {
            $('#btnLogin').prop('disabled', false);
        }
        else {
            $('#btnLogin').prop('disabled', 'disabled');
        }
    });

    $('#loginEmail').on('blur', function(){
        
        $('#loginEmail').validate({
            rules: {
                required: true,
                email: true
            }
        });
    });
    
    $('#loginPassword').on('blur', function(){
        
        $('#loginPassword').validate({
            rules: {
                required: true
            }
        });
    });
    
    
    /* ----------- registration modal  validation ---------- */
    $('#registerForm input').on('blur', function () {
        if ($("#registerForm").valid() && $('#availableEmail').text() == "") {
            $('#btnRegister').prop('disabled', false);
        } else {
            $('#btnRegister').prop('disabled', 'disabled');
        }
    });

    $('#registerForm').validate({
        rules: {
            useremail: {
                required: true,
                email: true
            },
            userpassword: {
                required: true
            },
            registerCpassword: {
                required: true,
                equalTo: "#userpasswordRegister"
            }
        }
    });

    /* --------------- create user validation --------------- */
    $('#registration input').on('blur', function () {
        if ($("#registration").valid()) {
            $('#btnRegistration').prop('disabled', false);
        } else {
            $('#btnRegistration').prop('disabled', 'disabled');
        }
    });

    $("#registration").validate({
        rules: {
            fileuserphoto: {
                extension: "jped|png|jpg"
            },
            userfname: {
                required: true,
                minlength: 2,
                maxlength: 20
            },
            userlname: {
                required: true,
                minlength: 2,
                maxlength: 20
            },
            username: {
                required: true,
                minlength: 3,
                maxlength: 12
            },
            confirmpassword: {
                equalTo: "#userpassword"
            },
            userdob: {
                required: true,
                minAge: 18
            },
            companyname: {
                required: true,
                minlength: 3,
                maxlength: 30
            },
            filecompanylogo: {
                extension: "jped|png|jpg"
            }
        },
        message: {
            userfname: {
                required: "Please give a first name",
                minlength: "First name at least 3 chars",
                maxlength: "First name max 12 chars"
            },
            userlname: {
                required: "Please give a last name",
                minlength: "Last name at least 3 chars",
                maxlength: "Last name max 12 chars"
            },
            username: {
                required: "Please give a username",
                minlength: "Username at least 3 chars",
                maxlength: "Username max 12 chars"
            },
            confirmpassword: {
                equalTo: "Not matching the password"
            },
            file: {
                extension: "Please provide a valid image"
            },
            userdob: {
                required: "Please give a date of birth",
                minAge: "Not allowed you have than 18 years old"
            },
            companyname: {
                required: "Please give a company name",
                minlength: "Company name at least 3 chars",
                maxlength: "Company name max 12 chars"
            }

        }

    });

    /* --------------- edit user validation: Edit profile --------------- */
    $('#userpasswordEditUser').on('keyup', function(){
        $('#editUserProfile #userpassword').val($(this).val());
    });

    $('#editUserProfile input').on('blur', function () {
        if ($("#editUserProfile").valid()) {
            $('#btnEditUserProfile').prop('disabled', false);
        } else {
            $('#btnEditUserProfile').prop('disabled', 'disabled');
        }
    });

    $("#editUserProfile").validate({
        rules: {
            fileuserphoto: {
                extension: "jped|png|jpg"
            },
            username: {
                required: true,
                minlength: 3,
                maxlength: 12
            },
            userfname: {
                required: true,
                minlength: 2,
                maxlength: 20
            },
            userlname: {
                required: true,
                minlength: 2,
                maxlength: 20
            },
            useremail: {
                required: true,
                email: true
            },
            userpasswordEditUser: {
                required: true
            },
            userCpasswordEditUser: {
                required: true,
                equalTo: "#userpasswordEditUser"
            },
            userdob: {
                required: true,
                minAge: 18
            }
        }
    });

    $('#editCompanyProfile input').on('blur', function () {
        if ($("#editCompanyProfile").valid()) {
            $('#btnEditCompanyProfile').prop('disabled', false);
        } else {
            $('#btnEditCompanyProfile').prop('disabled', 'disabled');
        }
    });

    $("#editCompanyProfile").validate({
        rules: {
            filecompanylogoEdit: {
                extension: "jped|png|jpg"
            },
            companyname: {
                required: true,
                minlength: 3,
                maxlength: 30
            }
        }
    });
    /* --------------- create project  validation --------------- */
    $('#createProjectForm input').on('change', function () {
        if ($("#createProjectForm").valid()) {
            $('#btnCreateProject').prop('disabled', false);
        } 
        else {
            $('#btnCreateProject').prop('disabled', 'disabled');
        }
    });
    $("#createProjectForm").validate({
        rules: {
            projectname: {
                required: true,
                minlength: 2,
            },
            projectdescription: {
                required: true,
                minlength: 10,
                maxlength: 200
            },
            projectamounttoraise: "required",
            projectcreationdate: "required",
            projectclosingdate: {
                greaterThan: "#projectcreationdate"
            },

            fileOne: {
                extension: "jped|png|jpg"
            },
            fileTwo: {
                extension: "jped|png|jpg"
            },
            fileThree: {
                extension: "jped|png|jpg"
            },
            fileFour: {
                extension: "jped|png|jpg"
            }
        },
        messages: {
            projectname: {
                required: "Please give a project name",
                minlength: "Project name at least 2 chars"
            },
            projectdescription: {
                required: "Please give a project description",
                minlength: "Project description at least 10 chars",
                maxlength: "Project description max 200 chars"
            },
            projectamounttoraise: "Please give an amount to raise",
            projectcreationdate: "Please give a creation date",
            projectclosingdate: {
                greaterThan: "Closing date must be after today"
            }
        },
        errorPlacement: function (error, element) {
            //Custom position: first name
            if (element.attr("name") == "fileOne") {
                $("#pc1Error").text("Please provide a valid image");
            }
            else if (element.attr("name") == "fileTwo") {
                $("#pc2Error").text("Please provide a valid image");
            }
            else if (element.attr("name") == "fileThree") {
                $("#pc3Error").text("Please provide a valid image");
            }
            else if (element.attr("name") == "fileFour") {
                $("#pc4Error").text("Please provide a valid image");
            }
            else {
                error.insertAfter(element);
            }
        }

    });

}


function checkStrength(password, result) {
    var strength = 0

    if (password.length < 6) {
        result.removeClass()
        result.addClass('short')
        //return 'Too short' 
    }

    if (password.length > 7) strength += 1

    //If password contains both lower and uppercase characters, increase strength value.
    if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1

    //If it has numbers and characters, increase strength value.
    if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1

    //If it has one special character, increase strength value.
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

    //if it has two special characters, increase strength value.
    if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1


    //Calculated strength value, we can return messages

    //If value is less than 2

    if (strength < 2) {
        result.removeClass()
        result.addClass('weak')
        //return 'Weak'			
    }
    else if (strength == 2) {
        result.removeClass()
        result.addClass('good')
        //return 'Good'		
    }
    else {
        result.removeClass()
        result.addClass('strong')
        //return 'Strong'
    }
}

/*
function checkInputValidity (input){
    input.on('keyup', function () {
        if (input.valid()) {
            $('#btnLogin').prop('disabled', false);
        }
        else {
            $('#btnLogin').prop('disabled', 'disabled');
        }
    });
}
*/