using Autodesk.Revit.UI;
//using RevitExporter.Properties;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BoldarcRevitPlugin
{
    internal class BoldarcRibbon : IExternalApplication
    {

        private string AssemblyFullName
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        public Result OnStartup(UIControlledApplication a)
        {
            RibbonPanel panel = a.CreateRibbonPanel("Boldarc");
            this.AddPushButton(panel);
            return 0;
        }

        private void AddPushButton(RibbonPanel panel)
        {
            PushButton pushButton = panel.AddItem(new PushButtonData("Export", "Export to Boldarc", this.AssemblyFullName, "BoldarcRevitPlugin.Activate")) as PushButton;
            pushButton.ToolTip = "Export to Boldarc";
            ContextualHelp contextualHelp = new ContextualHelp(0, "Boldarc support info goes here.");
            pushButton.SetContextualHelp(contextualHelp);
            //pushButton.set_LargeImage(Imaging.CreateBitmapSourceFromHBitmap(Resources.LogoBig.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
            //pushButton.set_Image(Imaging.CreateBitmapSourceFromHBitmap(Resources.LogoSmall.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return 0;
        }
    }
}
