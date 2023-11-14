using UnityEngine;

namespace Unit
{
    interface IComponent
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        Component component { get; }
        bool isDestroyed { get; }
    }

    public class Helpers
    {
        public static bool IsNullOrDestroyed(object obj)
        {
            if (obj is IComponent)
                return (obj as IComponent).isDestroyed;
            else if (obj is Component)
                return (obj as Component) == null;
            else
                return obj == null;
        }
    }
}