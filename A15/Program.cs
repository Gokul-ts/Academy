// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a Priority Queue of generic type T with underlying data structure List<T>.
// ------------------------------------------------------------------------------------------------
using System.Collections;
using static System.Console;

#region Class program -----------------------------------------------------------------------------
internal class Program {
   static void Main () {
      var (r, q) = (new Random (), new PriorityQueue<int> ());
      for (int i = 1; i <= 100; i++) {
         if (r.NextDouble () < 0.75) {
            q.Enqueue (i);
            WriteLine ($"Added  : {i}");
            continue;
         } else if (!q.IsEmpty) WriteLine ($"Removed: {q.Dequeue ()}");
         WriteLine ($"Queue  : {(q.IsEmpty ? "Empty" : string.Join (',', q))}");
      }
      WriteLine ($"Test case {(TestQ () ? "passed" : "failed")}");

      // Helper function to check if each parent node is less than its children
      bool TestQ () {
         if (!q.IsEmpty) {
            ReadOnlySpan<int> arr = q.ToArray ();
            int p = 0, n = arr.Length;
            while (true) {
               var (c1, c2) = (2 * p + 1, 2 * p + 2);
               if (c1 >= n) break;
               int c = (c2 < n && arr[c2] < arr[c1]) ? c2 : c1;
               if (arr[p] > arr[c]) return false;
               p = c;
            }
         }
         return true;
      }
   }
}
#endregion

#region Class PriorityQueue -----------------------------------------------------------------------
/// <summary>Class priority queue is a binary tree that implements the shape property to 
/// maintain balance and the ordering property that enables it to work as a priority queue</summary>
public class PriorityQueue<T> (int capacity = 0) : IEnumerable<T> where T : IComparable<T> {
   #region Properties -----------------------------------------------
   public bool IsEmpty => mCount == 0;
   #endregion

   #region Interface Implementation ---------------------------------
   /// <summary>Returns each element in the internal collection</summary>
   public IEnumerator<T> GetEnumerator () {
      for (int i = 0; i < mCount; i++)
         yield return mList[i];
   }
   IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds the element to the queue in order</summary>
   public void Enqueue (T elem) {
      int n = mCount; mList.Add (elem);
      while (n > 0) {
         int p = (n - 1) / 2;                           // Index of parent
         if (mList[n].CompareTo (mList[p]) >= 0) break; // element >= parent
         (mList[n], mList[p]) = (mList[p], mList[n]);   // Swap element and parent
         n = p;
      }
   }

   /// <summary>Removes and returns the smallest element and re-orders the queue</summary>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty");
      var elem = mList[0];
      int last = mCount - 1;
      mList[0] = mList[last];
      mList.RemoveAt (last);
      int p = 0, n = mCount;
      while (true) {
         var (l, r) = (2 * p + 1, 2 * p + 2);                          // left and right child
         if (l >= n) break;                                            // no children in tree
         int c = (r < n && mList[r].CompareTo (mList[l]) < 0) ? r : l; // pick smaller child
         if (mList[p].CompareTo (mList[c]) <= 0) break;                // Shape property satisfied
         (mList[p], mList[c]) = (mList[c], mList[p]);                  // swap parent with child
         p = c;
      }
      return elem;
   }
   #endregion

   #region Private data ---------------------------------------------
   List<T> mList = new (capacity);
   int mCount => mList.Count;
   #endregion
}
#endregion