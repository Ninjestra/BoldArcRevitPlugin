using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Utility;
using System;
using System.Windows.Forms;

namespace BoldarcRevitPlugin
{


    [Journaling(JournalingMode.NoCommandData), Regeneration(RegenerationOption.Manual), Transaction(TransactionMode.Manual)]
    public class Activate : IExternalCommand
    {
        public Result Execute(ExternalCommandData inCommandData, ref string inMessage, ElementSet inElements)
        {
            UIApplication application = inCommandData.Application;
            UIDocument activeUIDocument = application.ActiveUIDocument;
            if (activeUIDocument == null)
            {
                return Result.Cancelled;
            }
            Autodesk.Revit.ApplicationServices.Application arg_19_0 = application.Application;
            Document document = activeUIDocument.Document;
            if (document == null)
            {
                return Result.Cancelled;
            }
            if (document.ActiveView is View3D)
            {
               
                //exporterDialog.SaveFileDialog.FileName = exporterDialog.SaveFileDialog.FileName.Replace(".rvt", ".dae");
                ExportView3D(document, document.ActiveView as View3D);
            }
            else
            {
                MessageBox.Show("You must be in 3D view to export.");
            }
            return Result.Succeeded;
        }

        internal void ExportView3D(Document document, View3D view3D)
        {
            BoldarcExportContext _exportContext = new BoldarcExportContext(document);
            CustomExporter customExporter = new CustomExporter(document, _exportContext);
            customExporter.IncludeFaces = false;
            customExporter.ShouldStopOnError = false;
            customExporter.Export(view3D);
        }

       /* private string GetAssetDescription(Asset asset)
        {
            string text = "";
            for (int i = 0; i < asset.get_Size(); i++)
            {
                AssetProperty assetProperty = asset.get_Item(i);
                text += this.GetPropertyDescription(assetProperty);
            }
            return text;
        }

        private string GetPropertyDescription(AssetProperty assetProperty)
        {
            string text = assetProperty.get_Name() + ": " + assetProperty.get_Type().ToString() + "  ";
            switch (assetProperty.get_Type())
            {
                case 2:
                    text += (assetProperty as AssetPropertyBoolean).get_Value().ToString();
                    break;
                case 4:
                    text += (assetProperty as AssetPropertyInteger).get_Value().ToString();
                    break;
                case 5:
                    text += (assetProperty as AssetPropertyFloat).get_Value().ToString();
                    break;
                case 6:
                    text += (assetProperty as AssetPropertyDouble).get_Value().ToString();
                    break;
                case 7:
                    text += (assetProperty as AssetPropertyDoubleArray2d).get_Value().ToString();
                    break;
                case 8:
                    text += (assetProperty as AssetPropertyDoubleArray3d).get_Value().ToString();
                    break;
                case 9:
                    text += (assetProperty as AssetPropertyDoubleArray4d).get_Value().ToString();
                    break;
                case 10:
                    text += (assetProperty as AssetPropertyDoubleMatrix44).get_Value().ToString();
                    break;
                case 11:
                    text += (assetProperty as AssetPropertyString).get_Value().ToString();
                    break;
                case 14:
                    text += (assetProperty as AssetPropertyDistance).get_Value().ToString();
                    break;
                case 15:
                    text += this.GetAssetDescription(assetProperty as Asset);
                    break;
                case 17:
                    text += (assetProperty as AssetPropertyInt64).get_Value().ToString();
                    break;
                case 18:
                    text += (assetProperty as AssetPropertyUInt64).get_Value().ToString();
                    break;
                case 20:
                    text += (assetProperty as AssetPropertyFloatArray).GetValue().ToString();
                    break;
            }
            text += "\n";
            if (assetProperty.get_NumberOfConnectedProperties() > 0)
            {
                text += "CONNECTED START\n";
            }
            for (int i = 0; i < assetProperty.get_NumberOfConnectedProperties(); i++)
            {
                text = text + "       " + this.GetPropertyDescription(assetProperty.GetConnectedProperty(i));
            }
            if (assetProperty.get_NumberOfConnectedProperties() > 0)
            {
                text += "CONNECTED END\n";
            }
            return text;
        }

        private string FindTexturePathInAsset(Asset asset)
        {
            for (int i = 0; i < asset.get_Size(); i++)
            {
                AssetProperty assetProperty = asset.get_Item(i);
                string text = this.FindTexturePathInAssetProperty(assetProperty);
                if (text.Length > 0)
                {
                    return text;
                }
            }
            return "";
        }

        private string FindTexturePathInAssetProperty(AssetProperty assetProperty)
        {
            AssetPropertyType type = assetProperty.get_Type();
            if (type != 11)
            {
                if (type == 15)
                {
                    string text = this.FindTexturePathInAsset(assetProperty as Asset);
                    if (text.Length > 0)
                    {
                        return text;
                    }
                }
            }
            else if (assetProperty.get_Name() == "unifiedbitmap_Bitmap")
            {
                return (assetProperty as AssetPropertyString).get_Value().ToString();
            }
            for (int i = 0; i < assetProperty.get_NumberOfConnectedProperties(); i++)
            {
                string text2 = this.FindTexturePathInAssetProperty(assetProperty.GetConnectedProperty(i));
                if (text2.Length > 0)
                {
                    return text2;
                }
            }
            return "";
        }*/
    }
}
