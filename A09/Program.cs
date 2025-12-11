// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a generic queue of T, using an array as the underlying storage structure.
// ------------------------------------------------------------------------------------------------
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      var q = new MyQueue<int> ();
      foreach (int n in new List<int> { 10, 20, 30, 40 })
         q.Enqueue (n);
      // Checks if all elements are added
      Assert (q.Count == 4);
      // Checks if the queue is full
      Assert (q.IsFull);
      q.Enqueue (50);
      // Checks if the Capacity is increased
      Assert (q.Capacity == 8);
      // Checks if first element is removed
      Assert (q.Dequeue () == 10);
      // Checks count after removing element
      Assert (q.Count == 4);
      for (int i = 0; i < 4; i++) q.Dequeue ();
      // Checks if exception is thrown when queue is empty
      try {
         q.Dequeue ();
      } catch (Exception e) {
         Assert (e.Message == "Queue is empty!");
      }
      WriteLine ("Test cases passed");

      // Helper function to assert test cases
      void Assert (bool condition) {
         if (!condition) { WriteLine ("Test cases failed"); Environment.Exit (0); }
      }
   }
}
#endregion

#region Class MyQueue<T> --------------------------------------------------------------------------
class MyQueue<T> {
   #region Properties -----------------------------------------------
   public bool IsEmpty => mCount == 0;
   public bool IsFull => mCount == Capacity;
   public int Capacity => mArray.Length;
   public int Count => mCount;
   int mCount;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an element to the end of queue</summary>
   public void Enqueue (T element) {
      if (IsFull) {
         mEnd = Capacity;
         Array.Resize (ref mArray, Capacity * 2);
      }
      mArray[mEnd] = element;
      mEnd = (mEnd + 1) % Capacity;
      mCount++;
   }

   /// <summary>Removes and returns the first element</summary>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty!");
      var element = mArray[mStart];
      mStart = (mStart + 1) % Capacity;
      var temp = new T[Capacity];
      for (int i = mStart; i < mEnd; i++) temp[i] = mArray[i];
      mArray = temp;
      mCount--;
      return element;
   }
   #endregion

   #region Private data ---------------------------------------------
   int mStart, mEnd;
   T[] mArray = new T[4];
   #endregion
}
#endregion