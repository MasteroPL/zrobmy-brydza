using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameManagerLib.Models;
using EasyHosting.Models.Client;
using EasyHosting.Models.Actions;

using LeavePlaceActionRequestSerializer = ServerSocket.Actions.LeavePlace.RequestSerializer;
using LeavePlaceActionResponseSerializer = ServerSocket.Actions.LeavePlace.ResponseSerializer;
using SitPlayerOutRequestSerializer = ServerSocket.Actions.SitPlayerOut.RequestSerializer;
using SitPlayerOutResponseSerializer = ServerSocket.Actions.SitPlayerOut.ResponseSerializer;

namespace Assets.Code.UI
{
    class CustomButtonPositionSitOutScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Button ReferencedButton;
        [SerializeField] GameManagerScript GameManager;
        [SerializeField] SeatsManagerScript SeatManager;

        private void OnLeavePlaceRequestCallback(Request request, ActionsSerializer response, object additionalData) {
            var position = (PlayerTag)additionalData;

            var data = new LeavePlaceActionResponseSerializer(response.Actions[0].ActionData);
            data.Validate();

            if (data.Status == "OK") {
                if (SeatManager.IsSeatTaken(position)) {
                    SeatManager.SitOutPlayer(position);
                }
            }
        }
        private void OnSitPlayerOutRequestCallback(Request request, ActionsSerializer response, object additionalData) {
            var position = (PlayerTag)additionalData;

            var data = new SitPlayerOutResponseSerializer(response.Actions[0].ActionData);
            data.Validate();

            if(data.Status == "OK") {
                if (SeatManager.IsSeatTaken(position)) {
                    SeatManager.SitOutPlayer(position);
                }
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            PlayerTag buttonID = CastCharForPlayerTag(ReferencedButton.gameObject.name[0]);
            bool available = SeatManager.CheckSeatAvailability(buttonID);
            if (!available)
            {
                if (UserData.IsAdmin ||
                    (UserData.Position.ToString()[0] == ReferencedButton.gameObject.name[0]) && UserData.Sitting)
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
                if((UserData.Sitting && UserData.Position.ToString()[0] == ReferencedButton.gameObject.name[0]) || UserData.IsAdmin){

                    if (UserData.Sitting && UserData.Position.ToString()[0] == ReferencedButton.gameObject.name[0]) {
                        var requestData = new LeavePlaceActionRequestSerializer();
                        requestData.PlaceTag = (int)buttonID;
                        GameManager.PerformServerAction("leave-place", requestData.GetApiObject(), this.OnLeavePlaceRequestCallback, buttonID);
                    }
                    else {
                        var requestData = new SitPlayerOutRequestSerializer();
                        requestData.PlaceTag = (int)buttonID;
                        GameManager.PerformServerAction("sit-player-out", requestData.GetApiObject(), this.OnSitPlayerOutRequestCallback, buttonID);
                    }
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
