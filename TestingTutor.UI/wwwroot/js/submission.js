"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/submissionHub").build();

connection.start().then(function () {
    console.log("Connected");

});

connection.on("FeedbackFinish", function (number, index) {
    window.location = "/Submissions/AssignmentFeedback?id=" + number.toString() + "&index=" + index.toString();
});