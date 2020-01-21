function shareHandler () {
  /* ----------- User Edit Shares ------------------------- */
  //ordinary
  $("#txtNbShareOrdinary").on("keyup", function () {
    $("#pNbShareOrdinary").text($("#txtNbShareOrdinary").val());
  });

  $("#txtShareValueOrdinary").on("keyup", function () {
    $("#pShareValueOrdinary").text($("#txtShareValueOrdinary").val());
  });

  //preferencial
  $("#txtNbSharePreferencial").on("keyup", function () {
    $("#pNbSharePreferencial").text($("#txtNbSharePreferencial").val());
  });

  $("#txtShareValuePreferencial").on("keyup", function () {
    $("#pShareValuePreferencial").text($("#txtShareValuePreferencial").val());
  });

  //non voting
  $("#txtNbShareNonvoting").on("keyup", function () {
    $("#pNbShareNonvoting").text($("#txtNbShareNonvoting").val());
  });

  $("#txtShareValueNonvoting").on("keyup", function () {
    $("#pShareValueNonvoting").text($("#txtShareValueNonvoting").val());
  });

  //redeemable
  $("#txtNbShareRedeemable").on("keyup", function () {
    $("#pNbShareRedeemable").text($("#txtNbShareRedeemable").val());
  });

  $("#txtShareValueRedeemable").on("keyup", function () {
    $("#pShareValueRedeemable").text($("#txtShareValueRedeemable").val());
  });

  /* ----------- Project Detail shares ------------------------- */
  var sliderOrdinary = $("#ordinaryRange");
  var shareNbOrdinary = $("#ordinaryValue");
  var shareValueOrdinary = $('#sharevalueOrdinaryDetailsP');
  var priceOrdinary = $('#priceOrdinary');
  var tempOrdinary = $('#tempOrdinary');
  var bulletOrdinary = $('.priceShareOrdinary');
  var subOrdinary = $('#subOrdinary');
  sliderRange(sliderOrdinary, shareNbOrdinary, shareValueOrdinary, priceOrdinary, tempOrdinary, bulletOrdinary, subOrdinary);

  var sliderPreferencial = $("#preferencialRange");
  var shareNbPreferencial = $("#preferencialValue");
  var shareValuePreferencial = $('#sharevaluePreferencialDetailsP');
  var pricePreferencial = $('#pricePreferencial');
  var tempPreferencial = $('#tempPreferencial');
  var bulletPreferencial = $('.priceSharePreferencial');
  var subPreferencial = $('#subPreferencial');
  sliderRange(sliderPreferencial, shareNbPreferencial, shareValuePreferencial, pricePreferencial, tempPreferencial, bulletPreferencial, subPreferencial);

  var sliderNonvoting = $("#nonvotingRange");
  var shareNbNonvoting = $("#nonvotingValue");
  var shareValueNonvoting = $('#sharevalueNonvotingDetailsP');
  var priceNonvoting = $('#priceNonvoting');
  var tempNonvoting = $('#tempNonvoting');
  var bulletNonvoting = $('.priceShareNonvoting');
  var subNonvoting = $('#subNonvoting');
  sliderRange(sliderNonvoting, shareNbNonvoting, shareValueNonvoting, priceNonvoting, tempNonvoting, bulletNonvoting, subNonvoting);

  var sliderRedeemable = $("#redeemableRange");
  var shareNbRedeemable = $("#redeemableValue");
  var shareValueRedeemable = $('#sharevalueRedeemableDetailsP');
  var priceRedeemable = $('#priceRedeemable');
  var tempRedeemable = $('#tempRedeemable');
  var bulletRedeemable = $('.priceShareRedeemable');
  var subRedeemable = $('#subRedeemable');
  sliderRange(sliderRedeemable, shareNbRedeemable, shareValueRedeemable, priceRedeemable, tempRedeemable, bulletRedeemable, subRedeemable);

  $(".slider").on("change", function(){
    var strOrdinary = priceOrdinary.text();
    var dotOrdinary = strOrdinary.replace(",", ".");
    getNum(dotOrdinary);

    var strPreferencial = pricePreferencial.text();
    var dotPreferencial = strPreferencial.replace(",", ".");
    getNum(dotPreferencial);

    var strNonvoting = priceNonvoting.text();
    var dotNonvoting = strNonvoting.replace(",", ".");
    getNum(dotNonvoting);

    var strRedeemable = priceRedeemable.text();
    var dotRedeemable = strRedeemable.replace(",", ".");
    getNum(dotRedeemable);

    var total = Number(dotOrdinary) + Number(dotPreferencial) + Number(dotNonvoting) + Number(dotRedeemable); 
  
    $("#totalPrice").text(""+total.toLocaleString());
    $('#tempTotalPrice').val("" + total.toLocaleString());
  });
  
}

function sliderRange(slider, shareNb, shareValue, price, tempPrice, bullet, substract) {
  shareNb.text(slider.val());

  slider.on('change', function () {
    shareNb.text(slider.val());
    var strShareValue = shareValue.text();
    var dotShareValue = strShareValue.replace(",", ".");
    var doublePrice = Number(shareNb.text() * dotShareValue).toLocaleString();
    price.text(doublePrice);
    tempPrice.val(price.text());
    var substractedNbShare = slider[0].max - slider.val();
    substract.val(substractedNbShare);
    
    //slider.val() --- bullet value
    var mleft = (slider.val()*100/slider[0].max-5);
    if(mleft <0){mleft=0;}
    else if(mleft>92){mleft=92;}
    bullet.css('margin-left', mleft+"%");
  });

}

function transformTags(tagsArray, ulTags) {
  var strOrdinary = tagsArray.html();
  var ordinaryTags = strOrdinary.split(",");

  ordinaryTags.forEach(element => {
    ulTags.append("<li>" + element + "</li>");
  });
}

function getNum(str){
  if (str == null || str == ""){
    return 0;
  }
  else {
    str = parseFloat(str);
    return str;
  }
}