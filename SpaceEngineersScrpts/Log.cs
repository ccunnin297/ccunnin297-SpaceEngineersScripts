using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    public class Log
    {
        private enum LogLevel
        {
            Error = 0,
            Info = 1,
            Debug = 2
        }
        LogLevel logLevel = LogLevel.Debug;

        MyGridProgram program;

        public Log(MyGridProgram program)
        {
            this.program = program;
        }

        public void Debug(string message)
        {
            if (logLevel < LogLevel.Debug) { return; }
            program.Echo("D | " + message);
        }

        public void Info(string message)
        {
            if (logLevel < LogLevel.Info) { return; }
            program.Echo("I | " + message);
        }

        public void Error(string message)
        {
            if (logLevel < LogLevel.Error) { return; }
            program.Echo("E | " + message);
        }
    }
}
