var fileInputElement = document.querySelector("input[type=file]")
var checkBox = document.getElementById("file-checkbox")
var resumeBox = document.getElementById("item-resume")
fileInputElement.addEventListener("change", function () {
    console.log("thing");
    if (fileInputElement.files.length > 0) {
        checkBox.style.display = "inline";
        resumeBox.style.backgroundColor = "rgb(70, 70, 70)";

    }
    else {
        checkBox.style.display = "none";
        resumeBox.style.backgroundColor = "#2e2e2e";

    }
});

