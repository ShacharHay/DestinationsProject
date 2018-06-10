//weather.js

var weatherCallback = function (data) {
    var wind = data.query.results.channel.wind;
    var item = data.query.results.channel.item;
    var text = "Temperature: " + item.condition.temp + " °C";
    $("#temperatureDiv p").html(text);

    if (item.condition.temp <= 20)
    {
        updateTemperatureDiv("It seems cold you may want to visit Jerusalem");
    }
    else if (item.condition.temp > 20 && item.condition.temp < 28) {
        updateTemperatureDiv("The weather is great! come visit Tel-Aviv");
    } else {
        updateTemperatureDiv("It seems to be very hot out there! go visit Eilat");
    }
};

function updateTemperatureDiv(text) {
    $("#temperatureDiv").append("<p>" + text + "</p>");
}