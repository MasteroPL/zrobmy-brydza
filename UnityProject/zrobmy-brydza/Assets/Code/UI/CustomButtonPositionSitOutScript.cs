﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameManagerLib.Models;

namespace Assets.Code.UI
{
    class CustomButtonPositionSitOutScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Button ReferencedButton;
        [SerializeField] GameManagerScript GameManager;
        [SerializeField] SeatsManagerScript SeatManager;

        public void OnPointerEnter(PointerEventData data)
        {
            PlayerTag buttonID = CastCharForPlayerTag(ReferencedButton.gameObject.name[0]);
            bool available = SeatManager.CheckSeatAvailability(buttonID);
            if (!available)
            {
                if (UserData.IsAdmin ||
                    (UserData.position.ToString()[0] == ReferencedButton.gameObject.name[0]) && UserData.Sitting)
                {
                    ReferencedButton.image.color = new Color32(255, 221, 0, 255);
                }
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            ReferencedButton.image.color = new Color32(255, 255, 255, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayerTag buttonID = CastCharForPlayerTag(ReferencedButton.gameObject.name[0]);
            bool available = SeatManager.CheckSeatAvailability(buttonID);
            if (!available)
            {
                if (UserData.IsAdmin || 
                    (UserData.position.ToString()[0] == ReferencedButton.gameObject.name[0]) && UserData.Sitting)
                {
                    SeatManager.SitOutPlayer(buttonID);
                }
            }
        }

        private PlayerTag CastCharForPlayerTag(char Char)
        {
            switch (Char)
            {
                case 'N':
                    return PlayerTag.N;
                case 'E':
                    return PlayerTag.E;
                case 'S':
                    return PlayerTag.S;
                case 'W':
                    return PlayerTag.W;
                default:
                    return PlayerTag.NOBODY;
            }
        }
    }
}
