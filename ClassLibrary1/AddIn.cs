using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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
