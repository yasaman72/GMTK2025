using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    public class GameManager : Singleton<GameManager>
    {
        // TODO: replace this solution after using single point of entry architecture
        private void Start()
        {
            StartInitiatables();
        }

        [ContextMenu("GetInitiatables")]
        private void StartInitiatables()
        {
            List<IInitiatable> initiatables = new List<IInitiatable>();

            var rootObjects = gameObject.scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                initiatables.AddRange(rootObject.GetComponentsInChildren<IInitiatable>(true).ToList());
            }

            foreach (IInitiatable initable in initiatables)
            {
                initable.Initiate();
            }
        }
    }
}
