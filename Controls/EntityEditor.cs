using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.Controls
{
    public partial class EntityEditor : UserControl 
    {
        public EntityEditor()
        {
            InitializeComponent();
        }
        public EntityEditor( Entity entity)
        {
            InitializeComponent();
            this.Entity = entity;
            LoadForm();
        }
        private Entity _Entity;
        public Entity Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        private void LoadForm()
        {
            textBox1.Text = Entity.LogicalName;
        }
    }
}
