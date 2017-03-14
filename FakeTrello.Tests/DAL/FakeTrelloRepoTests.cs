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

            // IQueryable<Board>
            var queryBoards = fakeBoardTable.AsQueryable(); //Re-express this list as something that understands queries

            Mock<FakeTrelloContext> fakeContext = new Mock<FakeTrelloContext>();
            Mock<DbSet<Board>> mockBoardsSet = new Mock<DbSet<Board>>();

            // Hey LINQ, use the Provider from our FAKE board table/list
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(queryBoards.Provider);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(queryBoards.Expression);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(queryBoards.ElementType);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => queryBoards.GetEnumerator());

            mockBoardsSet.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fakeBoardTable.Add(board));
            fakeContext.Setup(c => c.Boards).Returns(mockBoardsSet.Object); //Context.Boards returns a fakeBoardTable (a list)

            FakeTrelloRepository repo = new FakeTrelloRepository(fakeContext.Object);
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
            List<Board> fakeBoardTable = new List<Board>
            {
                new Board { Name = "My Board" }
            };

            var queryBoards = fakeBoardTable.AsQueryable(); //Re-express this list as something that understands queries

            Mock<FakeTrelloContext> fakeContext = new Mock<FakeTrelloContext>();
            Mock<DbSet<Board>> mockBoardsSet = new Mock<DbSet<Board>>();

            // Hey LINQ, use the Provider from our FAKE board table/list
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(queryBoards.Provider);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(queryBoards.Expression);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(queryBoards.ElementType);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => queryBoards.GetEnumerator());

            mockBoardsSet.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fakeBoardTable.Add(board));
            fakeContext.Setup(c => c.Boards).Returns(mockBoardsSet.Object); //Context.Boards returns a fakeBoardTable (a list)

            //Mock<FakeTrelloContext> mockContext = new Mock<FakeTrelloContext>();
            FakeTrelloRepository repo = new FakeTrelloRepository(fakeContext.Object);

            //Act
            int expectedBoardCount = 1;
            int actualBoardCount = repo.Context.Boards.Count();

            //Assert
            Assert.AreEqual(expectedBoardCount, actualBoardCount);

        }
    }
}
