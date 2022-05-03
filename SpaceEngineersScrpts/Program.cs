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
    partial class Program : MyGridProgram
    {
        ShipTool shipTool;
        Log log;

        List<Updatable> updatables;

        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set RuntimeInfo.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.

            this.log = new Log(this);

            this.shipTool = new ShipTool(
                log,
                GridTerminalSystem.GetBlockWithName("Ship Tool Rotor") as IMyMotorAdvancedStator,
                GridTerminalSystem.GetBlockWithName("Ship Tool Seat Rotor") as IMyMotorAdvancedStator,
                GridTerminalSystem.GetBlockWithName("Ship Tool Grinder Hinge") as IMyMotorAdvancedStator,
                GridTerminalSystem.GetBlockWithName("Ship Tool Welder Hinge") as IMyMotorAdvancedStator,
                GridTerminalSystem.GetBlockWithName("Ship Tool Connector Hinge") as IMyMotorAdvancedStator
                );

            updatables = new List<Updatable>() {
                shipTool
            };

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.            
            if (shipTool == null)
            {
                Echo("Unable to initialize shipTool");
                return;
            }

            var args = argument.Split();

            // Interactive
            if ((updateSource & (UpdateType.Trigger | UpdateType.Terminal)) != 0)
            {
                InteractiveCommand(args);
            }

            if ((updateSource & UpdateType.Update1) != 0)
            {
                foreach (Updatable updatable in updatables)
                {
                    updatable.OnUpdate1();
                }
            }

            if ((updateSource & UpdateType.Update10) != 0)
            {
                foreach (Updatable updatable in updatables)
                {
                    updatable.OnUpdate10();
                }
            }

            if ((updateSource & UpdateType.Update100) != 0)
            {
                foreach (Updatable updatable in updatables)
                {
                    updatable.OnUpdate100();
                }
            }
        }

        private void InteractiveCommand(string[] args)
        {
            var command = args[0];
            switch (command)
            {
                case "shiptool":
                    shipTool.Command(args.Skip(1).ToArray());
                    break;
                default:
                    Echo("Unsupported command: " + command);
                    break;
            }
        }
    }
}
