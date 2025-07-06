using UnityEngine;

public class pickUpThow : MonoBehaviour
{
    public Camera cam;
    public float pickUpRange = 3f;
    public Transform holdParent;
    public float throwForce = 500f;
    public float spring = 800f;
    public float damper = 5f;
    // angular damping for rotation
    // this will be applied to Rigidbody.angularDamping when held
    // and restored on throw
    private float originalAngularDamping;
    // angular damping for rotation
    // this will be applied to rigidbody.angularDrag when held
    // and restored on throw
    private float originalAngularDrag;

    private holdableObject heldObject;
    private SpringJoint springJoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickUp();
            else
                Throw();
        }

        if (springJoint != null)
            springJoint.connectedAnchor = holdParent.position;
    }

    void TryPickUp()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            holdableObject ho = hit.collider.GetComponent<holdableObject>();
            if (ho != null)
            {
                heldObject = ho;
                Rigidbody rb = ho.rb;
        // save original angular damping and apply damping
        originalAngularDamping = rb.angularDamping;
        rb.angularDamping = damper;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                springJoint = ho.gameObject.AddComponent<SpringJoint>();
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.connectedAnchor = holdParent.position;
                springJoint.spring = spring;
                springJoint.damper = damper;
                springJoint.maxDistance = 0f;
            }
        }
    }

    void Throw()
    {
        Rigidbody rb = heldObject.rb;
    // restore original angular damping
    rb.angularDamping = originalAngularDamping;
        springJoint.connectedAnchor = holdParent.position;
        Destroy(springJoint);
        heldObject = null;
        rb.AddForce(cam.transform.forward * throwForce);
    }
}
