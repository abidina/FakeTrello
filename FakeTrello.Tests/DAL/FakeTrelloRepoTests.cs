using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using Moq;
using FakeTrello.Models;
using System.Linq;
using System.Data.Entity;

namespace FakeTrello.Tests.DAL
{
    [TestClass]
    public class FakeTrelloRepoTests
    {
        public Mock<FakeTrelloContext> fakeContext { get; set; }
        public FakeTrelloRepository repo { get; set; }
        public Mock<DbSet<Board>> mockBoardsSet { get; set; }
        public IQueryable<Board> queryBoards { get; set; }
        public List<Board> fakeBoardTable { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fakeBoardTable = new List<Board>();
            fakeContext = new Mock<FakeTrelloContext>();
            mockBoardsSet = new Mock<DbSet<Board>>();
            repo = new FakeTrelloRepository(fakeContext.Object);
            // IQueryable<Board>
            queryBoards = fakeBoardTable.AsQueryable(); //Re-express this list as something that understands queries

        }

        private void CreateFakeDatabase()
        {
            mockBoardsSet.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fakeBoardTable.Add(board));
            fakeContext.Setup(c => c.Boards).Returns(mockBoardsSet.Object); //Context.Boards returns a fakeBoardTable (a list)

            // Hey LINQ, use the Provider from our FAKE board table/list
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(queryBoards.Provider);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(queryBoards.Expression);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(queryBoards.ElementType);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => queryBoards.GetEnumerator());
            mockBoardsSet.Setup(b => b.Remove(It.IsAny<Board>())).Callback((Board board) => fakeBoardTable.Remove(board));

        }


        [TestMethod]
        public void EnsureICanCreateInstanceOfRepo()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void EnsureIHaveNotNullContext()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanInjectContextInstance()
        {
            FakeTrelloContext context = new FakeTrelloContext();
            FakeTrelloRepository repo = new FakeTrelloRepository(context);

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanAddBoard()
        {
            //Arrange
            List<Board> fakeBoardTable = new List<Board>();

            CreateFakeDatabase();

            ApplicationUser user = new ApplicationUser
            {
                Id = "myUserId",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            //Act
            repo.AddBoard("My Board", user);

            //Assert
            Assert.AreEqual(repo.Context.Boards.Count(), 1);
        }

        [TestMethod]
        public void EnsureICanReturnBoards()
        {
            //Arrange
            fakeBoardTable.Add(new Board { Name = "My Board"});
            CreateFakeDatabase();

            //Act
            int expectedBoardCount = 1;
            int actualBoardCount = repo.Context.Boards.Count();

            //Assert
            Assert.AreEqual(expectedBoardCount, actualBoardCount);

        }

        [TestMethod]
        public void EnsureICanFindABoard()
        {
            // Arrange
            fakeBoardTable.Add(new Board { BoardId = 1, Name = "My Board" });
            CreateFakeDatabase();

            //Act
            string expectedBoardName = "My Board";
            string actualBoardName = repo.GetBoard(1).Name;

            //Assert
            Assert.AreEqual(expectedBoardName, actualBoardName);
        }

        [TestMethod]
        public void EnsureICanRemoveBoard()
        {
            ApplicationUser Sally = new ApplicationUser();
            //Arrange
            fakeBoardTable.Add(new Board { BoardId = 1, Name = "My Board", Owner = Sally });
            fakeBoardTable.Add(new Board { BoardId = 2, Name = "My Board", Owner = Sally });
            fakeBoardTable.Add(new Board { BoardId = 3, Name = "My Board", Owner = Sally });
            CreateFakeDatabase();

            //Act
            int expectedBoardCount = 2;
            repo.RemoveBoard(3);
            int actualBoardCount = repo.Context.Boards.Count();

            //Assert
            Assert.AreEqual(expectedBoardCount, actualBoardCount);
        }
    }
}
