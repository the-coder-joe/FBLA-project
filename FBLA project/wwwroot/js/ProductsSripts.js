
var closeElement = document.getElementsByClassName("close")[0];



function openMembershipDisplay(membershipType) {
    try {
        let memberField = document.getElementById("membership-type");
        memberField.value = membershipType;
    } catch {
        openModal();
    }
    openModal();    
}

function openModal() {
    let modal = document.getElementById("data-modal");
    modal.style.display = "block";
}

function closeModal() {
    let modal = document.getElementById("data-modal");

    modal.style.display = "none";
}

window.onclick = function (event) {
    var modal = document.getElementById("data-modal");

    if (event.target == modal) {
        modal.style.display = "none";
    }
}
