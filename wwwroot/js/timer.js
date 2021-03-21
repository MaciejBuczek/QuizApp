const fullDashArray = 283;

function startTimer(totalSeconds) {
    const totalTime = totalSeconds;
    let timerInterval = 1000;
    document.getElementById("timer-label").innerText = updateTimerLabel(totalSeconds);
    let interval = setInterval(() => {
        if (--totalSeconds < 0) {
            clearInterval(interval);
        }
        else {
            document.getElementById("timer-label").innerText = updateTimerLabel(totalSeconds);
            setDasharray(totalTime, totalSeconds);
        }
    }, timerInterval);
}

function updateTimerLabel(totalSeconds) {
    let minutes = Math.floor(totalSeconds / 60);
    let seconds = totalSeconds % 60;

    if (seconds < 10) {
        seconds = `0${seconds}`;
    }
    return `${minutes}:${seconds}`;
}

function getPathFraction(totalTime, timeLeft) {
    const pathFraction = timeLeft / totalTime;
    return pathFraction - (1 / totalTime) * (1 - pathFraction);
}

function setDasharray(totalTime, timeLeft) {
    const value = `${(getPathFraction(totalTime, timeLeft) * fullDashArray).toFixed(0)} ${fullDashArray}`;
    document.getElementById("timer-path-foreground").setAttribute("stroke-dasharray", value);
}