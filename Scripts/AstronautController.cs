using UnityEngine;

public class AstronautController : MonoBehaviour
{
    public float thrustForce = 15f;
    public float rotationSpeed = 120f;
    
    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        float forward = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float strafe  = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float up      = Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftControl) ? -1 : 0;

        Vector3 localForce = new Vector3(strafe, up, forward).normalized * thrustForce;
        rb.AddForce(transform.TransformDirection(localForce), ForceMode.Force);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        Quaternion yaw = Quaternion.Euler(0f, mouseX * rotationSpeed * Time.fixedDeltaTime, 0f);
        Quaternion pitch = Quaternion.Euler(-mouseY * rotationSpeed * Time.fixedDeltaTime, 0f, 0f);
        
        rb.MoveRotation(rb.rotation * yaw * pitch);
    }
}