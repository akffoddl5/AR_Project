using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform targetPos;
    [SerializeField] Transform lookPos;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotSpeed = 5f;
    [SerializeField] float moveCount = 0.5f;
    [SerializeField] float moveDistance = 0.2f;
    bool animationEnd = false;
    bool updatePlay = false;
    float timeCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimalStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (!updatePlay)
            return;

        if (Vector3.Distance(transform.position, targetPos.position) > moveDistance)
        {
            TargetMove();
        }
        else if (Input.touchCount > 0)
        {
            TouchAnimal();
        }
        else
        {
            LookAtCamera();
        }
    }

    private void LookAtCamera()
    {
        timeCount = 0;

        anim.Play("Idle_A");

        Vector3 dir = lookPos.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);
    }

    private static void TouchAnimal()
    {
        Touch touch = Input.GetTouch(0);

        //첫번쨰 터치가 입력이 움직이는 중이라면
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = touch.deltaPosition;
            Vector2 screen_pos = touch.position;


            RaycastHit _hit;
            var a = Camera.main.ScreenPointToRay(screen_pos);
            var b = Physics.Raycast(a, out _hit, LayerMask.GetMask("Player"));

            if (b)
            {
                if (touchDelta.x > 0)
                {
                    _hit.transform.gameObject.GetComponent<Animator>().Play("Spin");
                }
                else
                {
                    _hit.transform.gameObject.GetComponent<Animator>().Play("Spin_R");

                }


            }
            else
            {
                //Debug.Log("못찾음");
            }

        }
    }

    IEnumerator AnimalStart()
    {
        anim.Play("StartSit");

        yield return new WaitForSeconds(3f);

        anim.Play("StandUp");

        while (!animationEnd)
        {
            yield return null;
        }
        animationEnd = false;

        yield return new WaitForSeconds(2f);

        //yield return TargetMove();

        updatePlay = true;
    }


    public void AnimationEnd()
    {
        animationEnd = true;
    }

    void TargetMove()
    {
        timeCount += Time.deltaTime;

        if (timeCount >= moveCount)
        {
            anim.Play("Walk");

            Vector3 dir = targetPos.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);

            transform.rotation = rot;

            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        }

    }

    //IEnumerator TargetMove()
    //{
    //    anim.Play("Walk");

    //    while (Vector3.Distance(transform.position, targetPos.position) > 0.1f)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);

    //        yield return null;
    //    }

    //    //transform.LookAt(Camera.main.transform);

    //    anim.speed = 0.5f;

    //    Vector3 dir = Camera.main.transform.position - transform.position;
    //    Quaternion rot = Quaternion.LookRotation(dir);

    //    while (transform.rotation != rot)
    //    {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);

    //        yield return null;
    //    }

    //    anim.speed = 1f;

    //    anim.Play("Idle_A");
    //}
}
