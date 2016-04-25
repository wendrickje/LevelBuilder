using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.Common
{
    namespace ResourceKeys
    {
        public class Global
        {
            public static string Foreground = "textforeground";
            public static string ForegroundHover = "textforegroundhover";
            public static string FontSize = "size";
            public static string FontFamily = "textfont";
            public static string DisabledForeground = "disabledcolorforeground";
            public static string DisabledBackground = "disabledcolorbackground";
            public static string ValidationTemplate = "ValidationTemplate";

            public const int EmptyTileKey = 0;
            public const int StartTileKey = -1;
            public const int EventTileKey = -2;
            
            
            public static string EventTileBrush = "EventTileBrush";
            public static string StartTileBrush = "StartTileBrush";
            public static string EmptyTileBrush = "EmptyTileBrush";
            public static string FillBucketTileBrush = "FillBucketTileBrush";
            public static string PickerTileBrush = "PickerTileBrush";



        }

        public class Icons
        {
            public static string Save = "SaveIcon";
            public static string SaveAs = "SaveAsIcon";
            public static string Export = "ExportIcon";
            public static string New = "NewIcon";
            public static string Layers = "LayersIcon";
            public static string VisibilityOn = "VisibilityOnIcon";
            public static string VisibilityOff = "VisibilityOffIcon";
            public static string Brush = "BrushIcon";
            public static string Help = "HelpIcon";
            public static string Browse = "Browse";
            public static string Settings = "Settings";
            public static string Configure = "Configure";
            public static string Flag = "Flag";
            public static string Warning = "Warning";
            public static string Event = "Event";
            public static string Selection = "Selection";
            public static string Bucket = "Bucket";
            public static string Eraser = "Eraser";
            

        }

        public class Expander
        {
            public static string HoverFillColor = "hoverbuttonfill";
            //public static string ButtonHoverBorderColor = "ButtonHoverBorderColor";
            public static string PressedBorderColor = "ButtonPressedBorderColor";
            public static string PressedFillColor = "ButtonPressedFillColor";

        }

        public class ToolBar
        {

            public static string ButtonHoverFillColor = "hoverbuttonfill";

            public static string GripperFillColor = "gripperfill";
            
            public static string Background = "background";

            public static string OverflowButtonBackground = "overflowbuttonbackground";

            public static string OverflowButtonForeground = "overflowbuttonforeground";
        }


        public class Button
        {

            public static string ButtonHoverFillColor = "hoverbuttonfill";
            public static string ButtonFillColor = "ButtonFillColor";
            public static string ButtonPressedBorderColor = "ButtonPressedBorderColor";
            public static string ButtonPressedFillColor = "ButtonPressedFillColor";

        }
    }
}

