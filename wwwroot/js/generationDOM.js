let questionCounter = 1;
var answersNumbers = [1];
function generateQuizQuestion() {
    questionCounter++;
    answersNumbers.push(1);
    var card = `<div class="card" id="cardQ` + questionCounter.toString() +`">
                    <div class="card-header" id="headingQ`+ questionCounter.toString() +`">
                        <div class="btn btn-danger float-right" onclick="removeQuestion('cardQ` + questionCounter.toString() +`')"><i class="fas fa-trash"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter.toString() + `" aria-expanded="false" aria-controls="collapseQ` + questionCounter.toString() +`">
                                Question #`+ questionCounter.toString() +`  &nbsp;<i class="fas fa-caret-down"></i>
                            </button>
                        </h1>
                    </div>

                    <div id="collapseQ`+ questionCounter.toString() +`" class="collapse" aria-labelledby="headingQ`+ questionCounter.toString() +`" data-parent="#quizAccordion">
                        <div class="card-body">
                            <div>
                                <div class="float-left mb-3">
                                    <label>Time to answer</label>
                                    <input type="number" class="form-control mb-2" id="timeQ`+ questionCounter.toString() +`" placeholder="[s]">
                                    <label>Points</label>
                                    <input type="number" class="form-control" id="pointsQ`+ questionCounter.toString() +`">
                                </div>
                                <div class="float-left mb-3 ml-4 w-75" id="questionContentQ`+ questionCounter.toString() +`">
                                    <label>Question</label>
                                    <div class="summernote"></div>
                                </div>

                                <div class="accordion float-left w-100 mb-4" id="accordionQ`+ questionCounter.toString() +`">
                                    <div class="card" id="cardQ`+ questionCounter.toString() +`A1">
                                        <div class="card-header heading">
                                            <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ questionCounter.toString() +`A1')"><i class="fas fa-trash"></i></div>
                                            <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ questionCounter.toString() +`A1')"><i class="fas fa-check"></i></div>
                                            <h1 class="mb-0">
                                                <button class="btn btn-link" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter.toString() + `A1" aria-expanded="true" aria-controls="collapseQ` + questionCounter.toString() +`A1">
                                                    Answer #1  &nbsp;<i class="fas fa-caret-down"></i>
                                                </button>
                                            </h1>
                                        </div>
                                        <div id="collapseQ`+ questionCounter.toString() + `A1" class="collapse" aria-labelledby="headingA1" data-parent="#accordionQ` + questionCounter.toString() +`">
                                            <div class="card-body" id="answerContentQ`+ questionCounter.toString() +`A1">
                                                <label>Answer content</label>
                                                <div class="summernote"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button class="btn btn-primary" onclick="addAnswer('accordionQ` + questionCounter.toString() +`')"><i class="fas fa-plus"></i>&nbsp; Add Answer</button>
                            </div>
                        </div>
                    </div>
                </div>`;
    return card;
}
function generateQuizAnswer(questionCounter) {
    var answerNumber = ++answersNumbers[questionCounter - 1];
    var card = `<div class="card" id="cardQ` + questionCounter.toString() + `A` + answerNumber.toString() +`">
                    <div class="card-header heading">
                        <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ questionCounter.toString() + `A` + answerNumber.toString() +`')"><i class="fas fa-trash"></i></div>
                        <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ questionCounter.toString() + `A` + answerNumber.toString() +`')"><i class="fas fa-check"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" onclick="changeCaret(this)" type="button" data-toggle="collapse" data-target="#collapseQ`+ questionCounter.toString() + `A` + answerNumber.toString() + `" aria-expanded="true" aria-controls="collapseQ` + questionCounter.toString() + `A` + answerNumber.toString() +`">
                                Answer #` + answerNumber.toString() +`  &nbsp;<i class="fas fa-caret-down"></i>
                            </button>
                        </h1>
                    </div>
                    <div id="collapseQ`+ questionCounter.toString() + `A` + answerNumber.toString() + `" class="collapse" aria-labelledby="headingA` + answerNumber.toString() +`" data-parent="#accordionQ` + questionCounter.toString() + `">
                        <div class="card-body" id="answerContentQ`+ questionCounter.toString() + `A` + answerNumber.toString() +`">
                            <label>Answer content</label>
                            <div class="summernote"></div>
                        </div>
                    </div>
                </div>`;
    return card;
}