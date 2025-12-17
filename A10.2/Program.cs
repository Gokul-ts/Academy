// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement double ended queue of T, using an array as the underlying data structure.
// ------------------------------------------------------------------------------------------------
using static System.Console;

#region class Program -----------------------------------------------------------------------------
/// <summary>class to check the functions of a double sided queue</summary>
internal class Program {
   static void Main () {
      var (q, r, n) = (new DQueue<int> (), new Random (), 0);
      for (int i = 0; i < 100; i++)
         if (r.NextDouble () < K) {
            if (!q.IsEmpty) WriteLine (r.NextDouble () < K ? $"RemoveFront:{q.DeqFront (),3}"
                                                           : $"RemoveRear :{q.DeqRear (),3}");
         } else {
            bool isFront = r.NextDouble () < K;
            if (isFront) q.EnqFront (++n);
            else q.EnqRear (++n);
            WriteLine (isFront ? $"AddFront   :{n,3}" : $"AddRear    :{n,3}");
         }
   }

   #region Private data ---------------------------------------------
   const double K = 0.25;
   #endregion
}
#endregion

#region class DQueue<T> --------------------------------------------------------------------------
/// <summary>Represents a double sided queue with FI-FO, LI-LO, FI-LO and LI-FO collection of 
/// objects</summary>
class DQueue<T> {
   #region Properties -----------------------------------------------
   public int Capacity => mArray.Length;

   public int Count => mCount;

   public bool IsFull => Capacity == mCount;

   public bool IsEmpty => mCount == 0;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an item to the front of DQueue</summary>
   public void EnqFront (T item) {
      if (IsFull) Resize ();
      mFront = (mFront - 1 + Capacity) % Capacity;
      mArray[mFront] = item;
      mCount++;
   }

   /// <summary>Adds an item to the rear of DQueue</summary>
   public void EnqRear (T item) {
      if (IsFull) Resize ();
      mArray[mRear] = item;
      mRear = (mRear + 1) % Capacity;
      mCount++;
   }

   /// <summary>Removes and returns an item from the front of DQueue</summary>
   public T DeqFront () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty!");
      var value = mArray[mFront];
      mArray[mFront] = default!;
      mFront = (mFront + 1) % Capacity;
      mCount--;
      return value;
   }

   /// <summary>Removes and returns an item from the rear of DQueue</summary>
   public T DeqRear () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty!");
      mRear = (mRear - 1 + Capacity) % Capacity;
      var value = mArray[mRear];
      mArray[mRear] = default!;
      mCount--;
      return value;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Resizes the queue with new capacity
   void Resize () {
      T[] temp = new T[Capacity * 2];
      for (int i = 0; i < mCount; i++)
         temp[i] = mArray[(mFront + i) % Capacity];
      (mArray, mFront, mRear) = (temp, 0, mCount);
   }
   #endregion

   #region Private data ---------------------------------------------
   T[] mArray = new T[4];
   int mFront, mRear, mCount;
   #endregion
}
#endregion