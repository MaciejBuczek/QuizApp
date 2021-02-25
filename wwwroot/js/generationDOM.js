let cardNumber = 1;
function generateAccordionCard() {
    cardNumber++;
    var card = `
    <div class="card">
        <div class="card-header" id="heading`+ cardNumber.toString() +`">
            <h2 class="mb-0">
                <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapse`+ cardNumber.toString() + `" aria-expanded="false" aria-controls="collapse ` + cardNumber.toString()+`">
                    Question #`+ cardNumber.toString()+`
                        </button>
            </h2>
        </div>
        <div id="collapse`+ cardNumber.toString() + `" class="collapse" aria-labelledby="heading` + cardNumber.toString()+`" data-parent="#quizAccordion">
            <div class="card-body">
                test
                    </div>
        </div>
    </div>`
    return card;
}