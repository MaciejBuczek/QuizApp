function validateeQuiz() {
    var errorTitle = "Quiz creation error"
    if (document.getElementById("quizName").value == "") {
        fireErrorAlert(errorTitle, "Please enter a quiz name");
        return false;
    }
    if (questionCounter == 0) {
        fireErrorAlert(errorTitle, "Quiz has to have at least one question");
        return false;
    }
    
    for (i = 0; i < answersNumbers.length; i++) {
        if (answersNumbers[i] == 0) {
            fireErrorAlert(errorTitle, "Each question has to have at least one answer");
            return false;
        }
    }

    var i;
    for (i = 1; i <= questionCounter; i++) {
        var content = document.getElementById("questionContentQ" + i).getElementsByTagName("p")[1].innerHTML;
        if (content == "<br>" || content == "" || document.getElementById("timeQ" + i).value == "" ||
            document.getElementById("pointsQ" + i).value == "") {
                fireErrorAlert(errorTitle, "Some questions are missing content, time or points");
                return false;
        }
        var isCorrectAnswerSet = false;
        var j;
        for (j = 1; j <= answersNumbers[i - 1]; j++) {
            content = document.getElementById("answerContentQ" + i + "A" + j).getElementsByTagName("p")[1].innerHTML
            if (content == "<br>" || content == "") {
                fireErrorAlert(errorTitle, "Some answers are missing content");
                return false;
            }
            if (document.getElementById("cardQ" + i + "A" + j).classList.contains("selected"))
                isCorrectAnswerSet = true;
        }
        if (!isCorrectAnswerSet) {
            fireErrorAlert(errorTitle, "Each question has to have at least one correct answer selected");
            return false;
        }
    }
    return true;
}

function fireErrorAlert(errorTitle, errorContent) {
    Swal.fire(
        errorTitle,
        errorContent,
        'error'
    )
}