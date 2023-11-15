using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BOBIA : MonoBehaviour
{
    [Header("Components")]

    Rigidbody2D rb;
    public Arrastrator arrastrator;

    [Header("FloorDetection")]

    public float distanceFloor = 2;
    public LayerMask floorLayer;

    [Header("Detection")]

    public float closestRadious = 3;
    public float scanRadious = 10;


    [Header("Move")]

    bool moving;
    public float moveVelocity = 10;
    public static event BOBAnimator.onAnim OnMove;

    [Header("Jump")]

    public float minDistanceLastMove = 4;
    public float jumpPower = 10;

    [Header("GetUP")]

    public float waitTimeUP = 1;
    bool gettingUp;



    [Header("Personlity")]

    public float pacience = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (EnSuelo())
        {            
            if(!gettingUp && (transform.eulerAngles.z > 85 && transform.eulerAngles.z < 265)) StartCoroutine(GetUp());
        }

        LinearDrag();
        ScanForItems();
    }


    #region Movility

    IEnumerator GetUp()
    {
        gettingUp = true;
        Debug.Log("Getting up");
        float waitTime = waitTimeUP;

        while ( waitTime > 0)
        {
            if (!EnSuelo())
            {
                gettingUp = false;
                yield break;
            }
            waitTime -= Time.deltaTime;
            yield return null;
        }

        yield return  transform.DORotate(Vector3.zero, 0.6f).WaitForCompletion();

        gettingUp = false;
    }

   
    
    IEnumerator MoveTo(GameObject objeto)
    {
        moving = true;

        float previousSeconds = 0, currentSeconds = 0, lastPosInSecond = transform.position.x;


        RaycastHit2D hit = Physics2D.CircleCast(transform.position, closestRadious, Vector3.forward);
        while (objeto != null && (hit.collider == null || hit.collider.gameObject.name != objeto.name))
        {
            if (gettingUp) yield return new WaitUntil(() => !gettingUp);

            OnMove?.Invoke(Mathf.RoundToInt((objeto.transform.position - transform.position).normalized.x));
            hit = Physics2D.CircleCast(transform.position, closestRadious, Vector3.forward);

            Vector2 force = (objeto.transform.position - transform.position).normalized* Vector2.right;

            rb.AddForce(force * moveVelocity);



            if (Mathf.Sign(rb.velocity.normalized.x) != Mathf.Sign(force.x))
            {
                rb.drag = 5;
            }




            currentSeconds += Time.deltaTime;

            if (Mathf.Floor(currentSeconds) > previousSeconds)
            {

                if (Mathf.Abs(transform.position.x - lastPosInSecond) < minDistanceLastMove /* cantidad de movimiento desde el ultimo segundo*/)
                {
                    if(CanReachJump(objeto)) JumpTo(objeto.transform.position);
                }

                previousSeconds = Mathf.Floor(currentSeconds);
                lastPosInSecond = transform.position.x;


            }

            yield return null;
        }
        OnMove?.Invoke(0);
        moving = false;
    }
    IEnumerator MoveTo(Vector3 pos)
    {
        moving = true;


        while (Mathf.Abs((pos-transform.position).magnitude) > closestRadious)
        {
            OnMove?.Invoke(Mathf.RoundToInt((pos - transform.position).normalized.x));
            rb.AddForce((pos - transform.position).normalized * Vector2.right * moveVelocity);



            yield return null;
        }
        OnMove?.Invoke(0);
        moving = false;
    }

    public void JumpTo(Vector3 pos)
    {
        rb.AddForce(jumpPower * (pos -transform.position).normalized , ForceMode2D.Impulse);
    }

    void LinearDrag()
    {
        rb.drag = EnSuelo() && !moving ? 5 : 0;
    }
    #endregion

    #region Logic

    bool CanReachJump(GameObject objeto)
    {
        return (objeto.transform.position- transform.position).magnitude < (0.4 * jumpPower);
    }

    #endregion

    #region Detection

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            Destroy(collision.gameObject);
        }
    }
    bool EnSuelo()
    {
        return Physics2D.Raycast(transform.position, Vector2.down,distanceFloor,floorLayer);
    }
    void ScanForItems()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position -(Vector3.forward * 10), scanRadious,Vector3.forward);

        if(hit.collider!= null && hit.collider.gameObject.CompareTag("Apple") && !moving && !arrastrator._onDrag)
        {
            StartCoroutine(MoveTo(hit.collider.gameObject));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * distanceFloor);
        Gizmos.DrawWireSphere(transform.position, scanRadious);
        Gizmos.DrawWireSphere(transform.position, closestRadious);


    }
    #endregion
}
