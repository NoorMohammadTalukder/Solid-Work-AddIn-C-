using ClassLibrary1.Properties;
using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using CodeStack.SwEx.AddIn.Enums;
using CodeStack.SwEx.Common.Attributes;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Title("Primitives")]
        [Description("Creates geometry primitives")]
        [Icon (typeof (Resources),nameof(Resources.cylinder))]

        private enum Commands_e
        {
            [Title("Create Cylinder")]
            [Description("Creates extruded cylinder")]
            [Icon(typeof(Resources), nameof(Resources.cylinder))]
            [CommandItemInfo(true,true,swWorkspaceTypes_e.Part,true)]
            CreateCylinder,

            [Title("Create Box")]
            [Description("Creates extruded box")]
            [Icon(typeof(Resources), nameof(Resources.box))]
            [CommandItemInfo(true, true, swWorkspaceTypes_e.Part, true)]
            CreateBox
        }

        public override bool OnConnect()
        {
            AddCommandGroup<Commands_e>(OnButtonClick,OnButtonEnable);
            return true;
        }

        private void OnButtonClick(Commands_e cmd)
        {
            try
            {
                switch (cmd)
                {
                    case Commands_e.CreateBox:
                        App.IActiveDoc2.CreateBox(0.01, 0.02, 0.03);
                        //ToDo: Create a Box
                        break;
                    case Commands_e.CreateCylinder:
                        //Todo:  Create a Cylinder
                        App.IActiveDoc2.CreateCylinder(0.01, 0.01);
                        break;
                }
            }
            catch( Exception ex)
            {
                App.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
            }
        }

        private void OnButtonEnable(Commands_e cmd, ref CommandItemEnableState_e state)
        {
            switch (cmd)
            {
                case Commands_e.CreateBox:
                   
                    break;

                case Commands_e.CreateCylinder:
                    var model = App.IActiveDoc2;

                    state = CommandItemEnableState_e.DeselectDisable;

                    if (model is PartDoc)
                    {
                        var selType = (swSelectType_e)model.ISelectionManager.GetSelectedObjectType3(1, -1);

                        if (selType == swSelectType_e.swSelDATUMPLANES)
                        {
                            state = CommandItemEnableState_e.DeselectEnable;
                        }
                        else if (selType == swSelectType_e.swSelFACES)
                        {
                            var face = model.ISelectionManager.GetSelectedObject6(1, -1) as IFace2;

                            if (face.IGetSurface().IsPlane())
                            {
                                state = CommandItemEnableState_e.DeselectEnable;
                            }
                        }
                    }
                    else
                    {
                       
                    }
                    break;
            }
        }

       
    }
}
