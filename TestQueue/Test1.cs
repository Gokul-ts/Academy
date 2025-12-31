namespace TestQueue;

[TestClass]
public sealed class PriorityQueueTests {
   [TestMethod]
   public void TestEnqueue () {
      PriorityQueue<int> q = [];
      Assert.IsTrue (q.IsEmpty);
      q.Enqueue (3);
      Assert.IsFalse (q.IsEmpty);
      q.Enqueue (1);
      q.Enqueue (2);
      Assert.AreEqual (1, q.Dequeue ());
   }

   [TestMethod]
   public void TestDequeue () {
      PriorityQueue<int> q = [];
      Assert.IsTrue (q.IsEmpty);
      Assert.ThrowsException<InvalidOperationException> (() => q.Dequeue ());
      q.Enqueue (2);
      q.Enqueue (-1);
      Assert.AreEqual (-1, q.Dequeue ());
   }

   [TestMethod]
   public void TestRandom () {
      const int MAX = 100000;
      var (q, list, r) = (new PriorityQueue<int> (), new List<int> (), new Random ());
      Assert.IsTrue (q.IsEmpty);
      for (int i = 0; i < MAX; i++) {
         var elem = r.Next (int.MinValue, int.MaxValue);
         q.Enqueue (elem);
         list.Add (elem);
      }
      Assert.IsFalse (q.IsEmpty);
      list.Sort ();
      for (int i = 0; i < MAX; i++)
         Assert.AreEqual (list[i], q.Dequeue ());
   }
}