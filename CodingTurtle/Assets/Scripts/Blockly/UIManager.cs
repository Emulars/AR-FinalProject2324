using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject startBlock;
    [SerializeField] private GameObject sideBar;
    [SerializeField] private GameObject workTable;
    [SerializeField] private GameObject infoText;


    [Header("Sprite")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;


    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // If the start block is active, then deactivate all UI elements
        if (startBlock.activeSelf)
        {
            startBlock.SetActive(false);
            sideBar.SetActive(false);
            workTable.SetActive(false);
            infoText.SetActive(false);
            gameObject.GetComponent<Image>().sprite = onSprite;
        }
        // If the start block is inactive, then activate all UI elements
        else
        {
            startBlock.SetActive(true);
            sideBar.SetActive(true);
            workTable.SetActive(true);
            //infoText.SetActive(true);
            gameObject.GetComponent<Image>().sprite = offSprite;
        }
    }

}
