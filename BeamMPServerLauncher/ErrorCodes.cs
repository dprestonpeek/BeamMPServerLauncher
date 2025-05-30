﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeamMPServerLauncher
{
    public enum ErrorCode { Unknown, PassMessage, ServerNotFound, MapNotFound, PreviewsNotFound, BeamMPServerNotFound, AreYouSureDelete }
    public struct Error
    {
        public string title = "";
        public string message = "";
        public string ok = "";
        public string alt = "";
        public OkFunction okFunc = OkFunction.None;
        public AltFunction altFunc = AltFunction.None;
        
        public Error()
        {
        }

    }

    internal class ErrorCodes
    {
        public static Error GetErrorInfo(ErrorCode code, string extraMsg)
        {
            Error error = new Error();

            switch(code)
            {
                case ErrorCode.ServerNotFound:
                    error.title = "Error - ServerConfig.toml not found";
                    error.message = "Couldn't find ServerConfig.toml. Please copy it to this directory and open the Server Launcher again. " + extraMsg;
                    error.ok = "Ok, Close";
                    error.alt = "Open Directory";
                    error.okFunc = OkFunction.CloseApp;
                    error.altFunc = AltFunction.OpenDirectory;
                    break;
                case ErrorCode.MapNotFound:
                    error.title = "Error - No map files found";
                    error.message = "There are no maps in the 'maps' folder. Please copy a map zip file to this directory and start the server again. " + extraMsg;
                    error.ok = "Ok";
                    error.alt = "Open Directory";
                    error.okFunc = OkFunction.CloseErrorWindow;
                    error.altFunc = AltFunction.OpenDirectory;
                    break;
                case ErrorCode.PreviewsNotFound:
                    error.title = "Error - No preview image files found";
                    error.message = "There was an error locating preview image files for the selected map. " + extraMsg;
                    error.ok = "Ok";
                    error.alt = "Open Directory";
                    error.okFunc = OkFunction.CloseErrorWindow;
                    error.altFunc = AltFunction.OpenDirectory;
                    break;
                case ErrorCode.BeamMPServerNotFound:
                    error.title = "Error - BeamMP-Server.exe not found";
                    error.message = "Could not locate BeamMP-Server.exe in this directory. Please copy it to this location and try again.";
                    error.ok = "Ok";
                    error.alt = "Open Directory";
                    error.okFunc = OkFunction.CloseErrorWindow;
                    error.altFunc = AltFunction.OpenDirectory;
                    break;
                case ErrorCode.PassMessage:
                    error.title = "Error";
                    error.message = extraMsg + " ";
                    error.ok = "Ok";
                    error.alt = "";
                    error.okFunc = OkFunction.CloseErrorWindow;
                    error.altFunc = AltFunction.None;
                    break;
                case ErrorCode.Unknown:
                    error.title = "Error - Unknown Error";
                    error.message = "An unknown error occurred. See log for more info. " + extraMsg;
                    error.ok = "Ok";
                    error.alt = "Open Log File";
                    error.okFunc = OkFunction.CloseErrorWindow;
                    error.altFunc = AltFunction.OpenLog;
                    break;
                case ErrorCode.AreYouSureDelete:
                    error.title = "Are you sure?";
                    error.message = "Are you sure you want to permanently delete this map? You will not be able to recover it from the Recycle Bin.";
                    error.ok = "Yes";
                    error.alt = "Cancel";
                    error.okFunc = OkFunction.YesDeleteMap;
                    error.altFunc = AltFunction.CloseErrorWindow;
                    break;
            }

            return error;
        }
    }
}
