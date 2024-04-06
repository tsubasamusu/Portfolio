let sheet;

let todayRow;

function onDesignatedTimeArrived() {
    const RECORDING_DATE_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_RECORDING_DATE");
    
    sheet = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();

    todayRow = sheet.getRange(sheet.getMaxRows(), RECORDING_DATE_COLUMN).getNextDataCell(SpreadsheetApp.Direction.UP).getRow() + 1;
    
    sheet.getRange(todayRow, RECORDING_DATE_COLUMN).setValue(new Date());
        
    setYesterdayAllAccess(getYesterdayAllAccessFromMessage(getLastBotMessage()));
        
    setPageVies(getPageViews());
        
    setInstagramFollowers(getInstagramFollowers());
        
    setYesterdayInstagramImpressions(getYesterdayInstagramImpressions());
        
    setAndroidInstalls(getAndroidInstalls());
        
    setIosUnits();

    setNotionValues();
}

function sendMessageToSlack(message) {
    if (message == null) return;

    message += "\n\n " + getYesterdayAllAccessFromMessage(getLastBotMessage());

    const PAYLOAD =
    {
        channel: PropertiesService.getScriptProperties().getProperty("SLACK_ID_CHANNEL_SEND"),
        text: message
    };

    const OPTIONS =
    {
        method: "post",
        contentType: "application/json",
        payload: JSON.stringify(PAYLOAD),
        headers:
        {
            "Authorization": "Bearer " + PropertiesService.getScriptProperties().getProperty("SLACK_TOKEN_BOT")
        }
    };

    UrlFetchApp.fetch("https://slack.com/api/chat.postMessage", OPTIONS);
}