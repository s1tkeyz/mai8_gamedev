using UnityEngine;
using System.Collections.Generic;

public class ScannerController : MonoBehaviour
{
    private List<Renderer> stationRenderers = new List<Renderer>();
    
    void Awake()
    {
        var stations = GameObject.FindGameObjectsWithTag("Station");
        foreach (var st in stations)
            stationRenderers.AddRange(st.GetComponentsInChildren<Renderer>(true));
    }

    void Update()
    {
        bool isActive = Input.GetKey(KeyCode.R);
        float val = isActive ? 1f : 0f;

        foreach (var renderer in stationRenderers)
        {
            renderer.material.SetFloat("_ScannerActive", val);
        }
    }
}