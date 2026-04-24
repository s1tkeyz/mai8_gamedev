using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TetherSystem : MonoBehaviour
{
    public Transform anchor;
    public float maxTetherLength = 8f;
    public float minTetherLength = 2f;
    public float scrollSpeed = 1.5f;
    
    [Header("Физика троса")]
    public float stiffness = 20f;
    public float damping = 8f;

    public LineRenderer tetherLine;
    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        // Управление длиной колесиком
        float scroll = Input.mouseScrollDelta.y;
        maxTetherLength = Mathf.Clamp(maxTetherLength + scroll * scrollSpeed, minTetherLength, 40f);

        // Обновление визуала
        if (tetherLine != null)
        {
            tetherLine.SetPosition(0, transform.position);
            tetherLine.SetPosition(1, anchor.position);
        }

        // Физика ограничения
        Vector3 toAnchor = anchor.position - transform.position;
        float currentDist = toAnchor.magnitude;

        if (currentDist > maxTetherLength)
        {
            Vector3 dir = toAnchor.normalized;
            float stretch = currentDist - maxTetherLength;
            
            // Пружинная сила
            Vector3 force = dir * stretch * stiffness;
            
            // Гашение колебаний (демпфирование)
            float velAlongTether = Vector3.Dot(rb.linearVelocity, dir);
            force -= dir * velAlongTether * damping;
            
            rb.AddForce(force, ForceMode.Force);
        }
    }
}