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

        public const string ERROR_ALREADY_EXIST_SAVE_DATA_NAME = "���̃Z�[�u�f�[�^���͊��Ɏg���Ă��܂��B";

        public const string ERROR_UNCORRECT_SAVE_DATA_NAME = "�������Z�[�u�f�[�^������͂��Ă��������B";

        public const string ERROR_UNCORRECT_PASSCODE = "�������p�X�R�[�h����͂��Ă��������B";

        public const string ERROR_NEED_LEAST_ONE_MEMBER = "���Ȃ��Ƃ�1�l�̃����o�[���K�v�ł��B";

        public const string ERROR_CANNOT_ADD_MORE_MEMBERS = "����ȏ�A�����o�[�𑝂₹�܂���B";

        public const string ERROR_NO_NAME_MEMBER = "���O�̓��͂���Ă��Ȃ������o�[�����܂��B";

        public const string ERROR_FAILD_TO_GET_SAVE_DATA = "�T�[�o�[����̃Z�[�u�f�[�^�̎擾�Ɏ��s���܂����B\n�ʂ̃Z�[�u�f�[�^�Ƀ��O�C�����邩�A�C���^�[�l�b�g�ڑ����Ċm�F���Ă����蒼���Ă��������B";

        public const string ERROR_FAILED_TO_SAVE_TO_DATA_BASE = "�T�[�o�[�ւ̃Z�[�u�f�[�^�̕ۑ��Ɏ��s���܂����B\n�ʂ̃Z�[�u�f�[�^�Ƀ��O�C�����邩�A�C���^�[�l�b�g�ڑ����Ċm�F���Ă����蒼���Ă��������B";

        public const string ERROR_FAILD_TO_GET_JWT = "JSON Web Token �̎擾�Ɏ��s���܂����B\n�C���^�[�l�b�g�ڑ����Ċm�F���Ă����蒼���Ă��������B";

        public const string ERROR_CORRUPTION_SAVE_DATA = "�Z�[�u�f�[�^���j�����Ă��܂��B\n�ʂ̃Z�[�u�f�[�^�Ƀ��O�C�����邩�A�V���ɃZ�[�u�f�[�^���쐬���Ă��������B";

        public const string ERROR_FAILD_TO_GET_CLOUD_STORAGE_OBJECTS = "�ǉ��R���e���c�̎擾�Ɏ��s���܂����B\n�C���^�[�l�b�g�ڑ����Ċm�F���Ă����蒼���Ă��������B";

        public const string BUTTON_TEXT_CHECKING = "�m�F��";

        public const string BUTTON_TEXT_NEXT = "����";

        public const string BUTTON_TEXT_SUCCESS = "����";

        public const string BUTTON_TEXT_ERROR = "�G���[";

        public const string BUTTON_TEXT_SAVE = "�ۑ�";

        public const string BUTTON_TEXT_SAVING = "�ۑ���";

        public const string BUTTON_TEXT_MAKE_SAVE_DATA = "�Z�[�u�f�[�^���쐬";

        public const string BUTTON_TEXT_START = "�X�^�[�g";

        public const string BUTTON_TEXT_STOP = "�X�g�b�v";

        public const string BUTTON_TEXT_LOGOUT = "���O�A�E�g";

        public const string BUTTON_TEXT_BACK = "�߂�";

        public const string SAVE_DATA_NAME = "SaveData";
    }
}