using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LevelBuilder.Common
{
    public class Utilities
    {

        public static DependencyObject GetAncestorFromType(DependencyObject descendant, Type type, bool allowSubclassOfType, DependencyObject stopAtVisual = null, Type stopAtType = null)
        {
            DependencyObject parent = null;
            for (DependencyObject i = descendant; i != null; i = parent)
            {
                parent = Utilities.GetParent(i, true);
                if (parent != null)
                {
                    if (parent == stopAtVisual)
                    {
                        return null;
                    }
                    if (stopAtType != null && stopAtType == parent.GetType())
                    {
                        return null;
                    }
                    Type type1 = parent.GetType();
                    if (type1 == type)
                    {
                        return parent;
                    }
                    if (allowSubclassOfType && type.IsAssignableFrom(type1))
                    {
                        return parent;
                    }
                }
            }
            return null;
        }

        public static DependencyObject GetParent(DependencyObject child, bool includeLogicalParent)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }
            Visual visual = child as Visual;
            if (visual != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(visual);
                if (parent != null)
                {
                    return parent;
                }
                if (includeLogicalParent)
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        return frameworkElement.Parent;
                    }
                }
                return null;
            }
            if (includeLogicalParent)
            {
                FrameworkContentElement frameworkContentElement = child as FrameworkContentElement;
                if (frameworkContentElement != null)
                {
                    return frameworkContentElement.Parent;
                }
            }
            Visual3D visual3D = child as Visual3D;
            if (visual3D == null)
            {
                return null;
            }
            return VisualTreeHelper.GetParent(visual3D);
        }
        

        public class ExceptionUtilities
        {
            public static void HandleException(Exception e)
            {
                var msg = e.Message;
                var inner = e.InnerException == null ? null : e.InnerException.Message;
                MessageBox.Show(String.Format("{0}\n{1}", msg, inner), Common.Constants.ExceptionDialogTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
