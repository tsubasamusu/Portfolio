namespace LightingDemonstration
{
    public class UiManager_Preview : UiManagerBase
    {
        public void MoveToHomeScene() => LoadSceneWithFadeOut(ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Home));
    }
}