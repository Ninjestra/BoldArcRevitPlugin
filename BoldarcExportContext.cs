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
        private Stack<Transform> m_stackTransform;
        private Stack<ElementId> m_stackElementId = new Stack<ElementId>();
        private Document m_Document;

        ElementId CurrentElementId
        {
            get
            {
                return (m_stackElementId.Count > 0)
                  ? m_stackElementId.Peek()
                  : ElementId.InvalidElementId;
            }
        }

        Element CurrentElement
        {
            get
            {
                return m_Document.GetElement(
                  CurrentElementId);
            }
        }

        public BoldarcExportContext(Document inDocument) 
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            m_Document = inDocument;
            m_Exporter = new FbxExporter();
            m_stackTransform = new Stack<Transform>();
            m_stackTransform.Push(Transform.Identity);
        }
        public void OnPolymesh(PolymeshTopology inMesh)
        {
            switch (inMesh.DistributionOfNormals)
            {
                case DistributionOfNormals.AtEachPoint:
                    break;

                case DistributionOfNormals.OnEachFacet:
                    break;

                case DistributionOfNormals.OnePerFace:
                    break;

                default:
                    break;
            }
            m_Exporter.AddGeometry(inMesh.GetPoints(), inMesh.GetFacets(), inMesh.GetNormals(), m_stackTransform.Peek());

        }
        public bool Start() 
        {

            return true; 
        }
        public void Finish() 
        {
            m_Exporter.ExportFbx();
            //Process.Start(@"D:\Program Files\Blender Foundation\Blender\blender.exe", @"-b --python D:\Joel\Scripts\python\Blender\RevitUnrealBridge.py -- D:\Joel\FBX\exporter_test.fbx");
        }
        public bool IsCanceled() { return false; }
        public RenderNodeAction OnViewBegin(ViewNode inNode) { return 0; }
        public void OnViewEnd(ElementId inID) { }
        public RenderNodeAction OnElementBegin(ElementId inID)
        {
            m_stackElementId.Push(inID);
        //    Category category = CurrentElement.get_Category();
            //category.CategoryType == CategoryType.
            //bool _b = category != null && (category.get_Id().get_IntegerValue().Equals(-2001320) || category.get_Id().get_IntegerValue().Equals(-2000175) || category.get_Id().get_IntegerValue().Equals(-2000126));
          //  if ()
          //  {
                //this.ExportSolids(element);
         //       return RenderNodeAction.Skip;
         //   }
           // if (this.IsElementDecal(element))
            //{
            //    //this.ExportDecal(element);
             //   return RenderNodeAction.Skip;
           // }
         //   if (CurrentElement.Category == )
          //  {
            String _name = CurrentElement.Category.Name + "_0_" + CurrentElement.Name;
            m_Exporter.BeginMesh(_name);
            return RenderNodeAction.Proceed;
          //  }
           // else
            //    return RenderNodeAction.Skip;
           
            
        }
        public void OnElementEnd(ElementId inID) 
        {
            //if (CurrentElement.Category.CategoryType == CategoryType.Model)
            m_Exporter.EndMesh();
            m_stackElementId.Pop();
        }
        public RenderNodeAction OnInstanceBegin(InstanceNode inNode) 
        {
            m_stackTransform.Push(m_stackTransform.Peek().Multiply(inNode.GetTransform()));
            return RenderNodeAction.Proceed; 
        }
        public void OnInstanceEnd(InstanceNode inNode) 
        {
            m_stackTransform.Pop();
        }
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
