

////////////////// FIX THIS JS


//Toggle The Modal Per Person
function toggleModal() {
    $('#myModal').modal('toggle');
};

//Enables The Delete buttons in the DDL
$('#DeleteDanger').on('click', function (event) {
    event.preventDefault(); // To prevent following the link (optional)
    document.getElementById('<%=BtnRemoveGuard.ClientID%>').disabled = false;
    document.getElementById('<%=BtnRemoveMembership.ClientID %>').disabled = false;
    document.getElementById('<%=BntResetPwd.ClientID %>').disabled = false;
});

//Stops The Drop Down From Closing On Clicks inside The DDL
$('.dropdown-menu').click(function (e) {
    e.stopPropagation();
});


console.log("Check For New Person: ", check);
if (check === "True") {
    var cust = <%=GetCustomer%>;
    var custList = new Array();

    custObj = cust//Parse the data

    var arvlObj = {};//Create a New Object Each Time an Arival Comes In
    arvlObj.email = custObj.email;
    arvlObj.name = custObj.name;
    arvlObj.dob = custObj.dob;
    arvlObj.phone = custObj.phone;
    arvlObj.imgUrl = custObj.imgUrl;
    arvlObj.joined = custObj.joined;
    arvlObj.iceName = custObj.iceName;
    arvlObj.icePhone = custObj.icePhone;
    arvlObj.visited = custObj.visited;
    arvlObj.membership = custObj.membership;
    arvlObj.day = Date.now();

    //push the obj to an Array
    custList.push(arvlObj);

    //CHECK local Storage for other Arivals
    var ListOfCust =JSON.parse(localStorage.getItem("TodaysArivals"));
                        
    if(ListOfCust!=null){
        //Check For New Arivals Only and add to Array
        ListOfCust.forEach(function (entry) {
            //Removes The Local Storage Every Day
            if(msgObj.day < entry.day){
                return;
            }
            if (entry.name != arvlObj.name)
                custList.push(entry);
        });                          
    }
    //Store The Array as a JSON.stringify in local storage / over write if all ready there
    localStorage.setItem("TodaysArivals", JSON.stringify(custList));
}