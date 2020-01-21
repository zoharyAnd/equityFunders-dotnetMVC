// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function (){
    validationHandler();
    shareHandler();
    filterHandler();
    loadMore();
    
    
    $( "#userdob" ).datepicker({ dateFormat: 'mm/dd/yy' });
    $('.community-slick').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3
      });
    
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth()+1;
    var yyyy = today.getFullYear();

    if(dd<10) {
        dd = '0'+dd
    } 

    if(mm<10) {
        mm = '0'+mm
    } 

    today = yyyy + '-' + mm + '-' + dd;
    

    $("#projectcreationdate").val(today); 
    //bind country select
    $.ajax({
        type: "GET",
        url: "/country.xml",
        dataType: "xml",
        success: function (xml) {
            var selectCreate = $('#countriesCreate');
            var selectEdit = $('#countriesEdit');
            var optionIni = $("<option value='Select'>---- Select a country ----</option>");
            selectCreate.append(optionIni);
            selectEdit.append(optionIni);
            $(xml).find('country').each(function () {
                var name = $(this).attr('countryName');
                var option = $("<option>" + name + "</option>");
                option.attr("value", name);
                selectCreate.append(option);
                selectEdit.append(option);
            });
            /*
            selectCreate.children(":first").text("------Select a country ------").attr("selected", true);
            selectEdit.children(":first").text("------Select a country ------").attr("selected", true);
            */
        }
    }); 
    $('.paypalEmailError').css('display', 'none');
    var paypalEmailFlag = false;
    var tempPriceFlag = false;

    $('.donationPanel input').on('change', function(){
       if ($('#tempTotalPrice').val()==''){
           tempPriceFlag = false;
       }
       else {
           tempPriceFlag = true;
       }

       if(tempPriceFlag == true && paypalEmailFlag == true){
           $('#inputDonate').attr('disabled', false);
       }
       else {
            $('#inputDonate').attr('disabled', true);
       }
        
    });

    $('#txtPaymentEmail').on('keyup', function(){
        var txtPaymentEmail = $('#txtPaymentEmail').val();
        $('#paymentEmail').val(txtPaymentEmail);

        var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        
        if (!expr.test(txtPaymentEmail)) {
            $('.paypalEmailError').css('display', 'block');
            paypalEmailFlag = false;
        }
        else 
        {
            $('.paypalEmailError').css('display', 'none');
            paypalEmailFlag=true;
        }
    });

    $('.contactModalTrigger').on('click', function(){
        var senderEmail = $(this).parent().parent().find('.senderEmail p').text();
        $('#destinatorEmail').val(senderEmail);
    });
});



function showLoginModal() {
    $('#loginModal').modal();
}

function clearLogin(){
    $('#loginError').text('');
    $('#loginForm input').val('');
}

function clearRegistration(){
    $('#registerForm input').val('');
}

function clearContactUs(){
    $('#contactUsReply input, #contactUsReply textarea').val('');

}


function getUsercountryCreate() {
    $('#usercountryCreate').val($('#countriesCreate').val());
}

function getUsercountryEdit() {
    $('#usercountryEdit').val($('#countriesEdit').val());
}

function getCategoryIdProjectEdit () {
    $('fkcategory').val($('#projectCategoryCreate').val());
}

function loadMore(){
    $('#btnLoadMore').on('click', function(){
        var h = $('.card-container').height();
        h = h+517;

        if(h == 1046){
            $('.card-container').height(h);
            $('#btnLoadMore').css("display", "none");
            $('#btnViewAllP').css("display", "inline-block");            
        }
        else {
            $('.card-container').height(h);
        }
    });
}

function computeAge(strDob){
    var today = new Date();

    var dd = today.getDate();
    var mm = today.getMonth()+1;
    var yyyy = today.getFullYear();

    if(dd<10) {
        dd = '0'+dd
    } 

    if(mm<10) {
        mm = '0'+mm
    } 

    var strToday = mm + '/' + dd + '/' + yyyy;
    

    if(strDob == strToday){
        return ""; 
    }
    else{
        var splitted = strDob.split("/");
        //year, day, month
        var dob = new Date(splitted[2], splitted[0], splitted[1]-1);
        
        var age = Math.floor((today-dob) / (365.25 * 24 * 60 * 60 * 1000));
        return age+" years old";
        }
    
}