﻿using AllTodo.Shared.Exceptions;
using AllTodo.Shared.Models.Primitives;
using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllTodo.Shared.Models
{
    public enum TodoState
    {
        NOT_STARTED,
        IN_PROGRESS,
        COMPLETED
    }


    public class TodoDTO
    {
        public int ID { get; set; } = 0;
        public int UserID { get; set; } = -1;
        public string Title { get; set; }
        public string Description { get; set; }
        public TodoState State { get; set; }

        public (bool success, string message) Validate()
        {
            var title_validation = TodoTitle.Validate(Title);
            if (!title_validation.success)
                return title_validation;

            var description_validation = TodoDescription.Validate(Description);
            if (!description_validation.success)
                return description_validation;

            return Todo.Validate(new TodoTitle(Title), new TodoDescription(Description), State);
        }

        public (bool success, string message) ValidateIdentifiers(IUserService userservice, ITodoService todoservice)
        {
            return Todo.ValidateIdentifiers(ID, UserID, userservice, todoservice);
        }

        override
        public string ToString()
        {
            return $"ID: {this.ID}, Title: {this.Title}, Description: {this.Description}, State: {this.State}";
        }

        public Todo GetObject(IUserService userservice, ITodoService todoservice)
        {
            return new Todo(ID, UserID, new TodoTitle(Title), new TodoDescription(Description), State, userservice, todoservice);
        }
    }


    public class Todo
    {
        private readonly int id;
        public int ID { get { return id; } }

        private readonly int user_id;
        public int UserID { get { return user_id; } }

        private readonly TodoTitle title;
        public TodoTitle Title { get { return title; } }

        private readonly TodoDescription description;
        public TodoDescription Description { get { return description; } }

        private TodoState state;
        public TodoState State { get { return state; } }

        public Todo(int id, int user_id, TodoTitle title, TodoDescription description, TodoState state, IUserService userservice, ITodoService todoservice)
        {
            var validation_result = Todo.Validate(title, description, state);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);

            var identifier_validation_result = Todo.ValidateIdentifiers(id, user_id, userservice, todoservice);
            if (!identifier_validation_result.success)
                throw new InvalidInitializationException(identifier_validation_result.message);

            this.id = id;
            this.user_id = user_id;
            this.title = title;
            this.description = description;
            this.state = state;
        }

        public static (bool success, string message) ValidateIdentifiers(int id, int user_id, IUserService userservice, ITodoService todoservice)
        {
            if (!userservice.Exists(user_id))
                return (false, "User does not exist.");

            // TODO: Check that if Todo exists, this user owns it.

            return (true, "Validation Sucessful.");
        }

        public static (bool success, string message) Validate(TodoTitle title, TodoDescription description, TodoState state)
        {
            if (title is null)
                return (false, "Title cannot be null");
            if (description is null)
                return (false, "Description cannot be null");
            return (true, "Validation Successful");
        }

        public TodoDTO GetDTO()
        {
            TodoDTO dto = new TodoDTO();

            dto.ID = this.id;
            dto.UserID = this.user_id;
            dto.Title = this.title.Value;
            dto.Description = this.description.Value;
            dto.State = this.state;

            return dto;
        }
    }
}
