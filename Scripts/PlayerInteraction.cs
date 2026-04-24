using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Настройки")]
    public Camera playerCamera;
    public float detectionRange = 8f;
    public float repairRange = 3f;
    public LayerMask moduleLayer;

    [Header("UI")]
    public Text statusTextUI;
    public Text actionTextUI;

    private ModuleController currentTarget;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        // Луч игнорирует всё, кроме слоя Modules
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, moduleLayer))
        {
            ModuleController mod = hit.collider.GetComponent<ModuleController>();
            if (mod != null)
            {
                currentTarget = mod;
                bool canRepair = hit.distance <= repairRange;

                // Обновляем статус
                string status = mod.IsBroken ? "<color=red>ТРЕБУЕТ РЕМОНТА</color>" : "<color=green>ИСПРАВЕН</color>";
                statusTextUI.text = $"{mod.moduleName} | {status}";

                // Подсказка и ремонт
                if (mod.IsBroken && canRepair)
                {
                    actionTextUI.text = "[F] Починить модуль";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        mod.RepairModule();
                        Debug.Log($"🔧 Модуль {mod.moduleName} отремонтирован!");
                    }
                }
                else
                {
                    actionTextUI.text = "";
                }
                return;
            }
        }

        // Сброс если луч ни во что не попал или попал не в модуль
        currentTarget = null;
        statusTextUI.text = "";
        actionTextUI.text = "";
    }
}