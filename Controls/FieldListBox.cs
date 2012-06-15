using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.Controls
{
    class FieldListBox : System.Windows.Forms.ListBox
    {
        public FieldListBox()
        {
            this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        }
    }
}
