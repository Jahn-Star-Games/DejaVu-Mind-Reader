﻿//Developed by Halil Emre Yildiz - @Jahn_Star
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    [Header("Background Video")]
    public VideoClip videoClip;
    public VideoClip blurVideo;
    public float endOffset = 1.25f;
    [Header("Result Video")]
    public VideoClip _videoClip;
    public float _endOffset = 1.25f;
    [Header("Music")]
    public AudioClip[] audioClips;
    [Header("GUI")]
    public Text resultTextSource;
    public Text stepsTextSource, audioTextSource;
    public GameObject[] settingPanel;
    [Header("Game 1")]
    public GameObject gameUI;
    public GameObject playButton;
    private bool showResult;
    private static Text _textAnim;
    private static float _textAnim_time, _textAnim_speed;
    private void Awake()
    {
        if (!Instance) Instance = this;
        Screen.orientation = ScreenOrientation.Portrait;
        //
        ChangeAudio();
        //
        gameUI.SetActive(false);
        resultTextSource.gameObject.SetActive(false);
        //
        SettingsDisplay("false");
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    public void Restart()
    {
        GetComponent<Image>().enabled = false;
        playButton.SetActive(true);
        gameUI.SetActive(false);
        stepsTextSource.text = "";
        Color _color = stepsTextSource.color;
        _color.a = 0;
        stepsTextSource.color = _color;
    }
    public void Play()
    {
        GetComponent<Image>().enabled = true;
        playButton.SetActive(false);
        gameUI.SetActive(true);
    }
    public void MuteAudio()
    {
        audioSource.mute = !audioSource.mute;
    }
    int prevAudio = -1;
    public void ChangeAudio()
    {
        int currentAudio;
        do { currentAudio = Random.Range(0, audioClips.Length); }
        while (prevAudio == currentAudio);
        prevAudio = currentAudio;
        audioSource.clip = audioClips[currentAudio];
        audioTextSource.text = audioSource.clip.name;
        audioSource.Play();
    }
    public void SettingsDisplay(string show_or_hide)
    {
        if (show_or_hide == "true") foreach (GameObject button in settingPanel) button.SetActive(true);
        else if (show_or_hide == "false") foreach (GameObject button in settingPanel) button.SetActive(false);
        else foreach (GameObject button in settingPanel) button.SetActive(!button.activeInHierarchy);
    }
    public void ShowResults(string yourNumber)
    {
        showResult = true;
        gameUI.SetActive(false);
        videoPlayer.clip = _videoClip;
        resultTextSource.text = yourNumber;
    }
    public static void TextAnimation(Text textSource, float speed = 1f)
    {
        if (_textAnim != textSource)
        {
            _textAnim = textSource;
            _textAnim_speed = speed;
            _textAnim_time = 0;
        }
    }
    void Update()
    {
        if (showResult)
        {
            if (videoPlayer.time >= videoPlayer.clip.length - _endOffset)
            {
                showResult = false;
                resultTextSource.gameObject.SetActive(false);
                gameUI.SetActive(true);
                videoPlayer.clip = videoClip;
                videoPlayer.Stop();
                videoPlayer.Play();
                resultTextSource.text = "";
                Restart();
            }
            else if (videoPlayer.time >= 2.6f && !resultTextSource.gameObject.activeInHierarchy)
            {
                resultTextSource.gameObject.SetActive(true);
                TextAnimation(resultTextSource, 0.4f);
            }
        }
        else if (videoPlayer.time >= videoClip.length - endOffset)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
        if (audioSource.time > audioSource.clip.length - 0.1f) ChangeAudio();
        if (_textAnim)
        {
            Color temp_color = _textAnim.color;
            temp_color.a = Mathf.Clamp(Mathf.Sin(_textAnim_time += Time.deltaTime * _textAnim_speed), 0, 1);
            _textAnim.color = temp_color;
            if (temp_color.a > 0.9f) _textAnim = null;
        }
    }
}