function filterHandler(){
    

}

function ddlCategory(listCategory){
    //populate category dropdown
    $.each(listCategory, function (key, value) {
        $('#homeCategory').append($("<option value='" + value.id + "'>" + value.categoryname + "</option>"));
    });
}

function filterbyName(listProject){
    $('#txtSearchPName').on('keyup', function () {
        var filterPname = $('#txtSearchPName').val();
        var arrPname = [];

        $.each(listProject, function () {
            if (filterPname == this.projectname) {
                arrPname = this;
            }
            if(this.projectname.search(filterPname)!=-1){
                arrPname.push(this);
            }
        });

        if (arrPname.length == 0) {
            $('#filterError').show();
            $('#filterAll').hide();
            $('#resultFilter').hide();
        }
        else {
            $('#filterAll').hide();
            $('#filterError').hide();
            loopArrayKey(arrPname);
        }
    });
}

function filterbyCategory(listProject){
    $('#homeCategory').on('change', function () {
        var filterPcategory = $('#homeCategory').val();
        var arrPcategory = [];

        $.each(listProject, function () {
            if (filterPcategory == this.fkcategory.id) {
                arrPcategory.push(this);
            }
        });

        if (arrPcategory.length == 0) {
            $('#filterError').show();
            $('#filterAll').hide();
            $('#resultFilter').hide();
        }
        else {
            $('#filterAll').hide();
            $('#filterError').hide();
            loopArrayKey(arrPcategory);
        }
    });
}

function filterbyAmount(listProject){
    var arrRange1=[], arrRange2=[], arrRange3=[], arrRange4=[];

    $.each(listProject, function () {
        if (this.projectamounttoraise >= 10 && this.projectamounttoraise <= 500) { arrRange1.push(this);}
        else if (this.projectamounttoraise > 500 && this.projectamounttoraise <= 1000) { arrRange2.push(this); }
        else if (this.projectamounttoraise > 1000 && this.projectamounttoraise <= 2000) { arrRange3.push(this); }
        else if (this.projectamounttoraise > 2000) { arrRange4.push(this); }
    });

    $('#homeAmount').on('change', function () {
        var filterPamount = $('#homeAmount').val();
        
        if (filterPamount == 1) { filterArrayAmount(arrRange1); }
        else if (filterPamount == 2) { filterArrayAmount(arrRange2); }
        else if (filterPamount == 4) {filterArrayAmount(arrRange3); }
        else if (filterPamount == 4) { filterArrayAmount(arrRange4); }
        else {
            $('#filterAll').show();
            $('#resultFilter').hide();
        }
        
    });
}

function filterArrayAmount(array) {
    if (array.length == 0) {
        $('#filterError').show();
        $('#filterAll').hide();
        $('#resultFilter').hide();
    }
    else {
        $('#filterAll').hide();
        $('#filterError').hide();
        loopArrayKey(array);
    }
}

// for one result
function loopArray(array) {
    $.each(array, function () {
        var date = new Date(array.projectclosingdate);
        var strdate = fullWeekDay(date) + " " + date.getDate() + " " + fullMonth(date) + " " + date.getFullYear();

        var resultToAppend = fillCardData(array.id, array.projectname, array.projectdescription, array.projectimage1, array.fkcategory.categoryname, strdate, array.projectamounttoraise, array.projectamountraised);

        $('#resultFilter').empty();
        $('#resultFilter').append(resultToAppend);
        $('#resultFilter').show();
    });
}

//for multiple results 
function loopArrayKey(array) {
    $('#resultFilter').empty();
    $.each(array, function (key) {
        var date = new Date(array[key].projectclosingdate);
        var strdate = fullWeekDay(date) + " " + date.getDate() + " " + fullMonth(date) + " " + date.getFullYear();

        var resultToAppend = fillCardData(array[key].id, array[key].projectname, array[key].projectdescription, array[key].projectimage1, array[key].fkcategory.categoryname, strdate, array[key].projectamounttoraise, array[key].projectamountraised);
        
        $('#resultFilter').append(resultToAppend);
        $('#resultFilter').show();
    });
}

function hideFilterError() {
    $('#filterError').hide();
}

