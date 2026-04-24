using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float detectionRange = 8f;
    public float repairRange = 3f;
    public LayerMask moduleLayer;

    public Text statusTextUI;
    public Text actionTextUI;

    private ModuleController currentTarget;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, moduleLayer))
        {
            ModuleController mod = hit.collider.GetComponent<ModuleController>();
            if (mod != null)
            {
                currentTarget = mod;
                bool canRepair = hit.distance <= repairRange;

                string status = mod.IsBroken ? "<color=red>ТРЕБУЕТ РЕМОНТА</color>" : "<color=green>ИСПРАВЕН</color>";
                statusTextUI.text = $"{mod.moduleName} | {status}";

                if (mod.IsBroken && canRepair)
                {
                    actionTextUI.text = "[F] Починить модуль";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        mod.RepairModule();
                    }
                }
                else
                {
                    actionTextUI.text = "";
                }
                return;
            }
        }

        currentTarget = null;
        statusTextUI.text = "";
        actionTextUI.text = "";
    }
}