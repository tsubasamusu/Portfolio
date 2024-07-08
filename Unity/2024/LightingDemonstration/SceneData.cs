using System;

namespace LightingDemonstration
{
    [Serializable]
    public struct SceneData
    {
        public SceneType sceneType;

        public string sceneName;

        public bool IsValid() => !string.IsNullOrEmpty(sceneName);
    }
}