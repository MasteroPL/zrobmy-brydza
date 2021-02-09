using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Code.UI
{
    public class CustomButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Button ReferencedButton;

        public void OnPointerEnter(PointerEventData data)
        {
            ReferencedButton.image.color = new Color32(255, 221, 0, 255);
        }

        public void OnPointerExit(PointerEventData data)
        {
            ReferencedButton.image.color = new Color32(255, 255, 255, 0);
        }
        /*public void OnPointerClick(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }*/
    }
}
