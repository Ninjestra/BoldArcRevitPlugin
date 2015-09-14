using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoldarcManagedFbx;
using Autodesk.Revit.DB;

namespace BoldarcRevitPlugin
{
    class FbxExporter
    {
        private FileExporter m_pFileExporter;
        public FbxExporter()
        {
            m_pFileExporter = new FileExporter();
        }
        public void ExportFbx(IList<XYZ> inVertices, IList<PolymeshFacet> inIndices)
        {
            List<Vector3> _lVertices = new List<Vector3>(inVertices.Count);
            foreach (XYZ _vert in inVertices)
            {
                _lVertices.Add(new Vector3(_vert.X, _vert.Y, _vert.Z));
            }

            List<int> _lIndices = new List<int>(inIndices.Count * 3);
            foreach (PolymeshFacet _ind in inIndices)
            {
                _lIndices.Add(_ind.V1);
                _lIndices.Add(_ind.V2);
                _lIndices.Add(_ind.V3);
            }
            m_pFileExporter.SetGeometry(_lVertices, _lIndices);
            m_pFileExporter.ExportFile();
        }
    }
}
