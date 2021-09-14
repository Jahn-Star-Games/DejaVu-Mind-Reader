using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeyGames.ZihinOyunlari
{
    public class Madde : MonoBehaviour
    {
        public Text icerik;
        void Start()
        {
            icerik = transform.GetChild(0).GetComponent<Text>();
        }

        void DegerDegistir(string id, string deger)
        {
            icerik.text = deger;
        }
    }
}
