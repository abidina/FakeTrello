﻿using System.Collections.Generic;
using FakeTrello.Models;

namespace FakeTrello.Controllers.Contracts
{
    public interface ICardRepository
    {
        // List of methods to help deliver features
        // Create
        void AddCard(string name, List list, ApplicationUser owner);
        void AddCard(string name, int listId, string ownerId);

        // Read
        List<Card> GetCardsFromList(int listId);
        List<Card> GetCardsFromBoard(int boardId);
        Card GetCard(int cardId);
        List<ApplicationUser> GetCardAttendees(int cardId); 

        // Update
        bool AttachUser(string userId, int cardId); // true: successful, false: not successful
        bool MoveCard(int cardId, int oldListId, int newListId);
        bool CopyCard(int cardId, int newListId, string newOwnerId);

        // Delete
        bool RemoveCard(int cardId);
    }
}