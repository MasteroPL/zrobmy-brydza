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

            if (X && XX)
            {
                AddError("X", "INVALID_OPERATION", "Nie można zadeklarować jednocześnie kontry i rekontry");
                AddError("XX", "INVALID_OPERATION", "Nie można zadeklarować jednocześnie kontry i rekontry");
            }

            if (!X && !XX) { // Nie ma kontry, ani rekontry
                

                if ( !( 
                        (int)ContractHeight.ONE <= Height && Height <= (int)ContractHeight.SEVEN 
                        || Height == (int)ContractHeight.NONE 
                      )
                ) {
                    AddError("Height", "INVALID_CONTRACT_HEIGHT", "Za niski lub za wysoki kontrakt. Dozwolone wartości <-1> ∪ <1, 7>");
                }

                if (!(
                        (int)ContractColor.C <= Color && Color <= (int)ContractColor.NT
                        || Color == (int)ContractColor.NONE
                     )
                )
                {
                    AddError("Color", "INVALID_COLOR", "Zły kolor, dozwolone wartości <0, 4>");
                }

                if (    Height == (int)ContractHeight.NONE && Color != (int)ContractColor.NONE 
                      ||Height != (int)ContractHeight.NONE && Color == (int)ContractColor.NONE)
                {
                    if (Height == (int)ContractHeight.NONE) AddError("Height", "INVALID_CONTRACT", "Nie można zadeklarować kontraktu bez podania wysokości");
                    else AddError("Color", "INVALID_CONTRACT", "Nie można zadeklarować kontraktu bez podania koloru");
                }
            }
            else // zagranie kontry i rekontry
            {
                if (Height != (int)ContractHeight.NONE)
                {
                    AddError("Height", "INVALID_CONTRACT_HEIGHT", "Nieprawidłowa wartość wysokości kontraktu. Dla kontry/rekontry wartość musi wynosić -1");
                }
                if (Color != (int)ContractColor.NONE)
                {
                    AddError("Color", "INVALID_COLOR", "Nieprawidłowa wartość koloru kontraktu. Dla kontry/rekontry wartość musi wynosić -1");
                }
            }

            if (Errors.Count > 0 && throwException) {
                ThrowException();
            }
        }
    }
}
