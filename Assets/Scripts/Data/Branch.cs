using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    public class Branch
    {
        public Branch ParentBranch { get; set; }
        public List<Branch> ChildBranches { get; set; }
        public List<Leaf> ChildLeaves { get; set; }
        public float Diameter { get; set; }
        public float Length { get; set; }

        public Branch()
        {
            ChildLeaves = new List<Leaf>();
            ChildBranches = new List<Branch>();
        }
    }
}