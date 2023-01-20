using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    List<string> textik = new List<string>();
    private int kolvo;
    [SerializeField] private Text text;
    void Start()
    {
        kolvo = 0;
        textik.Add("Нажатий по монитору: " + kolvo + "\n");
        text.text = textik[kolvo];
    }


 void OnMouseDown()
    {
        kolvo++;
        textik.Add("Нажатий по монитору: " + kolvo + "\n");
        text.text += textik[kolvo];


    }
    void Update()
    {
        
    }
}
