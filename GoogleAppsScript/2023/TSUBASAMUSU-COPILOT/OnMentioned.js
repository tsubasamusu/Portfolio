let promptBody = null;

let userIdBody = null;

let mentionedUserId = null;

let channelId = null;

function doPost(e) {
    const BOT_ID = PropertiesService.getScriptProperties().getProperty("BOT_ID");

    const MENTION_TEXT = "<@" + BOT_ID + ">";

    const PARAMS = JSON.parse(e.postData.getDataAsString());

    channelId = PARAMS.event.channel;

    mentionedUserId = PARAMS.event.user;

    setUpUserIdDocument();

    userIdBody.appendParagraph(mentionedUserId);

    const MENTIONED_MESSAGE = PARAMS.event.text;

    const PROMPT = MENTIONED_MESSAGE.replace(MENTION_TEXT, "");

    setUpPromptDocument();

    promptBody.appendParagraph(PROMPT);

    sendMessageWithButtonsToSlack();

    return ContentService.createTextOutput(PARAMS.challenge);
}

function setUpUserIdDocument() {
    const DOCUMENT_ID = PropertiesService.getScriptProperties().getProperty("USER_DOCUMENT_ID");

    const DOCUMENT = DocumentApp.openById(DOCUMENT_ID);

    userIdBody = DOCUMENT.getBody();

    userIdBody.clear();
}

function setUpPromptDocument() {
    const DOCUMENT_ID = PropertiesService.getScriptProperties().getProperty("PROMPT_DOCUMENT_ID");

    const DOCUMENT = DocumentApp.openById(DOCUMENT_ID);

    promptBody = DOCUMENT.getBody();

    promptBody.clear();
}

function sendMessageToSlack(text) {
    const TOKEN = PropertiesService.getScriptProperties().getProperty("TOKEN");

    const SLACK_APP = SlackApp.create(TOKEN);

    SLACK_APP.postMessage(channelId, text);
}

function sendMessageWithButtonsToSlack() {
    const TOKEN = PropertiesService.getScriptProperties().getProperty("TOKEN");

    const API_URL = "https://slack.com/api/chat.postMessage";

    const MESSAGE = "<@" + mentionedUserId + ">\n" + "そのプロンプトで生成したいものはどちらですか？";

    const BLOCKS =
        [
            {
                "type": "section",
                "text":
                {
                    "type": "mrkdwn",
                    "text": MESSAGE
                }
            },
            {
                "type": "actions",
                "elements":
                    [
                        {
                            "type": "button",
                            "text":
                            {
                                "type": "plain_text",
                                "text": "文章"
                            },
                            "style": "primary",
                            "value": "text"
                        },
                        {
                            "type": "button",
                            "text":
                            {
                                "type": "plain_text",
                                "text": "画像"
                            },
                            "style": "danger",
                            "value": "image"
                        }
                    ]
            }
        ];

    const PAYLOAD =
    {
        token: TOKEN,
        channel: channelId,
        text: MESSAGE,
        blocks: BLOCKS
    };

    const HEADERS =
    {
        "Authorization": "Bearer " + TOKEN
    };

    const OPTIONS =
    {
        method: "post",
        contentType: "application/json",
        payload: JSON.stringify(PAYLOAD),
        headers: HEADERS
    };

    UrlFetchApp.fetch(API_URL, OPTIONS);
}