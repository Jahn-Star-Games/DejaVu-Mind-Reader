using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_Manager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Bu Buton")]
    public GameObject normalButonMetin;
    public GameObject basiliButonMetin;
    [Header("Diger Butonlar (Event)")]
    public GameObject digerAyarlar_btn;
    public GameObject muzikMod_btn;
    private void Start()
    {
        try
        {
            normalButonMetin.SetActive(true);
            basiliButonMetin.SetActive(false);
        }
        catch { }
        digerAyarlar_btn.SetActive(false);
        muzikMod_btn.SetActive(false);
    }
    public void OnPointerDown(PointerEventData data)
    {
        try {
            normalButonMetin.SetActive(false);
            basiliButonMetin.SetActive(true);
        }
        catch { }
    }
    public void OnPointerUp(PointerEventData data)
    {
        try
        {
            normalButonMetin.SetActive(true);
            basiliButonMetin.SetActive(false);
        }
        catch { }
    }
    public void DigerButonlarMod() 
    {
        digerAyarlar_btn.SetActive(!digerAyarlar_btn.activeInHierarchy);
        muzikMod_btn.SetActive(!muzikMod_btn.activeInHierarchy);
    }
}
