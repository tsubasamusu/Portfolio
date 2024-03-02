function setIosUnits() {
    const IOS_UNITS_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_IOS_UNITS");

    const FIRST_ROW = PropertiesService.getScriptProperties().getProperty("ROW_FIRST");

    const RECORDING_DATE_COLUMN = PropertiesService.getScriptProperties().getProperty("COLUMN_RECORDING_DATE");

    for (let currentRow = FIRST_ROW; currentRow <= sheet.getLastRow(); currentRow++) {
        if (!sheet.getRange(currentRow, IOS_UNITS_COLUMN).isBlank()) continue;
        
        if (sheet.getRange(currentRow, RECORDING_DATE_COLUMN).isBlank()) return;

        const DATE = sheet.getRange(currentRow, RECORDING_DATE_COLUMN).getValue();

        const UNIT = getIosUnits(DATE);
        
        if (UNIT == null) return;

        const YESTERDAY_UNIT = parseInt(sheet.getRange(currentRow - 1, IOS_UNITS_COLUMN).getValue());

        sheet.getRange(currentRow, IOS_UNITS_COLUMN).setValue(YESTERDAY_UNIT + UNIT);
    }
}

function getIosUnits(date) {
    const JWT = UrlFetchApp.fetch(PropertiesService.getScriptProperties().getProperty("CLOUD_RUN_URL")).getContentText();

    const VENDOR_NUMBER = PropertiesService.getScriptProperties().getProperty("APPLE_VENDOR_NUMBER");

    const HEADERS =
    {
        "Content-Type": "application/x-www-form-urlencoded",
        "Accept": "application/a-gzip",
        "Authorization": "Bearer " + JWT
    }

    const REQUEST =
    {
        method: "get",
        muteHttpExceptions: true,
        headers: HEADERS
    }

    const API_URL = "https://api.appstoreconnect.apple.com/v1/salesReports?filter[frequency]=DAILY&filter[reportSubType]=SUMMARY&filter[reportType]=SALES&filter[vendorNumber]=" + VENDOR_NUMBER + "&filter[reportDate]=" + Utilities.formatDate(date, "JST", "yyyy-MM-dd");

    const RESPONSE = UrlFetchApp.fetch(API_URL, REQUEST);
    
    if (RESPONSE.getContentText().includes("There were no sales for the date specified.")) return 0;
    
    if (RESPONSE.getContentText().includes("Report is not available yet.")) return null;
    
    if (RESPONSE.getContentText().includes("errors")) {
        sendMessageToSlack("App Store Connect のレポートの取得に失敗しました。\n\n" + RESPONSE.getContentText());

        return null;
    }

    const REPORT_GZIP = RESPONSE.getBlob().setContentType("application/x-gzip");

    const REPORT_TSV = Utilities.newBlob("", "text/tsv", "Report.tsv").setDataFromString(Utilities.ungzip(REPORT_GZIP).getDataAsString(), "UTF8");

    const REPORT_ARRAY = encodeTsvToArray(REPORT_TSV.getDataAsString());

    return parseInt(REPORT_ARRAY[REPORT_ARRAY.length - 2][7]);
}

function encodeTsvToArray(tsvText) {
    let allTexts = [];

    for (const TEXT of tsvText.split("\n")) {
        allTexts.push(TEXT.split("\t"))
    }

    const TEXT_LENGTHS = allTexts.map(text => text.length);

    const MAX_TEXT_LENGTH = Math.max(...TEXT_LENGTHS);

    let newArray = [];

    for (const TEXT of allTexts) {
        if (TEXT.length === MAX_TEXT_LENGTH) {
            newArray.push(TEXT);

            continue;
        }

        const ADD_NUMBER = MAX_TEXT_LENGTH - TEXT.length;

        for (let i = 1; i <= ADD_NUMBER; i++) {
            TEXT.push("");
        }

        newArray.push(TEXT);
    }

    return newArray;
}