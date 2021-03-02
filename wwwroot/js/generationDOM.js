let questionCounter = 1;
var answersNumbers = [1];
function generateQuizQuestion() {
    questionCounter++;
    answersNumbers.push(1);
    var card = `<div class="card" id="cardQ` + questionCounter +`">
                    <div class="card-header" id="headingQ`+ questionCounter +`">
                        <div class="btn btn-danger float-right" onclick="removeQuestion('cardQ` + questionCounter +`')"><i class="fas fa-trash"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter + `" aria-expanded="false" aria-controls="collapseQ` + questionCounter +`">
                                Question #`+ questionCounter +`  &nbsp;<i class="fas fa-caret-down"></i>
                            </button>
                        </h1>
                    </div>

                    <div id="collapseQ`+ questionCounter +`" class="collapse" aria-labelledby="headingQ`+ questionCounter +`" data-parent="#quizAccordion">
                        <div class="card-body">
                            <div>
                                <div class="float-left mb-3">
                                    <label>Time to answer</label>
                                    <input type="number" class="form-control mb-2" id="timeQ`+ questionCounter +`" placeholder="[s]">
                                    <label>Points</label>
                                    <input type="number" class="form-control" id="pointsQ`+ questionCounter +`">
                                </div>
                                <div class="float-left mb-3 ml-4 w-75" id="questionContentQ`+ questionCounter +`">
                                    <label>Question</label>
                                    <div class="summernote"></div>
                                </div>

                                <div class="accordion float-left w-100 mb-4" id="accordionQ`+ questionCounter +`">
                                    <div class="card" id="cardQ`+ questionCounter +`A1">
                                        <div class="card-header heading">
                                            <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ questionCounter +`A1')"><i class="fas fa-trash"></i></div>
                                            <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ questionCounter +`A1')"><i class="fas fa-check"></i></div>
                                            <h1 class="mb-0">
                                                <button class="btn btn-link" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter + `A1" aria-expanded="true" aria-controls="collapseQ` + questionCounter +`A1">
                                                    Answer #1  &nbsp;<i class="fas fa-caret-down"></i>
                                                </button>
                                            </h1>
                                        </div>
                                        <div id="collapseQ`+ questionCounter + `A1" class="collapse" aria-labelledby="headingA1" data-parent="#accordionQ` + questionCounter +`">
                                            <div class="card-body" id="answerContentQ`+ questionCounter +`A1">
                                                <label>Answer content</label>
                                                <div class="summernote"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button class="btn btn-primary" onclick="addAnswer('accordionQ` + questionCounter +`')"><i class="fas fa-plus"></i>&nbsp; Add Answer</button>
                            </div>
                        </div>
                    </div>
                </div>`;
    return card;
}
function generateQuizAnswer(questionCounter) {
    var answerNumber = ++answersNumbers[questionCounter - 1];
    var card = `<div class="card" id="cardQ` + questionCounter + `A` + answerNumber +`">
                    <div class="card-header heading">
                        <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ questionCounter + `A` + answerNumber +`')"><i class="fas fa-trash"></i></div>
                        <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ questionCounter + `A` + answerNumber +`')"><i class="fas fa-check"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter + `A` + answerNumber + `" aria-expanded="true" aria-controls="collapseQ` + questionCounter + `A` + answerNumber +`">
                                Answer #` + answerNumber +`  &nbsp;<i class="fas fa-caret-down"></i>
                            </button>
                        </h1>
                    </div>
                    <div id="collapseQ`+ questionCounter + `A` + answerNumber + `" class="collapse" aria-labelledby="headingA` + answerNumber +`" data-parent="#accordionQ` + questionCounter + `">
                        <div class="card-body" id="answerContentQ`+ questionCounter + `A` + answerNumber +`">
                            <label>Answer content</label>
                            <div class="summernote"></div>
                        </div>
                    </div>
                </div>`;
    return card;
}