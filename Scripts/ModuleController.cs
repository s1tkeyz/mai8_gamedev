using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public string moduleName = "Module A";
    public float breakProbability = 0.1f;
    public float checkInterval = 5f;

    public bool IsBroken { get; private set; }

    private Renderer modRenderer;
    private Color originalHullColor;
    private float timer;

    void Awake()
    {
        modRenderer = GetComponent<Renderer>();
        if (modRenderer != null)
            originalHullColor = modRenderer.material.GetColor("_HullColor");
    }

    void Start() => UpdateVisual();

    void Update()
    {
        if (IsBroken) return;

        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            if (Random.value <= breakProbability)
                BreakModule();
        }
    }

    public void BreakModule()
    {
        IsBroken = true;
        UpdateVisual();
    }

    public void RepairModule()
    {
        IsBroken = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (modRenderer == null) return;
        modRenderer.material.SetColor("_HullColor", IsBroken ? Color.red : originalHullColor);
    }
}