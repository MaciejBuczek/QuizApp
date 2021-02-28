let cardNumber = 1;
var answersNumbers = [1];
function generateQuizQuestion() {
    cardNumber++;
    answersNumbers.push(1);
    var card = `<div class="card" id="cardQ` + cardNumber.toString() +`">
                    <div class="card-header" id="headingQ`+ cardNumber.toString() +`">
                        <div class="btn btn-danger float-right" onclick="removeQuestion('cardQ` + cardNumber.toString() +`')"><i class="fas fa-trash"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseQ`+ cardNumber.toString() + `" aria-expanded="false" aria-controls="collapseQ` + cardNumber.toString() +`">
                                Question #`+ cardNumber.toString() +`
                            </button>
                        </h1>
                    </div>

                    <div id="collapseQ`+ cardNumber.toString() +`" class="collapse" aria-labelledby="headingQ`+ cardNumber.toString() +`" data-parent="#quizAccordion">
                        <div class="card-body">
                            <div>
                                <div class="float-left mb-3">
                                    <label>Time to answer</label>
                                    <input type="number" class="form-control mb-2" id="timeQ`+ cardNumber.toString() +`" placeholder="[s]">
                                    <label>Points</label>
                                    <input type="number" class="form-control" id="pointsQ`+ cardNumber.toString() +`">
                                </div>
                                <div class="float-left mb-3 ml-4 w-75" id="questionContentQ`+ cardNumber.toString() +`">
                                    <label>Question</label>
                                    <div class="summernote"></div>
                                </div>

                                <div class="accordion float-left w-100 mb-4" id="accordionQ`+ cardNumber.toString() +`">
                                    <div class="card" id="cardQ`+ cardNumber.toString() +`A1">
                                        <div class="card-header heading">
                                            <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ cardNumber.toString() +`A1')"><i class="fas fa-trash"></i></div>
                                            <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ cardNumber.toString() +`A1')"><i class="fas fa-check"></i></div>
                                            <h1 class="mb-0">
                                                <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#collapseQ`+ cardNumber.toString() + `A1" aria-expanded="true" aria-controls="collapseQ` + cardNumber.toString() +`A1">
                                                    Answer #1
                                                </button>
                                            </h1>
                                        </div>
                                        <div id="collapseQ`+ cardNumber.toString() + `A1" class="collapse show" aria-labelledby="headingA1" data-parent="#accordionQ` + cardNumber.toString() +`">
                                            <div class="card-body" id="answerContentQ`+ cardNumber.toString() +`A1">
                                                <label>Answer content</label>
                                                <div class="summernote"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button class="btn btn-primary" onclick="addAnswer('accordionQ` + cardNumber.toString() +`')"><i class="fas fa-plus"></i>&nbsp; Add Answer</button>
                            </div>
                        </div>
                    </div>
                </div>`;
    return card;
}
function generateQuizAnswer(cardNumber) {
    var answerNumber = ++answersNumbers[cardNumber - 1];
    var card = `<div class="card" id="cardQ` + cardNumber.toString() + `A` + answerNumber.toString() +`">
                    <div class="card-header heading">
                        <div class="btn btn-danger float-right" onclick="removeAnswer('cardQ`+ cardNumber.toString() + `A` + answerNumber.toString() +`')"><i class="fas fa-trash"></i></div>
                        <div class="btn btn-success float-right mr-4" onclick="setCorrectAnswer('cardQ`+ cardNumber.toString() + `A` + answerNumber.toString() +`')"><i class="fas fa-check"></i></div>
                        <h1 class="mb-0">
                            <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseQ`+ cardNumber.toString() + `A` + answerNumber.toString() + `" aria-expanded="true" aria-controls="collapseQ` + cardNumber.toString() + `A` + answerNumber.toString() +`">
                                Answer #` + answerNumber.toString() +`
                            </button>
                        </h1>
                    </div>
                    <div id="collapseQ`+ cardNumber.toString() + `A` + answerNumber.toString() + `" class="collapse" aria-labelledby="headingA` + answerNumber.toString() +`" data-parent="#accordionQ` + cardNumber.toString() + `">
                        <div class="card-body" id="answerContentQ`+ cardNumber.toString() + `A` + answerNumber.toString() +`">
                            <label>Answer content</label>
                            <div class="summernote"></div>
                        </div>
                    </div>
                </div>`;
    return card;
}