using MiniFarm.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm.UI
{
    public class ItemCell : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text count;

        private Image _backgroundImage;

        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite pressedSprite;
        
        private void Start()
        {
            _backgroundImage = GetComponent<Image>();
        }

        // private void OnDestroy()
        // {
        //     GetComponent<Button>().onClick.RemoveAllListeners();
        // }

        public void SetItem(ItemInstance item)
        {
            if (item.item == null || item.count <= 0)
            {
                image.sprite = null;
                count.text = "";
            }
            else
            {
                image.sprite = item.item.sprite;
                if (item.count > 1)
                {
                    count.text = item.count.ToString();
                }
                else
                {
                    count.text = "";
                }
            }
        }

        public void MarkPressed()
        {
            _backgroundImage.sprite = pressedSprite;
        }
        
        public void UnmarkPressed()
        {
            _backgroundImage.sprite = normalSprite;
        }
    }
}