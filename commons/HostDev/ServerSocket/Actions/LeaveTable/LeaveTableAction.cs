using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;
using System;

namespace ServerSocket.Actions.LeaveTable
{
    class LeaveTableAction : BaseAction {
        public LeaveTableAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            //TODO trzeba dopisać rozłączanie z serwera.
            throw new NotImplementedException();
        }
    }
}
