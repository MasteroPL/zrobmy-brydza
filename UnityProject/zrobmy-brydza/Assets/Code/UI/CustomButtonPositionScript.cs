using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameManagerLib.Models;
using EasyHosting.Models.Client;
using SitActionRequestSerializer = ServerSocket.Actions.Sit.RequestSerializer;
using SitActionResponseSerializer = ServerSocket.Actions.Sit.ResponseSerializer;
using EasyHosting.Models.Actions;

namespace Assets.Code.UI
{
    public class CustomButtonPositionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Button ReferencedButton;
        [SerializeField] GameManagerScript GameManager;
        [SerializeField] SeatsManagerScript SeatManager;

        private void OnSitRequestCallback(Request request, ActionsSerializer response, object additionalData) {
            var position = (PlayerTag)additionalData;

            if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0) {
                return;
            }

            var data = new SitActionResponseSerializer(response.Actions[0].ActionData);
            data.Validate();
            if (SeatManager.IsSeatTaken(position)) {
                SeatManager.SitOutPlayer(position);
            }
            SeatManager.SitPlayer(position, UserData.Username, ClickedByMe: true);
        }

        public void OnPointerEnter(PointerEventData data)
        {
            PlayerTag buttonID = CastCharForPlayerTag(ReferencedButton.gameObject.name[0]);
            bool available = SeatManager.CheckSeatAvailability(buttonID);
            if (available && !UserData.Sitting)
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
            PlayerTag buttonID = CastCharForPlayerTag(ReferencedButton.gameObject.name[0]);
            bool available = SeatManager.CheckSeatAvailability(buttonID);
            if (available && !UserData.Sitting)
            {
                var requestData = new SitActionRequestSerializer();
                requestData.PlaceTag = (int)buttonID;
                GameManager.PerformServerAction("sit", requestData.GetApiObject(), this.OnSitRequestCallback, buttonID);
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
