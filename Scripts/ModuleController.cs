using UnityEngine;

public class ModuleController : MonoBehaviour
{
    [Header("Информация")]
    public string moduleName = "Module A";
    
    [Header("Настройки поломки")]
    [Tooltip("Вероятность выхода из строя при каждой проверке (0-1)")]
    public float breakProbability = 0.1f;
    [Tooltip("Интервал проверки вероятности поломки (сек)")]
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
        // Мгновенно меняем цвет обшивки в шейдере
        modRenderer.material.SetColor("_HullColor", IsBroken ? Color.red : originalHullColor);
    }
}