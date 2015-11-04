using System;
using System.Text;
using System.Collections.Generic;
using BoldarcManagedFbx;
using Autodesk.Revit.DB;

namespace BoldarcRevitPlugin
{
    class FbxExporter
    {
        private FileExporter m_pFileExporter;
        private BoldarcManagedFbx.Mesh m_pCurrentMesh;
        //private Transform m_pCurrentTransform;
        private Dictionary<int, BoldarcManagedFbx.Material> m_pMaterialDict;
        

        public FbxExporter()
        {
            m_pFileExporter = new FileExporter();
            m_pMaterialDict = new Dictionary<int, BoldarcManagedFbx.Material>();
        }

        public void BeginMesh(String inName)
        {
            m_pCurrentMesh = new BoldarcManagedFbx.Mesh(inName);
            m_pCurrentMesh.Position = new Vector3();
            m_pCurrentMesh.Rotation = new Vector3();
            m_pCurrentMesh.Scale = 1.0;
        }

        public void AddTransform(Transform inTrans)
        {
            //m_pCurrentTransform = inTrans;

            XYZ _rot = inTrans.OfVector(XYZ.Zero);
            XYZ _pos = inTrans.OfPoint(_rot);
            double _scale = inTrans.Scale;

            m_pCurrentMesh.Position = new Vector3(_pos.X, _pos.Y, _pos.Z);
            m_pCurrentMesh.Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z);
            m_pCurrentMesh.Scale = _scale;

        }

        public void AddGeometry(IList<XYZ> inVertices, IList<PolymeshFacet> inIndices, IList<XYZ> inNormals, Transform inTransform)
        {
            List<Vector3> _lVertices = new List<Vector3>(inVertices.Count);
            foreach (XYZ _vert in inVertices)
            {
                XYZ _newVert = inTransform.OfPoint(_vert);//_ofVert -_ofZero;

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
                _lNormals.Add(new Vector3(_norm.X, _norm.Y, _norm.Z));
            }

            m_pCurrentMesh.AddGeometry(_lVertices, _lIndices, _lNormals);

            

            
        }

        public void AddMaterial(Document inDocument, MaterialNode inNode)
        {
            if (!m_pMaterialDict.ContainsKey(inNode.MaterialId.IntegerValue))
            {
                Autodesk.Revit.DB.Material _revitMat = inDocument.GetElement(inNode.MaterialId) as Autodesk.Revit.DB.Material;
                if (_revitMat != null && _revitMat.IsValidObject)
                {
                    BoldarcManagedFbx.Material _mat = new BoldarcManagedFbx.Material(_revitMat.Name);

                    _mat.Red = _revitMat.Color.Red;
                    _mat.Green = _revitMat.Color.Green;
                    _mat.Blue = _revitMat.Color.Blue;
                    _mat.Shininess = _revitMat.Shininess;
                    _mat.Smoothness = _revitMat.Smoothness;
                    _mat.Transparency = _revitMat.Transparency;

                    m_pMaterialDict.Add(inNode.MaterialId.IntegerValue, _mat);
                    m_pCurrentMesh.MaterialIDPerFace.Add(m_pCurrentMesh.FaceCount, inNode.MaterialId.IntegerValue);
                }
                else
                    m_pCurrentMesh.MaterialIDPerFace.Add(m_pCurrentMesh.FaceCount, -1);
            }
            else
                m_pCurrentMesh.MaterialIDPerFace.Add(m_pCurrentMesh.FaceCount, inNode.MaterialId.IntegerValue);



        }

        public void EndMesh()
        {
            //m_pCurrentTransform = null;
            //m_pCurrentMesh.OptimizePoints();
            m_pCurrentMesh.FixPosition();
            if (!m_pCurrentMesh.IsEmpty())
            {
                m_pFileExporter.AddMesh(m_pCurrentMesh);
            }
        }

        public void ExportFbx()
        {
            m_pFileExporter.ExportFile(m_pMaterialDict);
        }

       
    }
}
