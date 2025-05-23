using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    public Transform UIRoot => canvasTransform;

    [SerializeField] private GameObject canvasPrefab;
    private Transform canvasTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitCanvas();
    }

    private void InitCanvas()
    {
        if (FindFirstObjectByType<Canvas>() == null)
        {
            GameObject canvasGO = Instantiate(canvasPrefab);
            DontDestroyOnLoad(canvasGO);
            canvasTransform = canvasGO.transform;
        }
        else
        {
            canvasTransform = FindFirstObjectByType<Canvas>().transform;
        }
    }
}
