using System;
using System.Collections.Generic;
using BoldarcManagedFbx;
using Autodesk.Revit.DB;

namespace BoldarcRevitPlugin
{
    class FbxExporter
    {
        private FileExporter m_pFileExporter;
        private BoldarcManagedFbx.Mesh m_pCurrentMesh;
        public FbxExporter()
        {
            m_pFileExporter = new FileExporter();
        }

        public void BeginMesh(String inName)
        {
            m_pCurrentMesh = new BoldarcManagedFbx.Mesh(inName);
        }

        public void AddGeometry(IList<XYZ> inVertices, IList<PolymeshFacet> inIndices, IList<XYZ> inNormals, Transform inTransform)
        {
            List<Vector3> _lVertices = new List<Vector3>(inVertices.Count);
            foreach (XYZ _vert in inVertices)
            {
                XYZ _newVert = inTransform.OfPoint(_vert);
                _lVertices.Add(new Vector3(_newVert.X, _newVert.Y, _newVert.Z));
            }

            List<int> _lIndices = new List<int>(inIndices.Count * 3);
            foreach (PolymeshFacet _ind in inIndices)
            {
                _lIndices.Add(_ind.V1);
                _lIndices.Add(_ind.V2);
                _lIndices.Add(_ind.V3);
            }

            List<Vector3> _lNormals = new List<Vector3>(inNormals.Count);
            foreach (XYZ _norm in inNormals)
            {
                //XYZ _newNorm = inTransform.OfPoint(_norm);
                _lNormals.Add(new Vector3(_norm.X, _norm.Y, _norm.Z));
            }

            m_pCurrentMesh.AddGeometry(_lVertices, _lIndices, _lNormals);
            
        }

        public void EndMesh()
        {
            m_pCurrentMesh.OptimizePoints();
            if (!m_pCurrentMesh.IsEmpty())
                m_pFileExporter.AddMesh(m_pCurrentMesh);
        }

        public void ExportFbx()
        {
            m_pFileExporter.ExportFile();
        }

       
    }
}
