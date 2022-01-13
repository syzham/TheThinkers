using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Inventory
{
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isSlotted = false;
        public string itemName = "";
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isSlotted) return;
            var popUp = gameObject.transform.Find("HoverPanel");
            var image = gameObject.transform.Find("ImageHolder").GetComponent<Image>().sprite;
            popUp.gameObject.SetActive(true);
            popUp.Find("ImageHolder").GetComponent<Image>().enabled = true;
            popUp.Find("ImageHolder").GetComponent<Image>().sprite = image;
            popUp.Find("NameHolder").GetComponent<TMP_Text>().text = itemName;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.Find("HoverPanel").gameObject.SetActive(false);
        }
        
    }
}
