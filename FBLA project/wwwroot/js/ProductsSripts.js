$(document).ready(function () {
    var modal = document.getElementById("data-modal");
    var btn = document.getElementById("myBtn");
    var closeElement = document.getElementsByClassName("close")[0];

    $("#myBtn").on("click", openModal);
    function openModal() {
        modal.style.display = "block";
    }

    closeElement.onclick = function () {
        modal.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }

});