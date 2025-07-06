using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class holdableObject : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
