using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer)), RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Image))]
public class VideoPlayer_Manager : MonoBehaviour
{
    public VideoClip videoKlibi, kapiVideoKlip;
    public Text kapiSonucSayiMetin;
    public AudioClip sesKlibi;
    VideoPlayer videoOynatici;
    AudioSource sesOynatici;
    public float snOnceYenile = 1;
    [Header("Butonlar")]
    public GameObject oyna_btn;
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        videoOynatici = gameObject.GetComponent<VideoPlayer>() ? gameObject.GetComponent<VideoPlayer>() : gameObject.AddComponent<VideoPlayer>();
        videoOynatici.renderMode = VideoRenderMode.CameraFarPlane;
        videoOynatici.targetCamera = Camera.main;
        videoOynatici.aspectRatio = VideoAspectRatio.FitOutside;
        videoOynatici.playOnAwake = videoOynatici.isLooping = true;
        videoOynatici.waitForFirstFrame = videoOynatici.skipOnDrop = false;
        videoOynatici.audioOutputMode = VideoAudioOutputMode.None;
        videoOynatici.clip = videoKlibi ? videoKlibi : null;
        videoOynatici.Play();

        sesOynatici = gameObject.GetComponent<AudioSource>() ? gameObject.GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
        sesOynatici.clip = sesKlibi ? sesKlibi : null;
        sesOynatici.playOnAwake = sesOynatici.loop = true;
        sesOynatici.Play();

        oyun1.SetActive(false);
        kapiSonucSayiMetin.gameObject.SetActive(false);
    }

    public void SesMod() 
    {
        sesOynatici.mute = !sesOynatici.mute;
    }
    public void GeriGit()
    {
        oyna_btn.SetActive(true);
        GetComponent<Image>().enabled = false;
        oyun1.SetActive(false);
    }
    public void OyunListesi()
    {
        GetComponent<Image>().enabled = true;
        oyna_btn.SetActive(false);
        oyun1.SetActive(true);
    }
    void Update()
    {
        if (kapiKlipOynat)
        {
            if (videoOynatici.time >= kapiVideoKlip.length) 
            {
                kapiKlipOynat = false;
                kapiSonucSayiMetin.gameObject.SetActive(false);
                oyun1.SetActive(true);
                videoOynatici.clip = videoKlibi;
                videoOynatici.Stop();
                videoOynatici.Play();
                kapiSonucSayiMetin.text = "";
            }
            else if (videoOynatici.time >= 2.25f) kapiSonucSayiMetin.gameObject.SetActive(true);
        }
        else if (videoOynatici.time >= videoKlibi.length - snOnceYenile)
        {
            videoOynatici.Stop();
            videoOynatici.Play();
        }
    }
    public GameObject oyun1;
    bool kapiKlipOynat;
    public void SonucGoster_KapiAc(string akildakiSayi)
    {
        kapiKlipOynat = true;
        oyun1.SetActive(false);
        videoOynatici.clip = kapiVideoKlip;
        kapiSonucSayiMetin.text = akildakiSayi;
    }
}
