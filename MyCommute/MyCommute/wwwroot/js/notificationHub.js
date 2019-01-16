"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();


connection.on("ReceiveRatingNotification", function (ratingCount) {
    document.cookie = "ratingNotifications=" + ratingCount + "; path=/";
    $("#ratingNotification").text(ratingCount);
});

connection.on("ReceiveFriendNotification", function (ratingCount) {
    document.cookie = "friendNotifications=" + ratingCount + "; path=/";
    $("#friendNotification").text(ratingCount);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

$("#rateUser").on("click", function () {
    var receiverId = $("#userId").val();
    connection.invoke("SendRatingNotification", receiverId).catch(function (err) {
        return console.error(err.toString());
    });
});

$("#addFriend").on("click", function () {
    var receiverId = $("#userId").val();
    connection.invoke("SendFriendNotification", receiverId).catch(function (err) {
        return console.error(err.toString());
    });
});

$("#ratingNotificationButton").on("click", function () {
    document.cookie = "ratingNotifications=0; path=/";
    $("#ratingNotification").text("0");
    connection.invoke("ClearRatingNotification").catch(function (err) {
        return console.error(err.toString());
    });
});

$("#friendNotificationButton").on("click", function () {
    document.cookie = "friendNotifications=0; path=/";
    $("#friendNotification").text("0");
    connection.invoke("ClearFriendNotification").catch(function (err) {
        return console.error(err.toString());
    });
});