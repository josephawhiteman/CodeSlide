using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.Modeler
{

    public class TreeConfig
    {
        public enum TreeViewMode
        {
            Heirarchical,
            Flat,
            ClassHeirarchy
        }
        public enum GroupBy
        {
            Entity,
            Schema
        }

        private GroupBy _ProjectGrouping = GroupBy.Schema;
        public GroupBy ProjectGrouping
        {
            get { return _ProjectGrouping; }
            set { _ProjectGrouping = value; }
        }

        private TreeViewMode _ViewMode = TreeViewMode.Heirarchical;
        public TreeViewMode ViewMode
        {
            get { return _ViewMode; }
            set { _ViewMode = value; }
        }
        private System.Windows.Forms.TreeView _Tree;
        public System.Windows.Forms.TreeView Tree
        {
            get { return _Tree; }
            set { _Tree = value; }
        }
    }
}
