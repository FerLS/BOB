using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class Arrastrator : MonoBehaviour
{

    HingeJoint2D joint;
    Rigidbody2D rb;

    [HideInInspector] public bool _onDrag;

    public event BOBAnimator.onAnim OnDrag;




    private void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {       
        Follow();
    }

    void Follow()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector3.forward);

        if (Input.GetMouseButtonDown(0)&& hit.collider != null && hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
        {
            _onDrag = true;
            OnDrag?.Invoke(1);
            joint.anchor = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            joint.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0) && _onDrag)
        {
            _onDrag = false;
            OnDrag?.Invoke(0);
            joint.enabled = false;
            rb.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 500);
        }
        if (_onDrag)
        {
            joint.connectedAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
