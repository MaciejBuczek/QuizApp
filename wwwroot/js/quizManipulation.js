function changeCaret(target) {
    var iconElement = target.getElementsByTagName("i")[0];
    if (iconElement.classList.contains("fa-caret-down")) {
        iconElement.classList.remove("fa-caret-down");
        iconElement.classList.add("fa-caret-up");
    } else {
        iconElement.classList.add("fa-caret-down");
        iconElement.classList.remove("fa-caret-up");
    }
}

function changeCheckBoxes(target) {
    var element = document.getElementById(target);
    if (element.checked)
        element.checked = false;
}

function addQuestion() {
    removeSummernoteClasses();
    document.getElementById("quizAccordion").innerHTML += generateQuizQuestion();
    startSummernote();
}

function addAnswer(target) {
    var number = parseInt(target.charAt(target.length - 1));
    removeSummernoteClasses();
    document.getElementById(target).innerHTML += generateQuizAnswer(number);
    startSummernote();
}

function removeQuestion(target) {
    var number = parseInt(target.charAt(target.length - 1));
    document.getElementById(target).remove();
    if (number != questionCounter && questionCounter != 1) {
        var i;
        for (i = 0; i < questionCounter - number; i++) {
            var nextElement = document.getElementById("cardQ" + (number + 1 + i));
            var oldValue = "Q" + (number + 1 + i);
            var newValue = "Q" + (number + i);
            while (nextElement.innerHTML.indexOf(oldValue) != -1) {
                nextElement.innerHTML = nextElement.innerHTML.replace(oldValue, newValue)
            }
            nextElement.id = "cardQ" + (number + i);
            nextElement.innerHTML = nextElement.innerHTML.replace(("Question #" + (number + 1 + i)), ("Question #" + (number + i)))
        }
    }
    questionCounter--;
    answersNumbers.splice((number - 1), 1);
}

function removeAnswer(target) {
    var answerNumber = parseInt(target.charAt(target.length - 1));
    var questionNumber = parseInt(target.charAt(target.length - 3));
    var lastAnswerNumber = answersNumbers[questionNumber - 1];
    document.getElementById(target).remove();
    var i;
    for (i = 0; i < lastAnswerNumber - answerNumber; i++) {
        var nextElement = document.getElementById("cardQ" + questionNumber.toString() + "A" + (answerNumber + 1 + i));
        var oldValue = "A" + (answerNumber + 1 + i);
        var newValue = "A" + (answerNumber + i);
        while (nextElement.innerHTML.indexOf(oldValue) != -1) {
            nextElement.innerHTML = nextElement.innerHTML.replace(oldValue, newValue)
        }
        nextElement.id = "cardQ" + questionNumber.toString() + "A" + (answerNumber + i);
        nextElement.innerHTML = nextElement.innerHTML.replace(("Answer #" + (answerNumber + 1 + i)), ("Answer #" + (answerNumber + i)))
    }
    answersNumbers[questionNumber - 1]--;
}

function setCorrectAnswer(target) {
    var targetElement = document.getElementById(target);
    var targetHeadingElement = targetElement.getElementsByClassName("heading")[0];
    var targetSetButton = targetHeadingElement.getElementsByTagName("div")[1];
    if (!targetElement.classList.contains("selected")) {
        targetElement.classList.add("selected");
        targetHeadingElement.classList.add("bg-success");
        targetSetButton.classList.remove("btn-success");
        targetSetButton.classList.add("btn-warning");
        targetSetButton.innerHTML = "<i class='fas fa-times'></i>";
        targetElement.getElementsByTagName("button")[0].classList.add("text-white");
        targetElement.get
    } else {
        targetElement.classList.remove("selected");
        targetHeadingElement.classList.remove("bg-success");
        targetSetButton.classList.add("btn-success");
        targetSetButton.classList.remove("btn-warning");
        targetSetButton.innerHTML = "<i class='fas fa-check'></i>";
        targetElement.getElementsByTagName("button")[0].classList.remove("text-white");
    }
}

function getQuiz() {
    var quiz = new Object;
    quiz.Title = document.getElementById("quizName").value;
    quiz.Description = document.getElementById("description").getElementsByClassName("note-editable")[0].innerHTML;
    quiz.NegativePoints = document.getElementById("negativePoints").checked;
    quiz.PartialPoints = document.getElementById("partialPoints").checked;
    quiz.Questions = [];
    var i;
    for (i = 1; i <= questionCounter; i++) {
        var question = new Object;
        question.Content = document.getElementById("questionContentQ" + i).getElementsByClassName("note-editable")[0].innerHTML;
        question.Time = document.getElementById("timeQ" + i).value;
        question.Points = document.getElementById("pointsQ" + i).value;
        question.Answers = [];
        var j;
        var correctAnswer = 0;
        for (j = 1; j <= answersNumbers[i - 1]; j++) {
            if (document.getElementById("cardQ" + i + "A" + j).classList.contains("selected"))
                correctAnswer += Math.pow(2, (j - 1));
            var answer = new Object;
            answer.Content = document.getElementById("answerContentQ" + i + "A" + j).getElementsByClassName("note-editable")[0].innerHTML;
            question.Answers.push(answer);
        }
        question.CorrectAnswer = correctAnswer;
        quiz.Questions.push(question);
    }
    return quiz;
}