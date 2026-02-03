using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[System.Serializable]
public class StringList
{
    [TextArea] public List<string> list;
}

public class AchivePanelUI : MonoBehaviour
{
    [SerializeField] private List<Button> _achiveButtons = new List<Button>();

    [SerializeField] private List<GameObject> _achivePanel = new List<GameObject>();
    [SerializeField] private GameObject _achiveInfo;
    [SerializeField] private List<StringList> _infoList = new List<StringList>();

    [SerializeField] private TextMeshProUGUI _textInfo;

    [SerializeField] private Sprite _openImage;
    [SerializeField] private Sprite _closeImage;

    [SerializeField] private GameObject _menuPanel;

    private int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (YG2.saves.Achives.Count == 0)
        {
            for(int i = 0; i < 15; i++)
            {
                YG2.saves.Achives.Add(false);
            }
        }


        _achiveInfo.SetActive(false);

        ShowAchive();

        // ѕредположим, что мы хотим назначить id от 0 до количества кнопок-1
        for (int i = 0; i < _achiveButtons.Count; i++)
        {
            int id = i;
            _achiveButtons[i].onClick.AddListener(() => OpenInfo(id));
        }
    }
    public void BackToMenu()
    {
        gameObject.SetActive(false);
        _menuPanel.SetActive(true);
    }

    public void ShowAchive()
    {
        for (int i = 0; i < _achiveButtons.Count; i++)
        {
            if (YG2.saves.Achives[i] == true)
            {
                _achiveButtons[i].image.sprite = _openImage;
            }
            else
            {
                _achiveButtons[i].image.sprite = _closeImage;
            }
        }
    }

    public void OpenInfo(int id)
    {
        _achiveInfo.SetActive(true);
        switch (YG2.saves.SelectedLanguage)
        {
            case "ru":
                _textInfo.text = _infoList[id].list[0];
                break;
            case "en":
                _textInfo.text = _infoList[id].list[1];
                break;
            case "be":
                _textInfo.text = _infoList[id].list[2];
                break;
            case "de":
                _textInfo.text = _infoList[id].list[3];
                break;
        }
        if (id == 9 && YG2.saves.Achives[9] == false)// && условие секретное
        {
            _textInfo.text = "???";
        }

    }

    public void CloseInfo()
    {
        _achiveInfo.SetActive(false);
    }

    public void SwichLeft()
    {
        _achivePanel[index].SetActive(false);
        index--;
        if (index < 0)
            index = 2;
        _achivePanel[index].SetActive(true);
    }

    public void SwichRight()
    {
        _achivePanel[index].SetActive(false);
        index++;
        if (index > 2)
            index = 0;
        _achivePanel[index].SetActive(true);
    }
}
