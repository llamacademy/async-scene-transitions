using UnityEngine;

public class DemoUI : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionMode TransitionMode;

    public void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "Load Game Scene"))
        {
            SceneTransitioner.Instance.LoadScene("Game", TransitionMode);
        }
        if (GUI.Button(new Rect(10, 50, 200, 30), "Load Menu Scene"))
        {
            SceneTransitioner.Instance.LoadScene("Menu", TransitionMode);
        }
    }
}
