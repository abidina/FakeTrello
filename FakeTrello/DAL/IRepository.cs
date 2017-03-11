using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.DAL
{
    public interface IRepository
    {
        //List of methods to help deliver features

        //Create
        void AddBoard(string name, ApplicationUser owner);
        void AddList(string name, Board board);
        void AddList(string name, int boardId);
        void AddCard(string name, List list, ApplicationUser owner);
        void AddCard(string name, int listId, string ownerId);

        //Read
        List<Card> GetCardsFromList(int listId);
        List<Card> GetCardsFromBoard(int boardId);
        Card GetCard(int cardId);
        List GetList(int listId);
        List<List> GetListsFromBoard(int boardId); // list of Trello Lists
        List<Board> GetBoardsFromUser(string userId);
        Board GetBoard(int boardId);
        List<TrelloUser> GetCardAttachees(int cardId);

        //Update
        bool AttachUser(string userId, int cardId); //true: successful, false: not successful
        bool MoveCard(int cardId, int oldListId, int newListId);
        bool CopyCard(int cardId, int newListId, string newOwnerId);

        //Delete
        void RemoveBoard(int boardId);
        void RemoveList(int listId);
        void RemoveCard(int cardId);
    }
}
