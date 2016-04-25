using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LevelBuilder.Converters
{
    public class TileCanvasGridItemTemplateSelector : DataTemplateSelector
    {



        public DataTemplate StandardTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate EventTemplate { get; set; }
        public DataTemplate StartTemplate { get; set; }




        #region Overrides of DataTemplateSelector

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var value = item as int?;
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case Common.ResourceKeys.Global.StartTileKey:
                        return StartTemplate;
                    case Common.ResourceKeys.Global.EmptyTileKey:
                        return EmptyTemplate;
                    case Common.ResourceKeys.Global.EventTileKey:
                        return EventTemplate;
                }
            }

            return StandardTemplate;
        }

        #endregion
    }
}
