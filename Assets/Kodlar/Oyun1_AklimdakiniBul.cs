using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HeyGames.ZihinOyunlari;
public class Oyun1_AklimdakiniBul : MonoBehaviour
{
    [Header("Kaynak")]
    public Transform anaMadde;
    public GameObject madde_kaynak;
    string sonuc;
    [Header("Girdi")]
    public int sayiMiktari = 100;
    [Header("Cikti"), Tooltip("Aklından bir sayı tut, ")]
    public List<float> degerler;
    public List<string> talimatlar;
    private int mevcutTalimatId;
    public Text talimatCiktiMetin;
    public GameObject ileriButon, geriButon;
    //public List<Madde> yeni_maddeler;
    //public int madde_miktari;
    [Header("Editor")]
    public bool yenidenOlustur;
    void Start()
    {
        yenidenOlustur = true;
        oyunAlanTablo.SetActive(false);
        ileriButon.SetActive(true);
        geriButon.SetActive(false);
    }
    private void OnEnable()
    {
        yenidenOlustur = true;
        oyunAlanTablo.SetActive(false);
        ileriButon.SetActive(true);
        geriButon.SetActive(false);
        mevcutTalimatId = 0;
        oyunBilgiKutusu.TusaBas("C");
    }
    int carp, topla, bol, cikar, sifirlamaBol;
    void Update()
    {
        try
        {
            if (yenidenOlustur)
            {
                int ilkDeger, sonDeger, saglamaTest;
                do
                {
                    carp = Random.Range(1, 15);
                        carp += carp % 2 != 0 ? 1 : 0;
                    topla = Random.Range(1, 100);
                        topla += topla % 2 != 0 ? 1 : 0;
                    bol = Random.Range(1, 10);
                        bol += bol % 2 != 0 ? 1 : 0;
                    cikar = Random.Range(1, 75);
                        cikar += cikar % 2 != 0 ? 1 : 0;

                    sifirlamaBol = Random.Range(1, 15);
                    sifirlamaBol += sifirlamaBol % 2 != 0 ? 1 : 0;

                    ilkDeger = (int)TersFonksiyon(0);
                    sonDeger = (int)TersFonksiyon(sayiMiktari);
                    saglamaTest = (int)TersFonksiyon(((((sonDeger * 2 * sifirlamaBol) / ((sifirlamaBol * 2) / (carp * 2) > 1 ? (sifirlamaBol * 2) / (carp * 2) : 1)) + topla) / bol) - cikar);
                }
                while ((sifirlamaBol * 2) / (carp * 2) == 0 || ilkDeger < 0 || sonDeger > sayiMiktari || bol < 1 || saglamaTest != sonDeger);

                sonuc = "";
                degerler.Clear();

                int oncekiDeger = 0;
                bool ardisikDegil = false;
                for (int i = 0; i <= sayiMiktari; i++)
                {
                    if (oncekiDeger != (int)TersFonksiyon(i) && oncekiDeger + 1 != (int)TersFonksiyon(i)) ardisikDegil = true;
                    oncekiDeger = (int)TersFonksiyon(i);
                    degerler.Add((int)TersFonksiyon(i));
                    sonuc += i.ToString("00") + ": <color=red>" + degerler[i] + "</color> | ";
                }

                yenidenOlustur = ardisikDegil;
                if (!ardisikDegil) // Talimatlari ekle
                {
                    talimatlar.Clear();
                    talimatlar.Add("Merhaba, seninle bir oyun oynayacağız.");
                    talimatlar.Add("Sana sihirli işlemler yaptırarak aklındaki sayıyı bulacağım.");
                    //talimatlar.Add("İşlemleri aklından yada yukardaki mavi çubuğa tıklayarak hesap makinesinden yapabilirsin.");
                    talimatlar.Add("Aklından " + (ilkDeger <= 1 ? 1 : ilkDeger) + " ile " + sonDeger + " arasında bir sayı tut...");
                    talimatlar.Add("Kensi ile topla...");
                    talimatlar.Add(sifirlamaBol + " ile çarp...");
                    if ((sifirlamaBol * 2) / (carp * 2) > 1) talimatlar.Add((sifirlamaBol * 2) / (carp * 2) + " ile böl..."); // (sifirlamaBol * 2) ile bol ve (carp * 2) ile carp demek
                    talimatlar.Add(topla + " ile topla...");
                    talimatlar.Add(bol + " ile böl...");
                    talimatlar.Add(cikar + " ile çıkar...");
                    talimatlar.Add("Sonuc tam sayı değilse tam sayıya yuvarla ve aynı numaralı kapıya tıkla.");

                    talimatCiktiMetin.text = talimatlar[0];
                    MaddeOlustur();
                }
            }
        }
        catch { Application.Quit(); }
    }
    public GameObject oyunAlanTablo;
    public OyunBilgiKutusu oyunBilgiKutusu;
    public void TalimatDegistir(bool ileriGit)
    {
        if (ileriGit)
        {
            if (mevcutTalimatId + 1 < talimatlar.Count)
            {
                mevcutTalimatId++;
                talimatCiktiMetin.text = talimatlar[mevcutTalimatId];
                ileriButon.SetActive(true);
                geriButon.SetActive(true);
                if (mevcutTalimatId + 1 >= talimatlar.Count) 
                {
                    ileriButon.SetActive(false);
                    oyunBilgiKutusu.modDegistir = oyunBilgiKutusu.hesapMakinesi = true;
                    oyunAlanTablo.SetActive(true);
                }
            }
        }
        else if (mevcutTalimatId - 1 >= 0)
        {
            mevcutTalimatId--;
            talimatCiktiMetin.text = talimatlar[mevcutTalimatId];
            geriButon.SetActive(true);
            ileriButon.SetActive(true);

            if (mevcutTalimatId - 1 < 0) geriButon.SetActive(false);
        }
    }
    public void MaddeOlustur()
    {
        foreach (Transform madde in anaMadde) Destroy(madde.gameObject);
        if (degerler.Count > 0)
            for (int i = 0; i < degerler.Count; i++)
                Instantiate(madde_kaynak, anaMadde.transform).transform.GetChild(0).GetComponent<Text>().text = i.ToString("00");
    }
    public VideoPlayer_Manager arkaPlan;
    public void AklimdakiniBul(Text kapi)
    {
        int kapiNo = int.Parse(kapi.text);
        arkaPlan.SonucGoster_KapiAc((int)TersFonksiyon((float)kapiNo) + "");
    }
    private float TersFonksiyon(float kullaniciIslemSonucu) 
    {
        // kullanici fonksiyonu => ((((kullaniciSayisi * 2) * carp) + topla) / bol) - cikar;
        return ((((kullaniciIslemSonucu + cikar) * bol) - topla) / carp) / 2;
    }
}
