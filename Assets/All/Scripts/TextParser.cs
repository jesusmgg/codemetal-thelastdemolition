using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Parseado
{
    public Parseado(string[] parsed, List<Sprite> sprites, List<AudioClip> audios)
    {
        dialogo = parsed[0];
        /*foreach (Sprite sprite in sprites) {
            if (sprite.name == parsed [1]) {
                avatar = sprite;
            } else {
                avatar = null;
            }
        }*/

        avatar = sprites[int.Parse(parsed[1])];

        audio = audios[int.Parse(parsed[2])];
    }

    public string dialogo;
    public Sprite avatar;
    public AudioClip audio;
}

public class TextParser : MonoBehaviour
{
    private static Dialogues dialogos;
    public List<Sprite> listaAvatares = new List<Sprite>();
    public List<AudioClip> listaAudio = new List<AudioClip>();
    public float secondsBetweenCharacters = 0.02f;
    public Text cajaTexto;
    public Image cajaAvatar;
    public GameObject dialogueGui;
    private int temp;
    public GameObject receptor;
    private bool prohibir = false;

    /*public static Parseado parsing;*/

    // Use this for initialization
    void Start()
    {
        //cajaTexto = GameObject.Find ("Receptor").GetComponent<Text> ();
        //cajaAvatar = GameObject.Find ("Avatar").GetComponent<Image> ();
        cajaTexto.text = "";

        dialogos = GameObject.Find("Dialogues").GetComponent<Dialogues>();
        dialogos.SetTree("Principal");
    }

    public Parseado Parsear()
    {
        string[] parsed = dialogos.GetCurrentDialogue().Split('|');
        return new Parseado(parsed, listaAvatares, listaAudio);
    }

    public void Pasar(int duracion)
    {
        StartCoroutine(Esperar(duracion));
    }

    IEnumerator Esperar(int duracion)
    {
        foreach (var x in dialogueGui.GetComponentsInChildren<Image>())
        {
            x.enabled = true;
        }
        foreach (var x in dialogueGui.GetComponentsInChildren<Text>())
        {
            x.enabled = true;
        }

        cajaTexto.text = "";
        cajaAvatar.sprite = Parsear().avatar;
        GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(Parsear().audio);
        StartCoroutine(DisplayString(Parsear().dialogo));

        while (duracion > temp)
        {
            if (Input.GetButtonDown("A_1") && !prohibir)
            {
                prohibir = true;

                cajaTexto.text = "";
                cajaAvatar.sprite = Parsear().avatar;
                GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(Parsear().audio);
                StartCoroutine(DisplayString(Parsear().dialogo));
            }

            yield return null;
        }

        temp = 0;
        foreach (var x in dialogueGui.GetComponentsInChildren<Image>())
        {
            x.enabled = false;
        }
        foreach (var x in dialogueGui.GetComponentsInChildren<Text>())
        {
            x.enabled = false;
        }
    }

    IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;
        int currentCharacterIndex = 0;
        receptor.GetComponent<Text>().text = "";
        while (currentCharacterIndex < stringLength)
        {
            receptor.GetComponent<Text>().text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
                yield return new WaitForSeconds(secondsBetweenCharacters);
            }
            else
            {
                break;
            }
        }

        dialogos.Next();
        temp++;
        prohibir = false;
    }
}