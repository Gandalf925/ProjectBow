using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    [SerializeField] ParticleSystem particle1;
    [SerializeField] ParticleSystem particle2;
    Animator anim;


    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public IEnumerator Activate()
    {
        anim.SetBool("isActivated", true);
        yield return new WaitForSeconds(0.5f);
        if (particle1 != null)
        {
            particle1.Play();
        }
        if (particle2 != null)
        {
            particle2.Play();
        }
    }
}
