using System.Collections.Generic;
using UnityEngine;

public abstract class NodeComposite : Node
{
   [HideInInspector] public List<Node> children = new();

   public override Node Clone()
   {
      NodeComposite nodeComposite = Instantiate(this);
      nodeComposite.children = children.ConvertAll(child => child.Clone());
      return nodeComposite;
   }
}
