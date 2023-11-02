using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform targetPos;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotSpeed = 5f;
    bool animationEnd = false;
    bool updatePlay = false;

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

        yield return TargetMove();

        updatePlay = true;
    }


    public void AnimationEnd()
    {
        animationEnd = true;
    }

    private void OnBecameInvisible()
    {
        updatePlay = false;


    }

    IEnumerator TargetMove()
    {
        anim.Play("Walk");

        while (Vector3.Distance(transform.position, targetPos.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        //transform.LookAt(Camera.main.transform);

        anim.speed = 0.5f;

        Vector3 dir = Camera.main.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);

        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);

            yield return null;
        }

        anim.speed = 1f;

        anim.Play("Idle_A");
    }
}
