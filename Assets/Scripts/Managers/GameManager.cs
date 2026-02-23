using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deviloop
{
    public class GameManager : Singleton<GameManager>
    {
        public static AreaData CurrentModeAreaData;
        static List<IInitiatable> _initiatables = new List<IInitiatable>();

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        // TODO: replace this solution after using single point of entry architecture
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartInitiatables(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            foreach (IInitiatable initable in _initiatables)
            {
                initable.Deactivate();
            }
        }

        [ContextMenu("GetInitiatables")]
        private void StartInitiatables(Scene scene)
        {
            _initiatables = new List<IInitiatable>();

            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                _initiatables.AddRange(rootObject.GetComponentsInChildren<IInitiatable>(true).ToList());
            }

            foreach (IInitiatable initable in _initiatables)
            {
                initable.Initiate();
            }
        }
    }
}
