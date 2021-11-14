function generateAnswer(content, id) {
    return `<div class="form-check h2">
                              <input class="form-check-input mb-1 answer" type="checkbox" value=${id} id="checkbox-${id}" style="width:1em; heighT:1em;">
                              <label class="form-check-label ml-4" for="checkbox-${id}">
                                ${content}
                              </label>
                          </div>`;
}

function generateUserPointsLabel(username, points) {
    return `<div class="row">
                        <div class="col-6" style="font-size:1.5em">
                            ${username}
                        </div>
                        <div class="col-6" style="text-align:right; font-size:1.5em">
                            ${points}
                        </div>
                    </div>`;
}