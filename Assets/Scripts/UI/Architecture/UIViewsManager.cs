using System.Collections.Generic;

namespace Deviloop
{
    public class UIViewsManager : Singleton<UIViewsManager>
    {
        private Stack<UIView> _viewStack = new Stack<UIView>();

        public void OpenPage(UIView view)
        {
            if (view.OpenIsolated && _viewStack.Count > 0)
            {
                while (_viewStack.Count > 0)
                {
                    {
                        UIView v = _viewStack.Pop();
                        v.Close();
                    }
                }
            }

            _viewStack.Push(view);
            view.Open();
        }

        public void ClosePage()
        {
            _viewStack.Pop();
        }
    }
}
