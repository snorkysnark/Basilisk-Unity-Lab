using UnityEngine;

public static class TransformExtentions
{
    public static bool TryGetFirstChild(this Transform transform, out Transform child)
    {
        if(transform.childCount > 0)
        {
            child = transform.GetChild(0);
            return true;
        }
        child = null;
        return false;
    }
}