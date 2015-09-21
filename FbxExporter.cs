using System;
using System.Collections.Generic;
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
        public void AddGeometry(IList<XYZ> inVertices, IList<PolymeshFacet> inIndices)
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
            //Mesh _Mesh = new Mesh();
            //_Mesh.AddGeometry(_lVertices, _lIndices);
            //m_pFileExporter.AddMesh(_Mesh);
            m_pFileExporter.AddGeometry(_lVertices, _lIndices);
            
        }
        public void ExportFbx()
        {
            m_pFileExporter.ExportFile();
        }

       
    }
}
