using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;
using GameManagerLib.Models;

namespace ServerSocket.Actions.Bid
{
    public class RequestSerializer : BaseSerializer {
        [SerializerField(apiName: "height")]
        public int Height;
        [SerializerField(apiName: "color")]
        public int Color;
        // Double - kontra
        [SerializerField(apiName: "X")]
        public bool X;
        // redouble - rekontra
        [SerializerField(apiName: "XX")]
        public bool XX;

        public override void Validate(bool throwException = true) {
            base.Validate(throwException);

            if (!X && !XX) { // Nie ma kontry, ani rekontry
                if (Height < (int)ContractHeight.ONE || Height > (int)ContractHeight.SEVEN) {
                    AddError("height", "INVALID_CONTRACT_HEIGHT", "Za niski lub zawisoki kontrakt. Dozwolone wartości <-1> ∪ <1, 7>");
                }

                if (Color < (int)ContractColor.C || Color > (int)ContractColor.NT) {
                    AddError("color", "INVALID_COLOR", "Zły kolor, dozwolone wartości <0, 4>");
                }
            }
            else
            {
                if (Height != (int)ContractHeight.NONE)
                {
                    AddError("height", "INVALID_CONTRACT_HEIGHT", "Nieprawidłowa wartość wysokości kontraktu. Dla kontry/rekontry wartość musi wynosić -1");
                }
                if (Color != (int)ContractColor.NONE)
                {
                    AddError("color", "INVALID_COLOR", "Nieprawidłowa wartość koloru kontraktu. Dla kontry/rekontry wartość musi wynosić -1");
                }
            }

            if (Errors.Count > 0 && throwException) {
                ThrowException();
            }
        }
    }
}
