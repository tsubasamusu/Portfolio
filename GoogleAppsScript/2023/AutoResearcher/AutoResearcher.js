function GPT(prompt, myColumn, myRow, targetCellValue = null) {
    if (targetCellValue != null) {
        targetCellValue = targetCellValue.toString();

        if (targetCellValue != null && targetCellValue.includes("ÅiäÆóπÅj")) Utilities.sleep(5000);
    }

    if (myColumn == 1 && myRow != 5) {
        const SHEET = SpreadsheetApp.getActiveSheet();

        const OTHER_NAME_CELL_VALUES = SHEET.getRange(5, 1, myRow - 5).getValues();

        prompt = OTHER_NAME_CELL_VALUES + "à»äOÇÃ" + prompt;
    }

    const PAYLOAD =
    {
        model: "text-davinci-003",
        prompt: prompt,
        temperature: 0.3,
        max_tokens: 1000
    };

    const HEADERS =
    {
        Authorization: "Bearer " + ""
    };

    const OPTIONS =
    {
        contentType: "application/json",
        headers: HEADERS,
        payload: JSON.stringify(PAYLOAD),
    };

    const RESPONSE = JSON.parse(UrlFetchApp.fetch("https://api.openai.com/v1/completions", OPTIONS).getContentText());

    return RESPONSE.choices[0].text.trim();
}