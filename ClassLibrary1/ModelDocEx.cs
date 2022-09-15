using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primitives
{
    public static class ModelDocEx
    {
        public static void CreateBox(this IModelDoc2 model,double width, double height, double length)
        {
            CreateExtrusion(model,skMgr => skMgr.CreateCenterRectangle(0, 0, 0, width / 2, height / 2, 0) != null, height);
        }
        public static void CreateCylinder(this IModelDoc2 model,double diam, double height)
        {
            CreateExtrusion(model,skMgr => skMgr.CreateCircleByRadius(0, 0, 0, diam / 2) != null, height);
        }

        private static void CreateExtrusion(IModelDoc2 model,Func<ISketchManager, bool> creator, double height)
        {
            model.IActiveView.EnableGraphicsUpdate = false;
            model.FeatureManager.EnableFeatureTree = false;

            
            try
            {
                var sketch = CreateSketch(model, creator);

                if ((sketch as IFeature).Select2(false, 0))
                {
                    var feat = model.FeatureManager.FeatureExtrusion3(true, false, false,
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
            finally
            {
                model.IActiveView.EnableGraphicsUpdate = true;
                model.FeatureManager.EnableFeatureTree = true;

            }


        }

        private static ISketch CreateSketch(IModelDoc2 model,Func<ISketchManager, bool> creator)
        {

            //var model = App.IActiveDoc2;

            var skMgr = model.SketchManager;

            skMgr.InsertSketch(true);
            skMgr.AddToDB = true;

            var sketch = skMgr.ActiveSketch;

            //    var arc = skMgr.CreateCircleByRadius(0, 0, 0, radius);
            if (!creator.Invoke(skMgr))
            {
                throw new NullReferenceException("Failed to create skecth segment");
            }


            // if (arc == null)
            // {
            //     throw new NullReferenceException("Failed to create skecth segment");
            //  }

            skMgr.AddToDB = false;
            skMgr.InsertSketch(true);

            return sketch;


        }
    }
}
