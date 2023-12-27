using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioObj : PoolableMono
{
    private AudioSource _audioSource;

    public float _pitchRandomness = 0.2f;
    private float _basePitch;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _basePitch = _pitchRandomness;
    }

    public override void Init()
    {
        // do nothing
    }

    public void PlayClipwithVariablePitch(AudioClip clip)
    {
        StartCoroutine(PlayClipwithVariablePitchCor(clip));
    }

    public void PlayClip(AudioClip clip)
    {
        StartCoroutine(PlayClipCor(clip));
    }

    private IEnumerator PlayClipwithVariablePitchCor(AudioClip clip)
    {
        float randomPitcch = Random.Range(-_pitchRandomness, +_pitchRandomness);
        _audioSource.pitch = _basePitch + randomPitcch;
        PlayClip(clip);
        yield return new WaitForSeconds(clip.length);
        PoolManager.Instance.Push(this);
    }

    private IEnumerator PlayClipCor(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        PoolManager.Instance.Push(this);
    }
}
