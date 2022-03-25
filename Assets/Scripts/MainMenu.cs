using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        PlayerPrefs.SetInt("gridWidth", (int) GameObject.GetField("WidthField").GetText());
        PlayerPrefs.SetInt("gridHeight", (int) GameObject.GetField("HeightField").GetText());
        PlayerPrefs.SetInt("gridHardness", (int) GameObject.GetField("HardnessField").GetText());
        SceneManager.LoadScene("Game");
    }
}
