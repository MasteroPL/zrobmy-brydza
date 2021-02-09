using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Code.UI
{
    public class CustomButtonPositionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Button ReferencedButton;
        [SerializeField] GameManagerScript GameManager;

        public void OnPointerEnter(PointerEventData data)
        {
            bool available = GameManager.CheckSeatAvailability(ReferencedButton.gameObject.name[0]);
            if (available)
            {
                ReferencedButton.image.color = new Color32(255, 221, 0, 255);
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            ReferencedButton.image.color = new Color32(255, 255, 255, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            bool available = GameManager.CheckSeatAvailability(ReferencedButton.gameObject.name[0]);
            if (available)
            {
                GameManager.SitPlayer(ReferencedButton.gameObject.name[0], "nickname");
            }
        }
    }
}
