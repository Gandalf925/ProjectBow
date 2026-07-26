using UnityEngine;
using UnityEngine.EventSystems;

public static class PointerInputUtility
{
    public const int MousePointerId = -1;

    public static bool IsPointerOverUI(int pointerId)
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        return pointerId == MousePointerId
            ? EventSystem.current.IsPointerOverGameObject()
            : EventSystem.current.IsPointerOverGameObject(pointerId);
    }

    public static bool TryGetTouch(int fingerId, out Touch touch)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch candidate = Input.GetTouch(i);
            if (candidate.fingerId == fingerId)
            {
                touch = candidate;
                return true;
            }
        }

        touch = default;
        return false;
    }
}
