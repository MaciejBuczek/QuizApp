const fullDashArray = 283;
const timeDelay = 5;

function getFormattedTime(time) {
    let minutes = Math.floor(time / 60);
    let seconds = time % 60;

    if (seconds < 10)
        seconds = `0${seconds}`;

    return `${minutes}:${seconds}`;
}

function startTimer(time, onTimeEndFunc) {
    let totalTime = time;
    let timePassed = 0;
    let timeLeft = totalTime;

    document.getElementById("timer-label").innerText = getFormattedTime(timeLeft);

    timerInterval = setInterval(() => {
        timePassed++;
        timeLeft = totalTime - timePassed;
        document.getElementById("timer-label").innerText = getFormattedTime(timeLeft);
        setDasharray(timeLeft, totalTime);
        if (timeLeft == 0) {
            clearInterval(timerInterval);
            onTimeEndFunc();
        }
    }, 1000);
}

function getTimePercent(timeLeft, totalTime) {
    let percentRaw = timeLeft / totalTime;
    return percentRaw - (1 / totalTime) * (1 - percentRaw);
}

function setDasharray(timeLeft, totalTime) {
    let value = `${(getTimePercent(timeLeft, totalTime) * fullDashArray).toFixed(0)} ${fullDashArray}`;
    document.getElementById("timer-path-remaining").setAttribute("stroke-dasharray", value);
}