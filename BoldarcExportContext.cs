using Autodesk.Revit.DB;
using Autodesk.Revit.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BoldarcRevitPlugin
{
    class BoldarcExportContext : IExportContext
    {
        private FbxExporter m_Exporter;
        public BoldarcExportContext(Document inDocument) 
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            m_Exporter = new FbxExporter();
        }
        public void OnPolymesh(PolymeshTopology inMesh)
        {
            
            //Assembly.LoadFrom(asmLocation);
            m_Exporter.AddGeometry(inMesh.GetPoints(), inMesh.GetFacets());
            
        }
        public bool Start() { return true; }
        public void Finish() { m_Exporter.ExportFbx(); }
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

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.ToUpper().StartsWith("BOLDARCMANAGED"))
            {
                string asmLocation = Assembly.GetExecutingAssembly().Location;
                asmLocation = asmLocation.Substring(0, asmLocation.LastIndexOf('\\') + 1);
                string asmName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
                string filename = Path.Combine(asmLocation, asmName);

                //if (File.Exists(filename)) 
                return Assembly.LoadFrom(filename);
            }
            else
                return Assembly.GetExecutingAssembly();
        }
    }
}
