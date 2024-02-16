#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace npcWorld.SceneManagement {
    [CustomEditor(typeof(SceneCollector))]
    public class SceneCollectorEditor : Editor {
        private SceneCollector _sceneCollector;
        private List<Scene> _oldScenes = new List<Scene>();
        bool _canSave = true;
        bool _canLoad = true;
        private void OnEnable() {
            _sceneCollector = (SceneCollector)target;
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scene Collection", EditorStyles.boldLabel);
            if (GUILayout.Button("Load Scene Collection") && _canLoad) { LoadSceneCollection(); }
            if (GUILayout.Button("Save Current Hierarchy") && _canSave) { SaveHierarchy(); }
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }

        void LoadSceneCollection() {
            _canLoad = false;
            _oldScenes.Clear();

            var basePath = AssetDatabase.GetAssetPath(_sceneCollector.BaseScene);

            Scene baseScene = EditorSceneManager.OpenScene(basePath, OpenSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(basePath));

            //unload old collection
            foreach (var scene in SceneManager.GetAllScenes()) {
                if (scene != baseScene) {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }

            //load new collection
            foreach (var sceneAsset in _sceneCollector.SceneCollection) {
                var newPath = AssetDatabase.GetAssetPath(sceneAsset);
                EditorSceneManager.OpenScene(newPath, OpenSceneMode.Additive);
            }

            _canLoad = true;
        }

        void SaveHierarchy() {
            _canSave = false;
            int c = SceneManager.sceneCount;
            _sceneCollector.SceneCollection.Clear();
            for (int i = 0; i < c; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                if (sceneAsset != _sceneCollector.BaseScene && !_sceneCollector.SceneCollection.Contains(sceneAsset)) {
                    _sceneCollector.SceneCollection.Add(sceneAsset);
                }
            }
            _canSave = true;
        }
    }
}
#endif