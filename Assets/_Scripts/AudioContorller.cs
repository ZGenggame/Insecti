using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioContorller : MonoBehaviour {

    public static AudioContorller _Instance;

    [System.Serializable]
    public class AudioClipKind {
        public int Index;
        public string introduce;
        public AudioClip clip;
    }
    public List<AudioClipKind> clips;
    private AudioSource m_AudioSource;

    void Awake() {
        _Instance = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioOneTimeAtPoint(int index,float volume)
    {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
            //m_AudioSource.PlayOneShot(clips[index].clip);
         AudioSource.PlayClipAtPoint(clips[index].clip, transform.position, volume);
    }

    public void PlayAudioOneTime(int index) {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
        //m_AudioSource.PlayOneShot(clips[index].clip);
        m_AudioSource.clip = clips[index].clip;
        m_AudioSource.Play();
    }

    public void PlayAudioOneTimeNotStopLast(int index,float VolumeScale)
    {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
        m_AudioSource.PlayOneShot(clips[index].clip,VolumeScale);
    }

    public void PlayAudioOneTime()
    {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
        m_AudioSource.Play();
    }

    public void PlayAudioOneTimeDelay(int index,float delayTime)
    {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
        m_AudioSource.clip = clips[index].clip;
        Invoke("PlayAudioOneTime", delayTime);
    }

    public void ToPlayTargetSound(AudioSource Targt) {
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.UISoundState) == 1) return;
        Targt.Play();
    }

    public void StopPlay() {
        m_AudioSource.Stop();
    }

}
