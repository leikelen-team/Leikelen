using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Media;

namespace cl.uv.leikelen.Util
{
    public static class IntegerInput
    {
        private static bool IsTextAllowed(string text)
        {
            bool result = int.TryParse(text, out int value);
            if(!result)
                SystemSounds.Beep.Play();
            return result;

        }

        // Use the PreviewTextInputHandler to respond to key presses 
        public static void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        // Use the DataObject.Pasting Handler  
        public static void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}
