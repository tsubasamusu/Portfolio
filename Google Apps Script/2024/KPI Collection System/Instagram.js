function setInstagramFollowers(instagramFollowers) {
    const INSTAGRAM_FOLLOWERS_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_INSTAGRAM_FOLLOWERS");

    sheet.getRange(todayRow, INSTAGRAM_FOLLOWERS_COLUMN).setValue(instagramFollowers);
}

function getInstagramFollowers() {
    const INSTAGRAM_ID = PropertiesService.getScriptProperties().getProperty("INSTAGRAM_ID");

    const INSTAGRAM_TOKEN = PropertiesService.getScriptProperties().getProperty("INSTAGRAM_TOKEN");

    const INSTAGRAM_USER_NAME = PropertiesService.getScriptProperties().getProperty("INSTAGRAM_USER_NAME");

    const API_URL = "https://graph.facebook.com/v18.0/" + INSTAGRAM_ID + "?fields=business_discovery.username(" + INSTAGRAM_USER_NAME + "){followers_count}&access_token=" + INSTAGRAM_TOKEN;

    const RESPONSE = JSON.parse(UrlFetchApp.fetch(encodeURI(API_URL)).getContentText());

    return RESPONSE.business_discovery.followers_count;
}

function setYesterdayInstagramImpressions(yesterdayInstagramImpressions) {
    if (yesterdayInstagramImpressions == null) return;

    const INSTAGRAM_IMPRESSIONS_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_INSTAGRAM_IMPRESSIONS");

    sheet.getRange(todayRow - 1, INSTAGRAM_IMPRESSIONS_COLUMN).setValue(yesterdayInstagramImpressions);
}

function getYesterdayInstagramImpressions() {
    const INSTAGRAM_ID = PropertiesService.getScriptProperties().getProperty("INSTAGRAM_ID");

    const INSTAGRAM_TOKEN = PropertiesService.getScriptProperties().getProperty("INSTAGRAM_TOKEN");

    let yesterday = new Date();

    yesterday.setDate((new Date()).getDate() - 1);

    const YESTERDAY_TIME_STAMP = Math.floor(yesterday.getTime() / 1000);

    const TODAY_TIME_STAMP = Math.floor((new Date()).getTime() / 1000);

    const METRIC = "impressions";

    const API_URL = "https://graph.facebook.com/v18.0/" + INSTAGRAM_ID + "/insights?metric=" + METRIC + "&period=day&since=" + YESTERDAY_TIME_STAMP + "&until=" + TODAY_TIME_STAMP + "&access_token=" + INSTAGRAM_TOKEN;

    let response;

    const ERROR_MESSAGE = "Instagram の昨日のインプレッション数の取得に失敗しました。";

    try {
        response = UrlFetchApp.fetch(encodeURI(API_URL)).getContentText();
    }
    catch (e) {
        sendMessageToSlack(ERROR_MESSAGE + "\n\n" + e.toString());

        return null;
    }

    if (response.includes("error")) {
        sendMessageToSlack(ERROR_MESSAGE + "\n\n" + response);

        return null;
    }

    const RECORDING_DATE_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_RECORDING_DATE");

    const YESTERDAY_TEXT = Utilities.formatDate(sheet.getRange(todayRow - 1, RECORDING_DATE_COLUMN).getValue(), "JST", "yyyy-MM-dd");

    const JSON_RESPONSE = JSON.parse(response);

    for (let i = 0; i < JSON_RESPONSE.data[0].values.length; i++) {
        if (!JSON_RESPONSE.data[0].values[i].end_time.includes(YESTERDAY_TEXT)) continue;

        return parseInt(JSON_RESPONSE.data[0].values[i].value);
    }

    sendMessageToSlack(ERROR_MESSAGE + "\n\n" + JSON_RESPONSE);

    return null;
}