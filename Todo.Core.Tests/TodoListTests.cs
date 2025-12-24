using Xunit;
using Todo.Core;
using System.Linq;
using System.IO; 

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void Save_And_Load_Preserves_All_Data()
        {
            // Подготовка
            var originalList = new TodoList();
            var item1 = originalList.Add("Купить молоко");
            var item2 = originalList.Add("Прочитать книгу");
            item1.MarkDone();

            string testFile = "test_save.json";

            try
            {
                originalList.Save(testFile);
                var loadedList = new TodoList();
                loadedList.Load(testFile);

                Assert.Equal(2, loadedList.Count);

                var loadedItem1 = loadedList.Items.First(i => i.Title == "Купить молоко");
                var loadedItem2 = loadedList.Items.First(i => i.Title == "Прочитать книгу");

                Assert.True(loadedItem1.IsDone);
                Assert.False(loadedItem2.IsDone);
                Assert.NotEqual(Guid.Empty, loadedItem1.Id);
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public void Load_FromNonExistentFile_ThrowsException()
        {
            var list = new TodoList();
            string fakePath = "non_existent_file.json";

            Assert.Throws<FileNotFoundException>(() => list.Load(fakePath));
        }
    }
}