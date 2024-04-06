function setAndroidInstalls(androidInstalls) {
    if (androidInstalls == null) return;

    const ANDROID_INSTALLS_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_ANDROID_INSTALLS");

    for (let i = 1; i < androidInstalls.length; i++) {
        const TARGET_ROW = getSameDateRow(androidInstalls[i][0]);

        if (TARGET_ROW == null) continue;
        
        const ANDROID_INSTALLS_UNTIL_YESTERDAY = sheet.getRange(TARGET_ROW - 1, ANDROID_INSTALLS_COLUMN).getValue();
        
        const TARGET_CELL = sheet.getRange(TARGET_ROW, ANDROID_INSTALLS_COLUMN);
        
        if (!TARGET_CELL.isBlank()) continue;

        TARGET_CELL.setValue(ANDROID_INSTALLS_UNTIL_YESTERDAY + parseInt(androidInstalls[i][6]));
    }
}

function getAndroidInstalls() {
    let report = getObjectFromCloudStorage(null);

    if (report == null) return null;

    const REPORT_URL = JSON.parse(report.getContentText()).mediaLink;

    report = getObjectFromCloudStorage(REPORT_URL);

    if (report == null) return null;

    const REPORT_TEXT = report.getBlob().getDataAsString("UTF-16");

    return Utilities.parseCsv(REPORT_TEXT);
}

function getSameDateRow(dateText) {
    const FIRST_ROW = PropertiesService.getScriptProperties().getProperty("ROW_FIRST");

    const RECORDING_DATE_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_RECORDING_DATE");

    for (let currentRow = todayRow - 1; currentRow >= FIRST_ROW; currentRow--) {
        const CELL_DATE = sheet.getRange(currentRow, RECORDING_DATE_COLUMN).getValue();
        
        const CELL_DATE_TEXT = Utilities.formatDate(CELL_DATE, "JST", "yyyy-MM-dd");

        if (dateText == CELL_DATE_TEXT) return currentRow;
    }

    sendMessageToSlack("Play Console のレポートに一致する日付の行を見つけられませんでした。（" + dateText + "）");

    return null;
}

function getObjectFromCloudStorage(apiUrl) {
    const API_URL_IS_NULL = apiUrl == null;

    const HEADERS =
    {
        "Content-Type": "application/x-www-form-urlencoded",
        "Authorization": "Bearer " + getGoogleCloudJwt()
    };

    const REQUEST =
    {
        method: "get",
        muteHttpExceptions: true,
        headers: HEADERS
    }

    const PACKAGE_NAME = PropertiesService.getScriptProperties().getProperty("PACKAGE_NAME");

    const REPORT_BUCKET_NAME = PropertiesService.getScriptProperties().getProperty("REPORT_BUCKET_NAME");
    
    if (API_URL_IS_NULL) apiUrl = "https://www.googleapis.com/storage/v1/b/" + REPORT_BUCKET_NAME + "/o/" + encodeURIComponent("stats/installs/") + "installs_" + PACKAGE_NAME + "_" + (new Date()).getFullYear().toString() + getCurrentMonthText() + "_overview.csv";

    const RESPONSE = UrlFetchApp.fetch(apiUrl, REQUEST);
    
    if (!RESPONSE.getContentText().includes("error")) return RESPONSE;
    
    if (API_URL_IS_NULL) {
        sendMessageToSlack("今月分の Play Console のレポートの取得に失敗しました。\n\n" + RESPONSE.getContentText());

        const LAST_MONTH_YEAR_TEXT = (getLastMonthText() == "12" ? (new Date()).getFullYear() - 1 : (new Date()).getFullYear()).toString();

        apiUrl = "https://www.googleapis.com/storage/v1/b/" + REPORT_BUCKET_NAME + "/o/" + encodeURIComponent("stats/installs/") + "installs_" + PACKAGE_NAME + "_" + LAST_MONTH_YEAR_TEXT + getLastMonthText() + "_overview.csv";

        const SECOND_RESPONSE = UrlFetchApp.fetch(apiUrl, REQUEST);
        
        if (!SECOND_RESPONSE.getContentText().includes("error")) {
            sendMessageToSlack("先月分の Play Console のレポートの取得には成功しました。");

            return SECOND_RESPONSE;
        }

        sendMessageToSlack("先月分のレポートの取得も失敗しました。\n\n" + SECOND_RESPONSE.getContentText());

        return null;
    }

    sendMessageToSlack("Cloud Storage からの\n" + apiUrl.toString() + "\nの取得に失敗しました。\n\n" + RESPONSE.getContentText());

    return null;
}

function getLastMonthText() {
    const CURRENT_MONTH = ((new Date()).getMonth() + 1);
    
    let lastMonthText = (CURRENT_MONTH == 1 ? 12 : CURRENT_MONTH - 1).toString();
    
    if (lastMonthText.length == 1) lastMonthText = "0" + lastMonthText;

    return lastMonthText;
}

function getCurrentMonthText() {
    let currentMonthText = ((new Date()).getMonth() + 1).toString();
    
    if (currentMonthText.length == 1) currentMonthText = "0" + currentMonthText;

    return currentMonthText;
}

function getGoogleCloudJwt() {
    const PRIVATE_KEY = "";

    const SCOPES =
        [
            "https://www.googleapis.com/auth/devstorage.read_only"
        ];

    const JSON_WEB_TOKEN =
    {
        alg: "RS256",
        typ: "JWT"
    };

    const NOW = Math.floor(Date.now() / 1000);

    const CLAIM_SET =
    {
        iss: PropertiesService.getScriptProperties().getProperty("SERVICE_ACCOUNT_MAIL_ADDRESS"),
        scope: SCOPES.join(" "),
        aud: PropertiesService.getScriptProperties().getProperty("SERVICE_ACCOUNT_URL_TOKEN"),
        exp: NOW + 3600,
        iat: NOW
    };

    const ENCODED_JWT = Utilities.base64EncodeWebSafe(JSON.stringify(JSON_WEB_TOKEN));

    const ENCODED_CS = Utilities.base64EncodeWebSafe(JSON.stringify(CLAIM_SET));

    const SIGNATURE = Utilities.computeRsaSha256Signature(ENCODED_JWT + "." + ENCODED_CS, PRIVATE_KEY);

    const OPTIONS =
    {
        method: "post",
        contentType: "application/x-www-form-urlencoded",
        payload:
        {
            grant_type: "urn:ietf:params:oauth:grant-type:jwt-bearer",
            assertion: ENCODED_JWT + "." + ENCODED_CS + "." + Utilities.base64EncodeWebSafe(SIGNATURE)
        }
    };

    const RESPONSE = JSON.parse(UrlFetchApp.fetch("https://oauth2.googleapis.com/token", OPTIONS).getContentText());

    return RESPONSE.access_token;
}