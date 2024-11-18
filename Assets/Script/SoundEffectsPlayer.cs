using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{ 
    public AudioSource Chasemusicbg;
    public AudioSource SFXSource;
    public AudioSource SFXLaugh;

    public AudioClip walk, run, chase, laugh, laughidle,laughrun;
    
    public void sfx1()
    {
        SFXSource.clip = walk;
        SFXSource.Play();
    }
    public void sfx2()
    {
        SFXSource.clip = run;
        SFXSource.Play();
    }
    public void sfx3()
    {
        SFXSource.clip = laugh;
        SFXSource.Play();
    }
    public void sfx4()
    {
        SFXLaugh.clip = laughidle;
        SFXLaugh.Play();
    }
    public void sfx5()
    {
        SFXLaugh.clip = laughrun;
        SFXLaugh.Play();
    }
    public void bgm()
    {
        Chasemusicbg.clip = chase;
        Chasemusicbg.Play();
    }
}