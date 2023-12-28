using UnityEngine;

public class PlayerWalkSound : AudioPlayer
{
    [SerializeField] private AudioClip stepSound;

    public void PlayWalkSound()
    {
        PlayClipwithVariablePitch(stepSound);
    }

    public void StopWalkSound()
    {
        base.StopClip();
    }
}
