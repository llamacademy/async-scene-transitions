using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class SceneTransitioner : MonoBehaviour
{
    private static SceneTransitioner _instance;
    public static SceneTransitioner Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private Canvas TransitionCanvas;
    [SerializeField]
    private List<Transition> Transitions = new();
    
    private AsyncOperation LoadLevelOperation;
    private AbstractSceneTransitionScriptableObject ActiveTransition;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Invalid configuration. Duplicate Instances found! First one: {Instance.name} Second one: {name}. Destroying second one.");
            Destroy(gameObject);
            return;
        }

        SceneManager.activeSceneChanged += HandleSceneChange;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        TransitionCanvas = GetComponent<Canvas>();
        TransitionCanvas.enabled = false;
    }

    public void LoadScene(string Scene, 
        SceneTransitionMode TransitionMode = SceneTransitionMode.None, 
        LoadSceneMode Mode = LoadSceneMode.Single)
    {
        LoadLevelOperation = SceneManager.LoadSceneAsync(Scene);

        Transition transition = Transitions.Find(
            (transition) => transition.Mode == TransitionMode
        );
        if (transition != null)
        {
            LoadLevelOperation.allowSceneActivation = false;
            TransitionCanvas.enabled = true;
            ActiveTransition = transition.AnimationSO;
            StartCoroutine(Exit());
        }
        else
        {
            Debug.LogWarning($"No transition found for" +
                $" TransitionMode {TransitionMode}!" +
                $" Maybe you are misssing a configuration?");
        }
    }

    private IEnumerator Exit()
    {
        yield return StartCoroutine(ActiveTransition.Exit(TransitionCanvas));
        LoadLevelOperation.allowSceneActivation = true;
    }

    private IEnumerator Enter()
    {
        yield return StartCoroutine(ActiveTransition.Enter(TransitionCanvas));
        TransitionCanvas.enabled = false;
        LoadLevelOperation = null;
        ActiveTransition = null;
    }

    private void HandleSceneChange(Scene OldScene, Scene NewScene)
    {
        if (ActiveTransition != null)
        {
            StartCoroutine(Enter());
        }
    }

    [System.Serializable]
    public class Transition
    {
        public SceneTransitionMode Mode;
        public AbstractSceneTransitionScriptableObject AnimationSO;
    }
}
