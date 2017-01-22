using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> {
// I'm using an unsorted array for this example, but ideally this
// would be a binary heap. Find a binary heap class:
// * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
// * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
// * http://xfleury.github.io/graphsearch.html
// * http://stackoverflow.com/questions/102398/priority-queue-in-net
	
	//Tuple use
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();
	//private List<int[]> elements = new List<int[]>();

    public int Count
    {
        get { return elements.Count; }
    }
    
    public void Enqueue(T item, int priority)
    {
    	Tuple<T, int> tup = new Tuple<T, int>(item, priority);
    	elements.Add(tup);
        //elements.Add(Tuple.Create(item, priority));
        //elements.Add(new int[2] {item, }));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].second < elements[bestIndex].second) {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].first;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}