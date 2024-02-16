#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace npcWorld.SceneManagement {
    [CreateAssetMenu(fileName = "newSceneCollection", menuName = "NpcWorld/SceneManagement/SceneCollection", order = 1)]
    public class SceneCollector : ScriptableObject {
        public SceneAsset BaseScene;
        public List<SceneAsset> SceneCollection = new List<SceneAsset>();
        private void Awake() {
            if (SceneCollection.Contains(BaseScene)) {
                SceneCollection.Remove(BaseScene);
            }
        }
    }
}
#endif
