$("DOMContentLoaded").ready(function () {  
    const nodeListField = document.querySelectorAll(".field-validation-error");
    const nodeListFieldTwo = document.querySelector(".validation-summary-errors");

    if (nodeListFieldTwo != null) {

        nodeListFieldTwo.style.opacity = "1";
    }
    for (var i in nodeListField) {
        nodeListField[i].style.opacity = "1";
    }
});
