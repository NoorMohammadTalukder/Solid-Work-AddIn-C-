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
                        //ToDo: Create a Box
                        break;
                    case Commands_e.CreateCylinder:
                        //Todo:  Create a Cylinder
                        CreateCylinder(0.01, 0.01);
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

        private void CreateCylinder (double diam, double height)
        {
            var sketch = CreateSketch(diam / 2);
            
            if((sketch as IFeature).Select2(false, 0))
            {
                var feat= App.IActiveDoc2.FeatureManager.FeatureExtrusion3(true, false, false,
                            (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                            height, 0, false, false, false, false, 0, 0,
                            false, false, false, false, false, false, false,
                            (int)swStartConditions_e.swStartSketchPlane, 0, false);
               
                if (feat == null)
                {
                    throw new NullReferenceException("Failed to create extrusion");
                }
            }
            else
            {
                throw new Exception("Failed to create base sketch");
            }

            
        }

        private ISketch CreateSketch( double radius)
        {
         


            var model = App.IActiveDoc2;

            var skMgr = model.SketchManager;

            skMgr.InsertSketch(true);
            skMgr.AddToDB = true;

            var sketch = skMgr.ActiveSketch;

            var arc = skMgr.CreateCircleByRadius(0, 0, 0, radius);

            if (arc == null)
            {
                throw new NullReferenceException("Failed to create skecth segment");
            }

            skMgr.AddToDB = false;
            skMgr.InsertSketch(true);

            return sketch;


        }
    }
}
