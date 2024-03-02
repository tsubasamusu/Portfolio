namespace Roulette
{
    public static class ConstData
    {
        public const float RATIO_DISK_ROTATION_SPEED_REDUCTION_PER_FRAMES = 0.99f;

        public const float TIME_FADE = 0.5f;

        public const float TIME_BUTTON_ANIMATION = 0.1f;

        public const float TIME_SPAN_ROTATE_LOADING = 0.1f;

        public const float ANGLE_ROTATE_LOADING = 360f / 8f;

        public const float ANGLE_ROTATE_DISK_PER_SECONDS = 360f * 3f;

        public const float WIDTH_BUTTON_SAVE = 300f;

        public const float WIDTH_BUTTON_MAKE_SAVE_DATA = 800f;

        public const int SPAN_ISSUE_GOOGLE_CLOUD_JWT = 3000;

        public const int SHEET_COLUMN_SAVE_DATA_NAME = 1;

        public const int SHEET_COLUMN_PASSCODE = 2;

        public const int SHEET_COLUMN_JASON_DATA = 3;

        public const int SHEET_ROW_FIRST = 2;

        public const int MAX_LENGTH_SAVE_DATA_NAME = 15;

        public const int MAX_LENGTH_PASSCODE = 15;

        public const int MEX_LENGTH_MEMBER_NAME = 12;

        public const int MAX_MAMBERS_COUNT = 12;

        public const string SCENE_NAME_TITLE = "TitleScene";

        public const string SCENE_NAME_LOGIN = "LoginScene";

        public const string SCENE_NAME_SET_MAMBERS = "SetMembersScene";

        public const string SCENE_NAME_ROULETTE = "RouletteScene";

        public const string SCENE_NAME_MAKE_SAVE_DATA = "MakeSaveDataScene";

        public const string ERROR_ALREADY_EXIST_SAVE_DATA_NAME = "そのセーブデータ名は既に使われています。";

        public const string ERROR_UNCORRECT_SAVE_DATA_NAME = "正しいセーブデータ名を入力してください。";

        public const string ERROR_UNCORRECT_PASSCODE = "正しいパスコードを入力してください。";

        public const string ERROR_NEED_LEAST_ONE_MEMBER = "少なくとも1人のメンバーが必要です。";

        public const string ERROR_CANNOT_ADD_MORE_MEMBERS = "これ以上、メンバーを増やせません。";

        public const string ERROR_NO_NAME_MEMBER = "名前の入力されていないメンバーがいます。";

        public const string ERROR_FAILD_TO_GET_SAVE_DATA = "サーバーからのセーブデータの取得に失敗しました。\n別のセーブデータにログインするか、インターネット接続を再確認してからやり直してください。";

        public const string ERROR_FAILED_TO_SAVE_TO_DATA_BASE = "サーバーへのセーブデータの保存に失敗しました。\n別のセーブデータにログインするか、インターネット接続を再確認してからやり直してください。";

        public const string ERROR_FAILD_TO_GET_JWT = "JSON Web Token の取得に失敗しました。\nインターネット接続を再確認してからやり直してください。";

        public const string ERROR_CORRUPTION_SAVE_DATA = "セーブデータが破損しています。\n別のセーブデータにログインするか、新たにセーブデータを作成してください。";

        public const string ERROR_FAILD_TO_GET_CLOUD_STORAGE_OBJECTS = "追加コンテンツの取得に失敗しました。\nインターネット接続を再確認してからやり直してください。";

        public const string BUTTON_TEXT_CHECKING = "確認中";

        public const string BUTTON_TEXT_NEXT = "次へ";

        public const string BUTTON_TEXT_SUCCESS = "成功";

        public const string BUTTON_TEXT_ERROR = "エラー";

        public const string BUTTON_TEXT_SAVE = "保存";

        public const string BUTTON_TEXT_SAVING = "保存中";

        public const string BUTTON_TEXT_MAKE_SAVE_DATA = "セーブデータを作成";

        public const string BUTTON_TEXT_START = "スタート";

        public const string BUTTON_TEXT_STOP = "ストップ";

        public const string BUTTON_TEXT_LOGOUT = "ログアウト";

        public const string BUTTON_TEXT_BACK = "戻る";

        public const string SAVE_DATA_NAME = "SaveData";
    }
}