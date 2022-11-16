using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractSceneTransitionScriptableObject : ScriptableObject
{
    public AnimationCurve LerpCurve;
    public float AnimationTime = 0.25f;
    protected Image AnimatedObject;

    public abstract IEnumerator Enter(Canvas Parent);
    public abstract IEnumerator Exit(Canvas Parent);

    protected virtual Image CreateImage(Canvas Parent)
    {
        GameObject child = new GameObject("Transition Image");
        child.transform.SetParent(Parent.transform, false);

        return child.AddComponent<Image>();
    }
}