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
    public class ShipTool : Updatable
    {
        private Log log;

        private IMyMotorAdvancedStator shipToolRotor;
        private IMyMotorAdvancedStator shipToolSeatRotor;
        private IMyMotorAdvancedStator shipToolGrinderHinge;
        private IMyMotorAdvancedStator shipToolWelderHinge;
        private IMyMotorAdvancedStator shipToolConnectorHinge;

        public ShipTool(
            Log log,
            IMyMotorAdvancedStator shipToolRotor,
            IMyMotorAdvancedStator shipToolSeatRotor,
            IMyMotorAdvancedStator shipToolGrinderHinge,
            IMyMotorAdvancedStator shipToolWelderHinge,
            IMyMotorAdvancedStator shipToolConnectorHinge
            )
        {
            this.log = log;

            if (shipToolRotor == null)
            {
                log.Error("ShipTool>shipToolRotor null");
                return;
            }
            this.shipToolRotor = shipToolRotor;

            if (shipToolSeatRotor == null)
            {
                log.Error("ShipTool>shipToolSeatRotor null");
                return;
            }
            this.shipToolSeatRotor = shipToolSeatRotor;

            if (shipToolGrinderHinge == null)
            {
                log.Error("ShipTool>shipToolGrinderHinge null");
                return;
            }
            this.shipToolGrinderHinge = shipToolGrinderHinge;

            if (shipToolWelderHinge == null)
            {
                log.Error("ShipTool>shipToolWelderHinge null");
                return;
            }
            this.shipToolWelderHinge = shipToolWelderHinge;

            if (shipToolConnectorHinge == null)
            {
                log.Error("ShipTool>shipToolConnectorHinge null");
                return;
            }
            this.shipToolConnectorHinge = shipToolConnectorHinge;

            shipToolGrinderHinge.SetValue<float>("UpperLimit", 90);
            shipToolGrinderHinge.SetValue<float>("LowerLimit", 0);
            shipToolWelderHinge.SetValue<float>("UpperLimit", 90);
            shipToolWelderHinge.SetValue<float>("LowerLimit", 0);
            shipToolConnectorHinge.SetValue<float>("UpperLimit", 90);
            shipToolConnectorHinge.SetValue<float>("LowerLimit", 0);
        }

        public void Command(string[] args)
        {
            var command = args[0];
            switch (command)
            {
                case "rotate":
                    if (args.Length < 2)
                    {
                        log.Error("Missing shiptool rotate arg");
                        return;
                    }
                    ShipToolRotate(args[1]);
                    break;
                default:
                    log.Error("Unsupported shiptool command: " + command);
                    break;
            }
        }

        private void ShipToolRotate(string arg)
        {
            float rotorVelocity = shipToolRotor.GetValue<float>("Velocity");
            float seatRotorVelocity = shipToolSeatRotor.GetValue<float>("Velocity");
            float grinderHingeAbsVelocity = Math.Abs(shipToolGrinderHinge.GetValue<float>("Velocity"));
            float welderHingeAbsVelocity = Math.Abs(shipToolWelderHinge.GetValue<float>("Velocity"));
            float connectorHingeAbsVelocity = Math.Abs(shipToolConnectorHinge.GetValue<float>("Velocity"));
            switch (arg)
            {
                case "grinder":
                    shipToolRotor.RotorLock = false;
                    shipToolSeatRotor.RotorLock = false;
                    RotorUtils.RotateTo(shipToolRotor, 90, rotorVelocity);
                    RotorUtils.RotateTo(shipToolSeatRotor, 270, seatRotorVelocity);
                    shipToolGrinderHinge.SetValue<float>("Velocity", -grinderHingeAbsVelocity);
                    shipToolWelderHinge.SetValue<float>("Velocity", welderHingeAbsVelocity);
                    shipToolConnectorHinge.SetValue<float>("Velocity", connectorHingeAbsVelocity);
                    break;
                case "welder":
                    shipToolRotor.RotorLock = false;
                    shipToolSeatRotor.RotorLock = false;
                    RotorUtils.RotateTo(shipToolRotor, 270, rotorVelocity);
                    RotorUtils.RotateTo(shipToolSeatRotor, 90, seatRotorVelocity);
                    shipToolGrinderHinge.SetValue<float>("Velocity", grinderHingeAbsVelocity);
                    shipToolWelderHinge.SetValue<float>("Velocity", -welderHingeAbsVelocity);
                    shipToolConnectorHinge.SetValue<float>("Velocity", connectorHingeAbsVelocity);
                    break;
                case "connector":
                    shipToolRotor.RotorLock = false;
                    shipToolSeatRotor.RotorLock = false;
                    RotorUtils.RotateTo(shipToolRotor, 180, rotorVelocity);
                    RotorUtils.RotateTo(shipToolSeatRotor, 180, seatRotorVelocity);
                    shipToolGrinderHinge.SetValue<float>("Velocity", grinderHingeAbsVelocity);
                    shipToolWelderHinge.SetValue<float>("Velocity", welderHingeAbsVelocity);
                    shipToolConnectorHinge.SetValue<float>("Velocity", -connectorHingeAbsVelocity);
                    break;
                default:
                    log.Error("Unsupported shiptool rotate arg: " + arg + ", expected [grinder, welder, connector]");
                    break;
            }
        }

        public void OnUpdate1()
        {
            CheckShipToolRotorLock();
        }

        public void OnUpdate10()
        {
            CheckShipToolRotorLock();
        }

        public void OnUpdate100()
        {
            CheckShipToolRotorLock();
        }

        private void CheckShipToolRotorLock()
        {
            if (!shipToolRotor.RotorLock)
            {
                float angle = (float)(shipToolRotor.Angle * (180.0f / Math.PI));
                string valueToGet = shipToolRotor.GetValue<float>("Velocity") > 0 ? "UpperLimit" : "LowerLimit";
                if (angle == shipToolRotor.GetValue<float>(valueToGet))
                {
                    shipToolRotor.RotorLock = true;
                }
            }

            if (!shipToolSeatRotor.RotorLock)
            {
                float angle = (float)(shipToolSeatRotor.Angle * (180.0f / Math.PI));
                string valueToGet = shipToolSeatRotor.GetValue<float>("Velocity") > 0 ? "UpperLimit" : "LowerLimit";
                if (angle == shipToolSeatRotor.GetValue<float>(valueToGet))
                {
                    shipToolSeatRotor.RotorLock = true;
                }
            }
        }
    }
}
