using System.Diagnostics;

namespace Tank_Game
{
    internal class UpdatableRegistry<T> : CollectionRegistry<T> where T : IUpdatable
    {
        public void ProcessAll(Action<T> action)
        {
            foreach (var item in Items)
            {
                try
                {
                    action(item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error processing [{item.GetType().Name}]: {ex.Message}");
                }
            }
        }
    }
}

