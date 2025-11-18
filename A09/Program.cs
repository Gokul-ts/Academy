// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a generic queue of T, using an array as the underlying storage structure.
// ------------------------------------------------------------------------------------------------
using static System.Console;

class Program {
   static void Main () {
      var q = new MyQueue<int> ();
      foreach (int n in new List<int> { 10, 20, 30, 40 })
         q.Enqueue (n);
      // 1. Checks if all elements are added
      Assert (q.Count == 4, 1);
      // 2. Checks if the queue is full
      Assert (q.IsFull (), 2);
      q.Enqueue (50);
      // 3. Checks if the Capacity is increased
      Assert (q.Capacity == 8, 3);
      // 4. Checks if first element is removed
      Assert (q.Dequeue () == 10, 4);
      // 5. Checks count after removing element
      Assert (q.Count == 4, 5);
      for (int i = 0; i < 4; i++) q.Dequeue ();
      // 6. Checks if exception is thrown when queue is empty
      try {
         q.Dequeue ();
      } catch (Exception e) {
         Assert (e.Message == "Queue is empty!", 6);
      }

      void Assert (bool condition, int caseId)
         => WriteLine ($"Test case {caseId} {(condition ? "passed" : "failed")}");
   }
}

#region Class MyList<T> ---------------------------------------------
class MyQueue<T> {
   /// <summary>Adds an element to the end of queue</summary>
   public void Enqueue (T element) {
      if (IsFull ()) {
         mEnd = Capacity;
         Array.Resize (ref mArray, Capacity * 2);
      }
      mArray[mEnd] = element;
      mEnd = (mEnd + 1) % Capacity;
      mCount++;
   }

   /// <summary>Removes and returns the first element</summary>
   public T Dequeue () {
      if (IsEmpty ()) throw new InvalidOperationException ("Queue is empty!");
      var element = mArray[mStart];
      mStart = (mStart + 1) % Capacity;
      mCount--;
      return element;
   }

   /// <summary>Returns whether the queue is empty</summary>
   public bool IsEmpty () => mCount == 0;

   /// <summary>Returns whether the queue is full</summary>
   public bool IsFull () => mCount == Capacity;

   #region Properties -----------------------------------------------
   public int Capacity {
      get { mCapacity = mArray.Length; return mCapacity; }
   }
   int mCapacity;

   public int Count => mCount;
   int mCount;
   #endregion

   #region Private variables ----------------------------------------
   int mStart;
   int mEnd;
   T[] mArray = new T[4];
   #endregion
}
#endregion