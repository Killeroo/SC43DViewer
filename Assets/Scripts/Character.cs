using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject Camera;
    
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        gameObject.TryGetComponent(out _rigidbody);
        
        Debug.Assert(_rigidbody != null);
        Debug.Assert(Camera != null);
        Debug.Assert(_lineRenderer != null);
        
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.red;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody.AddForce(Camera.transform.right * 10, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidbody.AddForce(-Camera.transform.right * 10, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _rigidbody.AddForce(Camera.transform.forward * 10, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _rigidbody.AddForce(-Camera.transform.forward * 10, ForceMode.Acceleration);
        }
        
        Vector3 baseLinePos = transform.position + Vector3.up * 1;
        _lineRenderer.SetPosition(0, baseLinePos);
        _lineRenderer.SetPosition(1, baseLinePos + Camera.transform.forward * 10);
    }
}
