var   chat = $.connection.chatHub;
$.connection.hub.url = "http://101.37.18.64:8066/chat";  
  $.connection.hub.start().done(function() { chat.server.userConnected(1); }

);
  
/*顾客分诊信息*/
var customerTriage = function (userID,cuInfo) {
    chat.server.sendMessage(userID, cuInfo + "上门了请准备接待！");
};
/*顾客消费*/
var customerConsume = function (cuId,money) {
    chat.server.consume(cuId, money);
}

