function setYesterdayAllAccess(yesterdayAllAccess) {
    const ALL_ACCESS_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_ALL_ACCESS");

    sheet.getRange(todayRow - 1, ALL_ACCESS_COLUMN).setValue(yesterdayAllAccess);
}

function getYesterdayAllAccessFromMessage(slackMessage) {
    if (slackMessage == null) return null;

    for (let i = slackMessage.length; i >= 0; i--) {
        if (slackMessage.slice(i, i + 1) != " ") continue;

        return slackMessage.slice(i + 1, slackMessage.length + 1);
    }

    sendMessageToSlack("最新のメッセージからの総アクセス数の取得に失敗しました。");

    return null;
}

function getLastBotMessage() {
    const API_URL = "https://slack.com/api/conversations.history?channel=" + PropertiesService.getScriptProperties().getProperty("SLACK_ID_CHANNEL_GET");

    const OPTIONS =
    {
        method: "post",
        headers:
        {
            "contentType": "application/x-www-form-urlencoded",
            "Authorization": "Bearer " + PropertiesService.getScriptProperties().getProperty("SLACK_TOKEN_BOT")
        }
    };

    const RESPONSE = JSON.parse(UrlFetchApp.fetch(API_URL, OPTIONS).getContentText());

    const BOT_USER_ID = PropertiesService.getScriptProperties().getProperty("SLACK_ID_BOT_USER");

    for (let i = 0; i < RESPONSE.messages.length; i++) {
        if (RESPONSE.messages[i].user != BOT_USER_ID) continue;

        return RESPONSE.messages[i].text;
    }

    sendMessageToSlack("最新のメッセージの取得に失敗しました。");

    return null;
}