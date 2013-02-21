var stockTickerService;
var NASDAQ = {
    'FACEBOOK' : 'FB',
    'GOOGLE': 'GOOG'
}

$(document).ready(function () {
    stockTickerService = new WebSocket("ws://localhost:55555/StockTicker");
    
    stockTickerService.onopen = function () {
        $("#stockQuoteInfo").append("Connection opened<br/>");
    }

    stockTickerService.onmessage = function (message) {
        $("#stockQuoteInfo").append(message.data + "<br/>");
    }

    stockTickerService.onclose = function () {
        $("#stockQuoteInfo").append("Connection closed" + "<br/>");
    }

    stockTickerService.onerror = function (error) {
        $("#stockQuoteInfo").append(error.data + "<br/>");
    }

    $("#startButton").click(function () {
        if( $("#stockQuoteFB")[0].checked){
            stockTickerService.send(NASDAQ.FACEBOOK);
        } else {
            stockTickerService.send(NASDAQ.GOOGLE);
        }

        setUIState(true);
    });

    $("#stopButton").click(function () {
        stockTickerService.send("STOP");
        setUIState(false);
    });

});

function setUIState(isUpdating) {
    if (isUpdating) {
        $("#startButton").attr("disabled", "disabled");
        $("#stockQuoteFB").attr("disabled", "disabled");
        $("#stockQuoteGOOG").attr("disabled", "disabled");
        $("#stopButton").removeAttr("disabled");
        $("#stockQuoteInfo").text("");
    } else {
        $("#startButton").removeAttr("disabled");
        $("#stockQuoteFB").removeAttr("disabled");
        $("#stockQuoteGOOG").removeAttr("disabled");
        $("#stopButton").attr("disabled", "disabled");
    }
}
