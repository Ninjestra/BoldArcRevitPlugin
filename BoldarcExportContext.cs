using Autodesk.Revit.DB;
using Autodesk.Revit.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BoldarcRevitPlugin
{
    class BoldarcExportContext : IExportContext
    {
        public BoldarcExportContext(Document inDocument) { }
        public void OnPolymesh(PolymeshTopology inMesh)
        {
            FbxExporter _Exporter = new FbxExporter();
            _Exporter.ExportFbx(inMesh.GetPoints(), inMesh.GetFacets());
            
        }
        public bool Start() { return true; }
        public void Finish() { }
        public bool IsCanceled() { return false; }
        public RenderNodeAction OnViewBegin(ViewNode inNode) { return 0; }
        public void OnViewEnd(ElementId inID) { }
        public RenderNodeAction OnElementBegin(ElementId inID) { return 0; }
        public void OnElementEnd(ElementId inID) { }
        public RenderNodeAction OnInstanceBegin(InstanceNode inID) { return 0; }
        public void OnInstanceEnd(InstanceNode inID) { }
        public RenderNodeAction OnLinkBegin(LinkNode inNode) { return 0; }
        public void OnLinkEnd(LinkNode inNode) { }
        public RenderNodeAction OnFaceBegin(FaceNode inNode) { return 0; }
        public void OnFaceEnd(FaceNode inNode) { }
        public void OnRPC(RPCNode inNode) { }
        public void OnLight(LightNode inNode) { }
        public void OnDaylightPortal(DaylightPortalNode inNode) { }
        public void OnMaterial(MaterialNode inNode) { }
    }
}
