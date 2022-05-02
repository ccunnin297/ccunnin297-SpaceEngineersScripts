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
    public class RotorUtils
    {
        public static void RotateTo(IMyMotorAdvancedStator rotor, float targetAngle, float rotationVelocity = 1.0f)
        {
            rotor.TargetVelocityRPM = 0.0F;

            float angle = (float)(rotor.Angle * (180.0f / Math.PI));
            angle = (angle > 360 ? angle - 360 : (angle < -360 ? angle + 360 : angle));

            if (targetAngle > angle)
            {
                rotor.SetValue<float>("UpperLimit", targetAngle);
                rotor.SetValue<float>("LowerLimit", angle);
                rotor.SetValue<float>("Velocity", rotationVelocity);
            }
            else
            {
                rotor.SetValue<float>("UpperLimit", angle);
                rotor.SetValue<float>("LowerLimit", targetAngle);
                rotor.SetValue<float>("Velocity", -rotationVelocity);
            }
        }
    }
}
