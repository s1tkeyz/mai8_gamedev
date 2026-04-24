using UnityEngine;
using System.Collections.Generic;

public class ScannerController : MonoBehaviour
{
    [Tooltip("Все рендереры станции будут найдены автоматически по тегу")]
    private List<Renderer> stationRenderers = new List<Renderer>();
    
    void Awake()
    {
        // Собираем все рендереры станции
        var stations = GameObject.FindGameObjectsWithTag("Station");
        foreach (var st in stations)
            stationRenderers.AddRange(st.GetComponentsInChildren<Renderer>(true));
    }

    void Update()
    {
        // Мгновенная проверка нажатия и отпускания
        bool isActive = Input.GetKey(KeyCode.R);
        float val = isActive ? 1f : 0f;

        foreach (var renderer in stationRenderers)
        {
            // Устанавливаем свойство напрямую без интерполяции
            renderer.material.SetFloat("_ScannerActive", val);
        }
    }
}