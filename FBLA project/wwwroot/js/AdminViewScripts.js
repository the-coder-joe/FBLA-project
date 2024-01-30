// Get the modal
var modal = document.getElementById("data-modal");

// Get the button that opens the modal
var btn = document.getElementById("myBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal
function openModal(appId) {
    var selectedapp;
    for (var i = 0; i < jsondata.length; i++) {
        if (jsondata[i].applicationId == appId) {
            selectedapp = jsondata[i];
        }
    }
    console.log(selectedapp);

    var name = document.getElementById("modal-name");
    var phoneNumber = document.getElementById("modal-phoneNumber");
    var position = document.getElementById("modal-position");
    var strengths= document.getElementById("modal-strengths");
    var why = document.getElementById("modal-why");
    var download = document.getElementById("resume-download");

    name.innerHTML = selectedapp.fields.name;
    phoneNumber.innerHTML = selectedapp.fields.phoneNumber;
    position.innerHTML = selectedapp.job.title; 
    strengths.innerHTML = selectedapp.fields.strengths;
    why.innerHTML = selectedapp.fields.whyThisJob;
    download.setAttribute("href", "/Resumes/" + selectedapp.resumeFileName);
    
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
