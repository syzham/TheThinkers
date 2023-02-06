using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Inventory
{
    public class Slot : MonoBehaviour
    {
        public bool isSlotted;
        public string itemName = "";

        private Transform _popUp;
        private Sprite _sprite;
        private Image _image;
        private TMP_Text _text;

        private void Start()
        {
            _popUp = gameObject.transform.Find("HoverPanel");
            _sprite = gameObject.transform.Find("ImageHolder").GetComponent<Image>().sprite;
            _image = _popUp.Find("ImageHolder").GetComponent<Image>();
            _text = _popUp.Find("NameHolder").GetComponent<TMP_Text>();
        }

        public void Expand()
        {
            if (!isSlotted) return;
            _popUp.gameObject.SetActive(true);
            _image.enabled = true;
            _image.sprite = _sprite;
            _text.text = itemName;
        }

        public void Compress()
        {
            _popUp.gameObject.SetActive(false);
        }
        
    }
}
