using System;
using UnityEngine.UI;

namespace Deviloop
{
    namespace Utils.IDisposableUtils
    {
        public class DisposableLockUIInput : IDisposable
        {
            private readonly GraphicRaycaster _graphic;

            public DisposableLockUIInput(GraphicRaycaster graphic)
            {
                _graphic = graphic;
                graphic.enabled = false;
            }

            public void Dispose()
            {
                _graphic.enabled = true;
            }
        }
    }
}
