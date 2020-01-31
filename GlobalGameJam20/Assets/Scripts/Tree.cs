using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    private Graph<Questioning> graph;

    public Tree()
    {
        graph = new DirGraph<Questioning>();

        Questioning startQuestioning = new Questioning("Wie geht es Ihnen?", "Guten Tag", "Plagen Sie Existenzängste?", "Wie heißen Sie?");
        graph.AddNode(startQuestioning);


        Questioning vertrauen = new Questioning("Vertrauen Sie?");

        graph.AddNode(vertrauen);
        
    }
}
