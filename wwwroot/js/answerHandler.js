function handleanswer(replybtn){
    e.preventDefault();
    var fkquestion = $(replybtn).parent().find('.fkquestion').val();
    var fkuser = $(replybtn).parent().find('.fkuser').val();
    var answerdate = $(replybtn).parent().find('.answerdate').val();
    var answermessage = $(replybtn).parent().find('.answermessage').val();

    var data = '';
    console.log('question: '+fkquestion);
    console.log('user: '+fkuser);
    console.log('date: '+answerdate);
    console.log('message: '+answermessage);

    
    $.ajax({
        type: "POST",
        url: "/answer/Create/",
        data: JSON.stringify({ 
            'fkquestion': fkquestion, 
            'fkuser': fkuser,
            'answerdate':answerdate,
            'answermessage':answermessage
          }),
          contentType: "application/json",
          dataType: "json",
        success: function(){
            alert('data posted');
        },
        async: true,
        processData: false
    });
}

