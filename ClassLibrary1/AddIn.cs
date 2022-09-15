using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Primitives
{
    [ComVisible (true),Guid("4DB8A31A-7CB0-4429-9B35-8DA258E24AAB")]
    [AutoRegister("Geometry Primitives", "Create Geometry Primitives")]
    public class AddIn : SwAddInEx
    {
        private enum Commands_e
        {
            CreateCylinder,
            CreateBox
        }

        public override bool OnConnect()
        {
            AddCommandGroup<Commands_e>(OnButtonClick);
            return true;
        }

        private void OnButtonClick(Commands_e cmd)
        {
            switch(cmd)
            {
                case Commands_e.CreateBox:
                    //ToDo: Create a Box
                    break;
                case Commands_e.CreateCylinder:
                    //Todo:  Create a Cylinder
                    break;
            }
        }
    }
}