function fullWeekDay (date){
    var weeks = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    return weeks[date.getDay()]
}

function fullMonth(date) {
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return months[date.getMonth()]
}

function fillCardData(pId, strPname, strPdesc, imgSrc, strCategory, strdate, strToRaise, strRaised){
    if (strPname.length >30) {
        strPname = strPname.substring(0, 30) + " ...";
    }
    if (strPdesc.length > 80) {
        strPdesc = strPdesc.substring(0, 80) + " ...";
    }
    var percentRaised = (strRaised * 100) / strToRaise;
    percentRaised = percentRaised.toFixed(2);
    var strRange = parseInt(percentRaised) + "%";
    var strHref = "/project/Details/" + pId;
    var resultToAppend = '<a href="' + strHref + '" class="card-anchor-container"><div class="card"><div class="card-header"><img src="' + imgSrc + '" alt="projectimage" class="card-img-top img-responsive" /><span class="card-date">closing on ' + strdate + '</span></div><div class="card-body"><h3 class="card-title">' + strPname + '</h3><p class="card-text"> ' + strPdesc + '</p><p>Category: ' + strCategory + '</p><span class="pull-left meta-card">$ ' + strRaised + ' raised of  $ ' + strToRaise + ' </span><div class="progress"><div class="progress-bar progress-bar-striped" role="progressbar" style="width: ' + strRange + '" aria-valuemin="0" aria-valuemax="100"></div></div><span class="pull-right meta-card">' + percentRaised +' % funded </span></div></div ></a >';
    
    return resultToAppend;
}

function relatedCategory(listProject, category, pname) {
    var arrResult = [];

    $.each(listProject, function () {
        if (category == this.fkcategory.id && pname != this.projectname) {
            arrResult.push(this);
        }
    });

    // if no project with same category projects with closest closing dates 
    if (arrResult.length == 0) {
        $.each(listProject, function () {
            //convert date to timestamp
            time = this.projectclosingdate,
                date = new Date($.trim(time)),
                timestamp = date.getTime(); // Unix timestamp

            //replace closing date by the timeStamp value for comparison
            this.projectclosingdate = timestamp;
        });

        // sort the array by closing dates 
        listProject.sort(function (obj1, obj2) {
            return obj1.projectclosingdate - obj2.projectclosingdate;
        });

        loopArrayRelatedCategory(listProject);
        
    }
    else {
        loopArrayRelatedCategory(arrResult);
    }
}

function loopArrayRelatedCategory(arrResult){
    $('#relatedProjects').empty();
    $.each(arrResult, function (key) {
        if(key<=2){
            var date = new Date(arrResult[key].projectclosingdate);
            var strdate = fullWeekDay(date) + " " + date.getDate() + " " + fullMonth(date) + " " + date.getFullYear();
            var strImg = "../../"+arrResult[key].projectimage1;
            var strHref = "/project/Details/" + arrResult[key].id;
            var strPname = ""+arrResult[key].projectname;
            if (strPname.length > 30 ) {
                strPname = strPname.substring(0, 30) + " ...";
            }

            var percentRaised = (arrResult[key].projectamountraised * 100) / arrResult[key].projectamounttoraise;
            percentRaised = percentRaised.toFixed(2);
            var strRange = parseInt(percentRaised) + "%";
            

            var resultToAppend = '<a href="' + strHref + '" class="card-anchor-container"><div class="card small-card"><div class="card-header" style = "background: url(\'' + strImg + '\');background-size: cover;"><span class="card-date">closing on ' + strdate + '</span><div class="card-title"><h3>' + strPname + '</h3><p class="category-tag">' + arrResult[key].fkcategory.categoryname + '</p><p>$ ' + arrResult[key].projectamountraised + ' raised of  $ ' + arrResult[key].projectamounttoraise + ' </p></div><div class="progress"><div class="progress-bar progress-bar-striped" role="progressbar" style="width: ' + strRange + '" aria-valuemin="0" aria-valuemax="100"></div><div class="card-meta"><span>' + percentRaised + ' % funded </span> </div></div></div></div></a >';

            $('#relatedProjects').append(resultToAppend);
        }

        
    });
}


