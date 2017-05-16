using System;
using System.Windows;
using System.Windows.Controls;
using cl.uv.leikelen.src.Helper;
using cl.uv.leikelen.src.View.CoreWindow; //TODO: eliminar interdependencia

namespace cl.uv.leikelen.src.View.Widget
{
    public static class TimeLine
    {
        public static void InitTimeLine(TimeSpan duration)
        {

            ColumnDefinition rulerCol, contentCol;
            TextBlock text;

            TimeSpan frameTime = TimeSpan.FromSeconds(0);
            int colSpan = 10;
            for (int colCount = 0; frameTime < duration; colCount++)
            {
                rulerCol = new ColumnDefinition();
                rulerCol.Width = new GridLength(5, GridUnitType.Pixel);
                MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Add(rulerCol);

                contentCol = new ColumnDefinition();
                contentCol.Width = new GridLength(5, GridUnitType.Pixel);
                MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Add(contentCol);

                if (colCount % colSpan == 0 && colCount != 0)
                {
                    text = new TextBlock();
                    text.Text = "|";
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, colCount);
                    Grid.SetColumnSpan(text, colSpan);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);

                    text = new TextBlock();
                    text.Text = TimeUtils.TimeSpanToShortString(frameTime);
                    text.HorizontalAlignment = colCount == 0 ?
                        HorizontalAlignment.Left : HorizontalAlignment.Center;
                    Grid.SetRow(text, 1);
                    int offset = colCount == 0 ? 0 : (colSpan / 2);
                    Grid.SetColumn(text, colCount - offset);
                    Grid.SetColumnSpan(text, colSpan);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);
                }
                else
                {
                    text = new TextBlock();
                    text.Text = "·";
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, colCount);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);
                }

                frameTime = frameTime.Add(TimeSpan.FromMilliseconds(1000.00));
            }
        }
    }
}
