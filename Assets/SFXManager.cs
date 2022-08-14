using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    AudioSource audioSource;

    [SerializeField] AudioClip collectIngredient;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip[] ouch;
    [SerializeField] AudioClip throwGarbage;
    [SerializeField] AudioClip throwPot;
    [SerializeField] AudioClip foodDeliver;
    [SerializeField] AudioClip customerLeave;
    [SerializeField] AudioClip customerArrive;
    [SerializeField] AudioClip slap;

    public void playCollectIngredient()
    {
        if (collectIngredient)
        {
            audioSource.PlayOneShot(collectIngredient);
        }
    }
    public void playbuttonClick()
    {
        if (buttonClick)
        {
            audioSource.PlayOneShot(buttonClick);
        }
    }
    public void playthrowGarbage()
    {
        if (throwGarbage)
        {
            audioSource.PlayOneShot(throwGarbage);
        }
    }
    public void playthrowPot()
    {
        if (throwPot)
        {
            audioSource.PlayOneShot(throwPot);
        }
    }
    public void playfoodDeliver()
    {
        if (foodDeliver)
        {
            audioSource.PlayOneShot(foodDeliver);
        }
    }
    public void playcustomerLeave()
    {
        if (customerLeave)
        {
            audioSource.PlayOneShot(customerLeave);
        }
    }
    public void playcustomerArrive()
    {
        if (customerArrive)
        {
            audioSource.PlayOneShot(customerArrive);
        }
    }
    public void playslap()
    {
        if (slap)
        {
            audioSource.PlayOneShot(slap);
        }
    }



    public void playOuch()
    {
        if (ouch.Length>0)
        {
            audioSource.PlayOneShot(ouch[Random.Range(0, ouch.Length)]);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
