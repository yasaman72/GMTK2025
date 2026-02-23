using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deviloop
{
    public class UIViewsManager : Singleton<UIViewsManager>
    {
        private Stack<UIView> _viewStack = new Stack<UIView>();

        [SerializeField] private List<UIView> _screens;

        public T OpenPage<T>() where T : UIView
        {
            // Find existing view in scene
            T view = _screens.OfType<T>().FirstOrDefault();

            if (view == null)
            {
                Debug.LogError($"UIView of type {typeof(T).Name} not found in scene.");
                return null;
            }

            if (view.OpenIsolated && _viewStack.Count > 0)
            {
                while (_viewStack.Count > 0)
                {
                    UIView v = _viewStack.Pop();
                    v.Close();
                }
            }

            _viewStack.Push(view);
            view.Open();

            return view;
        }

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
