let channelId = null;

function doPost(e) {
    const PAYLOAD = JSON.parse(e.parameter.payload);

    channelId = PAYLOAD.channel.id;

    deleteSlackMessage(PAYLOAD);

    const VALUE = PAYLOAD.actions[0].value;

    const PROMPT = getPromptFromDocument();

    switch (VALUE) {
        case "text":
            const MESSAGE = getMentionText() + "\n" + getChatGptAnswer(PROMPT);
            sendMessageToSlack(MESSAGE);
            break;

        case "image":
            const IMAGE_FILE_URL = getDalleAnswer(PROMPT);
            sendFileToSlack(IMAGE_FILE_URL);
            break;
    }
}

function deleteSlackMessage(payload) {
    const TOKEN = PropertiesService.getScriptProperties().getProperty("TOKEN");

    const API_URL = "https://slack.com/api/chat.delete";

    const PAYLOAD =
    {
        token: TOKEN,
        channel: channelId,
        ts: payload.message.ts
    };

    const OPTIONS =
    {
        method: "post",
        payload: PAYLOAD
    };

    UrlFetchApp.fetch(API_URL, OPTIONS);
}

function getPromptFromDocument() {
    const DOCUMENT_ID = PropertiesService.getScriptProperties().getProperty("PROMPT_DOCUMENT_ID");

    const DOCUMENT = DocumentApp.openById(DOCUMENT_ID);

    const BODY = DOCUMENT.getBody();

    return BODY.getText();
}

function getMentionText() {
    const DOCUMENT_ID = PropertiesService.getScriptProperties().getProperty("USER_DOCUMENT_ID");

    const DOCUMENT = DocumentApp.openById(DOCUMENT_ID);

    const BODY = DOCUMENT.getBody();

    const MENTIONED_USER_ID = BODY.getText().trim();

    return "<@" + MENTIONED_USER_ID + ">";
}

function getChatGptAnswer(prompt) {
    const API_KEY = PropertiesService.getScriptProperties().getProperty("OPEN_AI_API_KEY");

    const API_URL = 'https://api.openai.com/v1/chat/completions';

    const MESSAGE =
    {
        role: "user",
        content: prompt
    };

    const MESSAGES = [MESSAGE];

    const PAYLOAD =
    {
        model: "gpt-3.5-turbo",
        temperature: 0.5,
        max_tokens: 1000,
        messages: MESSAGES
    }

    const HEADERS =
    {
        "Content-Type": "application/json",
        "Authorization": "Bearer " + API_KEY
    };

    const REQUEST =
    {
        method: "POST",
        muteHttpExceptions: true,
        headers: HEADERS,
        payload: JSON.stringify(PAYLOAD)
    }

    try {
        const RESPONSE = JSON.parse(UrlFetchApp.fetch(API_URL, REQUEST).getContentText());

        return RESPONSE.choices[0].message.content;
    }
    catch (e) {
        return e.toString();
    }
}

function getDalleAnswer(prompt) {
    const API_KEY = PropertiesService.getScriptProperties().getProperty("OPEN_AI_API_KEY");

    const API_URL = 'https://api.openai.com/v1/images/generations';

    const PAYLOAD =
    {
        n: 1,
        size: "256x256",
        prompt: prompt
    };

    const HEADERS =
    {
        "Authorization": "Bearer " + API_KEY,
        "Content-type": "application/json",
        "X-Slack-No-Retry": 1
    };

    const OPTIONS =
    {
        muteHttpExceptions: true,
        headers: HEADERS,
        method: "POST",
        payload: JSON.stringify(PAYLOAD)
    };

    try {
        const RESPONSE = JSON.parse(UrlFetchApp.fetch(API_URL, OPTIONS).getContentText());

        return RESPONSE.data[0].url;
    }
    catch (e) {
        const ERROR_MESSAGE = e.toString();

        sendMessageToSlack(ERROR_MESSAGE);

        return null;
    }
}

function sendMessageToSlack(text) {
    const TOKEN = PropertiesService.getScriptProperties().getProperty("TOKEN");

    const SLACK_APP = SlackApp.create(TOKEN);

    SLACK_APP.postMessage(channelId, text);
}

function sendFileToSlack(fileUrl) {
    const TOKEN = PropertiesService.getScriptProperties().getProperty("TOKEN");

    const API_URL = "https://slack.com/api/files.upload";

    let file = null;

    try {
        file = UrlFetchApp.fetch(fileUrl).getBlob();
    }
    catch (e) {
        const MESSAGE = getMentionText() + "\n画像ファイルの取得に失敗しました。";

        sendMessageToSlack(MESSAGE);

        return;
    }

    const PAYLOAD =
    {
        file: file,
        channels: channelId,
        initial_comment: getMentionText(),
        title: getPromptFromDocument()
    };

    const HEADERS =
    {
        "Authorization": "Bearer " + TOKEN
    };

    const OPTIONS =
    {
        method: "post",
        payload: PAYLOAD,
        headers: HEADERS
    };

    UrlFetchApp.fetch(API_URL, OPTIONS);
}