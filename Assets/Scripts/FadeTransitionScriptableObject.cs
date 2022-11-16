using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fade", menuName = "Scene Transitions/Fade")]
public class FadeTransitionScriptableObject : AbstractSceneTransitionScriptableObject
{
    public override IEnumerator Enter(Canvas Parent)
    {
        float time = 0;
        Color startColor = Color.black;
        Color endColor = new Color(0, 0, 0, 0);
        while (time < 1)
        {
            AnimatedObject.color = Color.Lerp(
                startColor, 
                endColor, 
                LerpCurve.Evaluate(time)
            );
            yield return null;
            time += Time.deltaTime / AnimationTime;
        }

        Destroy(AnimatedObject.gameObject);
    }

    public override IEnumerator Exit(Canvas Parent)
    {
        AnimatedObject = CreateImage(Parent);
        AnimatedObject.rectTransform.anchorMin = Vector2.zero;
        AnimatedObject.rectTransform.anchorMax = Vector2.one;
        AnimatedObject.rectTransform.sizeDelta = Vector2.zero;

        float time = 0;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = Color.black;
        while (time < 1)
        {
            AnimatedObject.color = Color.Lerp(
                startColor, 
                endColor, 
                LerpCurve.Evaluate(time)
            );
            yield return null;
            time += Time.deltaTime / AnimationTime;
        }
    }
}
