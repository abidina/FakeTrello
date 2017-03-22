using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.DAL
{
    public interface IBoardQuery
    {
        List<Board> GetBoardsFromUser(string userId);
        Board GetBoard(int boardId);
    }
}
