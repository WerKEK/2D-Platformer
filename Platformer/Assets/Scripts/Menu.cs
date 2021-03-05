using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{ 
    public Button[] lvls;
    public Text coinText;
    public Slider musicSlider, soundSlider;
    public Text musicText, soundText;



    void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    lvls[i].interactable = true;
                else
                    lvls[i].interactable = false;
            }
        if (!PlayerPrefs.HasKey("hp"))
            PlayerPrefs.SetInt("hp", 0);

        if (!PlayerPrefs.HasKey("bg"))
            PlayerPrefs.SetInt("bg", 0);

        if (!PlayerPrefs.HasKey("gg"))
            PlayerPrefs.SetInt("gg", 0);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetInt("MusicVolume", 3);

        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetInt("SoundVolume", 6);

        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");
    }
    void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
        PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();

        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }


    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void BuyHP(int cost)
    {
        if(PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
            GetComponent<Inventory>().Add_hp();
        }
    }
    public void BuyBG(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("bg", PlayerPrefs.GetInt("bg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
            GetComponent<Inventory>().Add_bg();
        }
    }
    public void BuyGG(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("gg", PlayerPrefs.GetInt("gg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
            GetComponent<Inventory>().Add_gg();
        }
    }
    public void Exit()
    {
            Application.Quit();    
    }
}

