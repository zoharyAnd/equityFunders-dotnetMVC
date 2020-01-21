/* ----------------- readUrl Edit User ------------------*/
function readUrlUphoto(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgUphoto').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlClogo(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgClogo').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};


/* ----------------- readUrl Create Project ------------------*/

function readUrlFileOne(input){
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFileOne').attr('src', e.target.result);
            $('#pc1Error').text(' ');
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlFileTwo(input){
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFileTwo').attr('src', e.target.result);
            $('#pc2Error').text(' ');
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlFileThree(input){
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFileThree').attr('src', e.target.result);
            $('#pc3Error').text(' ');
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlFileFour(input){
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFileFour').attr('src', e.target.result);
            $('#pc4Error').text(' ');
        };

        reader.readAsDataURL(input.files[0]);
    }
};


/* ----------------- readUrl Edit project ------------------*/
function readUrlOne(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgOne').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlTwo(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgTwo').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlThree(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgThree').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

function readUrlFour(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFour').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};
