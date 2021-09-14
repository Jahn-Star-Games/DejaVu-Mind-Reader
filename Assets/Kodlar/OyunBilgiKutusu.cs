using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OyunBilgiKutusu : MonoBehaviour
{
    [Header("Bilgi Kutusu Mod")]
    public Vector2 minMax_poz;
    public float animasyonSuresi;
    public float gecenSure, sure_normalized;

    internal bool hesapMakinesi = false, modDegistir;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (modDegistir && hesapMakinesi)
        {
            gecenSure += Time.deltaTime;
            sure_normalized = gecenSure / animasyonSuresi;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, minMax_poz.x), sure_normalized);
            if (rectTransform.anchoredPosition.y - minMax_poz.x < 0.25f) modDegistir = false;
        }
        else if (modDegistir)
        {
            gecenSure += Time.deltaTime;
            sure_normalized = gecenSure / animasyonSuresi;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, minMax_poz.y), sure_normalized);
            if (minMax_poz.y - rectTransform.anchoredPosition.y < 0.25f) modDegistir = false;
        }
    }
    public void BilgiKutusuMod()
    {
        hesapMakinesi = !hesapMakinesi;
        modDegistir = true;
        gecenSure = 0;
    }
    public float islemYap, sonIslemSonuc;
    public string cikti;
    public Text denklemMetin, denklemSonucMetin;
    public void TusaBas(string tus)
    {
        float rakam;
        if (float.TryParse(tus, out rakam))
        {
            if (islemYap == 1) islemYap = 2;
            cikti += rakam + "";
        }
        else if (tus != ".")
        {
            string sonKrktr = cikti.Length > 0 ? cikti[cikti.Length - 1] + "" : "";
            if (tus == "C")
            {
                cikti = "";
                islemYap = 0;
                denklemSonucMetin.text = "";
            }
            else if (sonKrktr == "+" || sonKrktr == "-" || sonKrktr == "*" || sonKrktr == "/")
            {
                cikti = cikti.Remove(cikti.Length - 1);
                islemYap = 0;
            }
            if (cikti.Length > 0 && sonKrktr != tus && islemYap != 2)
            {
                cikti += tus;
                islemYap = 1;
            }
            else if (islemYap == 2)
            {
                cikti = sonIslemSonuc + tus;
                islemYap = 1;
            }
        }
        else if (cikti.Length > 0 && cikti[cikti.Length - 1] != '.') cikti += ".";
        if (islemYap == 2) 
        {
            try
            {
                if (cikti.Contains("+")) sonIslemSonuc = float.Parse(cikti.Split('+')[0]) + float.Parse(cikti.Split('+')[1]);
                else if (cikti.Contains("-")) sonIslemSonuc = float.Parse(cikti.Split('-')[0]) - float.Parse(cikti.Split('-')[1]);
                else if (cikti.Contains("*")) sonIslemSonuc = float.Parse(cikti.Split('*')[0]) * float.Parse(cikti.Split('*')[1]);
                else if (cikti.Contains("/")) sonIslemSonuc = float.Parse(cikti.Split('/')[0]) / float.Parse(cikti.Split('/')[1]);
            }
            catch (System.FormatException) { cikti = cikti.Remove(cikti.Length - 1); }
            catch { }
            denklemSonucMetin.text = "= " + (int)sonIslemSonuc; 
        }
        else if (islemYap == 0 && cikti.Split('.').Length > 2) cikti = cikti.Remove(cikti.Length - 1);
        denklemMetin.text = cikti;
    }
}
