using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public enum MoveMode { Crawl, Walk, Run }

    [Header("References")]
    [SerializeField] private Transform visual;

    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float crawlSpeed = 2f;

    [Header("Movement")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float turnSpeed = 14f;

    private Rigidbody rb;
    private Vector3 inputWorld;
    private MoveMode mode = MoveMode.Walk;
    private Transform camTransform;

    public MoveMode CurrentMode => mode;

    public float SpeedNormalized
    {
        get
        {
            Vector3 v = rb.linearVelocity;
            float horizontal = new Vector2(v.x, v.z).magnitude;

            float max = mode == MoveMode.Run ? runSpeed :
                        mode == MoveMode.Walk ? walkSpeed :
                        crawlSpeed;

            return (max <= 0.01f) ? 0f : Mathf.Clamp01(horizontal / max);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (visual == null) visual = transform;

        Camera cam = Camera.main;
        camTransform = cam != null ? cam.transform : null;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        bool runHeld = Input.GetKey(KeyCode.LeftShift);
        bool crawlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (crawlHeld) mode = MoveMode.Crawl;
        else if (runHeld) mode = MoveMode.Run;
        else mode = MoveMode.Walk;

        if (camTransform != null)
        {
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            inputWorld = camForward * input.z + camRight * input.x;
        }
        else
        {
            inputWorld = input;
        }

        if (inputWorld.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputWorld, Vector3.up);
            visual.rotation = Quaternion.Slerp(visual.rotation, targetRot, 1f - Mathf.Exp(-turnSpeed * Time.deltaTime));
        }
    }

    private void FixedUpdate()
    {
        float targetSpeed = mode switch
        {
            MoveMode.Crawl => crawlSpeed,
            MoveMode.Run => runSpeed,
            _ => walkSpeed
        };

        Vector3 targetVel = inputWorld * targetSpeed;

        Vector3 currentVel = rb.linearVelocity;
        Vector3 currentHorizontal = new Vector3(currentVel.x, 0f, currentVel.z);

        Vector3 newHorizontal = Vector3.MoveTowards(
            currentHorizontal,
            targetVel,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(newHorizontal.x, currentVel.y, newHorizontal.z);
    }
}