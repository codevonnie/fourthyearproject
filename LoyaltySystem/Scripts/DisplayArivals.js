
var custList;
var element = document.getElementById("ArivalsData");


function DisplayArivals() {
    document.getElementById("HeaderTitle").innerText = "Today's Arrivals";
    if (localStorage) {
        console.log("Local Storage Avail");
        custList = JSON.parse(localStorage.getItem("TodaysArivals"));

        if (custList != null)
            CreateArivals();
        else {
            var noArrivals = document.createElement("div");
            noArrivals.className += "text-center";
            noArrivals.innerHTML = "<h4>No One Checked In Today So Far</h4>";
            element.appendChild(noArrivals);
        }
    }
    else {
        alert("Your Browser Does Not Support This Feature")
    }
}


//Creates all the Html Per Arival from local Storage
function CreateArivals() {
    for (var key in custList) {
        if (custList.hasOwnProperty(key)) {
            var val = custList[key];

            console.log(val);

            var para = document.createElement("div");
            para.className += "col-xs-6 col-sm-4 placeholder DisplayPersonBox text-left";

            var img = document.createElement("img");
            img.src = val.imgUrl;
            img.className += "img-responsive RoundImg Img-size";


            var name = document.createElement("div");
            name.innerHTML = "<b>Name:</b> " + val.name;

            var email = document.createElement("div");
            email.innerHTML = "<b>Email:</b> " + val.email;

            var dob = document.createElement("div");
            dob.innerHTML = "<b>Dob:</b> " + convertMillToDate(parseFloat(val.dob));

            var contactNum = document.createElement("div");
            contactNum.innerHTML = "<b>Contact Num:</b> " + val.phone;

            var iceName = document.createElement("div");
            iceName.innerHTML = "<b>iceName:</b> " + val.iceName;

            var iceNum = document.createElement("div");
            iceNum.innerHTML = "<b>iceNum:</b> " + val.icePhone;

            para.appendChild(img);
            para.appendChild(name);
            para.appendChild(email);
            para.appendChild(dob);
            para.appendChild(contactNum);
            para.appendChild(iceName);
            para.appendChild(iceNum);

            element.appendChild(para);
        }
    }

    function convertMillToDate(mill) {
        var num = mill;
        var d = new Date(num);
        return d.toDateString();
    }
}
