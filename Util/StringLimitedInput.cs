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
    public class StringLimitedInput
    {
        private int _length;

        public StringLimitedInput(int length)
        {
            _length = length;
        }

        private bool IsTextAllowed(string text)
        {
            bool result = text.Length <= _length;
            if (!result)
                SystemSounds.Beep.Play();
            return result;
        }

        // Use the PreviewTextInputHandler to respond to key presses 
        public void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        // Use the DataObject.Pasting Handler  
        public void PastingHandler(object sender, DataObjectPastingEventArgs e)
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
